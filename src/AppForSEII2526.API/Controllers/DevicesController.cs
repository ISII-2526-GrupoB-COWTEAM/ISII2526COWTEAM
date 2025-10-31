using AppForSEII2526.API.DTOs.DeviceDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppForSEII2526.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DevicesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DevicesController> _logger;

        public DevicesController(ApplicationDbContext context, ILogger<DevicesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<DeviceForReviewDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetDevicesForReview(string? filtroBrand, int? filtroYear)
        {
            var devices = await _context.Device
                .Where(d=> (d.Brand.Contains(filtroBrand) || filtroBrand == null) && (d.Year==filtroYear || filtroYear == null))
                .Select(d=>new DeviceForReviewDTO(d.Id, d.Name, d.Brand, d.Color, d.Year, d.Model.NameModel))
                .ToListAsync();
            return Ok(devices);
        }
        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Conflict)]
        [ProducesResponseType(typeof(PurchaseForDetailDTO), (int)HttpStatusCode.Created)]
        public async Task<ActionResult> CreatePurchase(PurchaseForCreateDTO purchaseForCreate)
        {
            // ── Validaciones iniciales ──────────────────────────────────────────────────
            if (purchaseForCreate == null)
            {
                ModelState.AddModelError("Body", "Error! Request body is required.");
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            if (purchaseForCreate.PurchaseDevices == null || purchaseForCreate.PurchaseDevices.Count == 0)
                ModelState.AddModelError("PurchaseDevices", "Error! You must purchase at least one device");

            // (Para compras directas no validamos rangos de fechas; solo la fecha de compra)
            if (purchaseForCreate.PurchaseDate > DateTime.Today.AddDays(1))
                ModelState.AddModelError("PurchaseDate", "Error! Purchase date cannot be in the far future");

            var user = await _context.ApplicationUser
                .FirstOrDefaultAsync(u => u.Name == purchaseForCreate.Name && u.Surname == purchaseForCreate.Surname);

            if (user == null)
                ModelState.AddModelError("User", "Error! User not found");

            if (!ModelState.IsValid)
                return BadRequest(new ValidationProblemDetails(ModelState));

            // ── Cargar dispositivos por nombre (según tu patrón anterior) ────────────────
            var deviceNames = purchaseForCreate.PurchaseDevices.Select(pd => pd.Name).ToList();

            var devices = await _context.Device
                .Include(d => d.Model)
                .Where(d => deviceNames.Contains(d.Name))
                .ToListAsync();

            // ── Construir la entidad Purchase con líneas ────────────────────────────────
            var purchase = new Purchase
            {
                DeliveryAddress = purchaseForCreate.DeliveryAddress,
                PaymentMethod = purchaseForCreate.PaymentMethod,
                ApplicationUser = user,
                PurchaseDate = purchaseForCreate.PurchaseDate,
                PurchaseItems = new List<PurchaseItem>()
            };

            foreach (var item in purchaseForCreate.PurchaseDevices)
            {
                var device = devices.FirstOrDefault(d => d.Name == item.Name);

                if (device == null)
                {
                    ModelState.AddModelError("PurchaseDevices", $"Error! Device named '{item.Name}' does not exist in the database");
                    continue;
                }

                // Quantity del request (si no viene, por defecto 1)
                var qty = item.Quantity > 0 ? item.Quantity : 1;

                // UnitPrice: tomamos el precio del catálogo (Device.PurchasePrice) para la transacción
                purchase.PurchaseItems.Add(new PurchaseItem
                {
                    Device = device,
                    Quantity = qty,
                    UnitPrice = device.PurchasePrice   // si tu campo se llama distinto (Price, Cost...), cámbialo aquí
                });
            }

            if (ModelState.ErrorCount > 0)
                return BadRequest(new ValidationProblemDetails(ModelState));

            // TotalPrice como suma de líneas (si tu dominio lo calcula en DB/trigger, puedes omitir esto)
            purchase.TotalPrice = purchase.PurchaseItems.Sum(li => li.UnitPrice * li.Quantity);

            _context.Add(purchase);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "An error occurred while creating a new purchase.");
                return Conflict("A database error occurred while processing your request.");
            }

            // ── Proyección a DTO de detalle coherente con lo que ya usamos ──────────────
            var purchaseCreated = await _context.Purchases
                .Where(p => p.Id == purchase.Id)
                .Include(p => p.ApplicationUser)
                .Include(p => p.PurchaseItems).ThenInclude(pi => pi.Device).ThenInclude(d => d.Model)
                .Select(p => new PurchaseForDetailDTO(
                    p.Id,
                    p.ApplicationUser.UserName,
                    p.ApplicationUser.Surname,
                    p.DeliveryAddress,
                    p.PurchaseDate,
                    p.TotalPrice,
                    p.PurchaseItems
                        .Select(pi => new PurchaseDeviceDTO(
                            pi.Device.Id,
                            /* purchasePrice mostrado en el detalle: el de la transacción */
                            pi.UnitPrice,
                            pi.Device.Brand,
                            pi.Device.Model.Name,   // importante: el texto del modelo
                            pi.Quantity))
                        .ToList(),
                    p.PaymentMethod))
                .FirstOrDefaultAsync();

            return CreatedAtAction("GetPurchase", new { id = purchase.Id }, purchaseCreated);
        }

    }
}
