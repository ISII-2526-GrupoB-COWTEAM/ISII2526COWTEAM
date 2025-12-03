using AppForSEII2526.API.DTOs.PurchaseDTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace AppForSEII2526.API.Controllers
{
    
        [Route("api/[controller]")]
        [ApiController]
        public class PurchasesController : ControllerBase
        {
            private readonly ApplicationDbContext _context;
            private readonly ILogger<PurchasesController> _logger;

            public PurchasesController(ApplicationDbContext context, ILogger<PurchasesController> logger)
            {
                _context = context;
                _logger = logger;
            }

            [HttpGet]
            [Route("[action]")]
            [ProducesResponseType(typeof(PurchaseForDetailDTO), (int)HttpStatusCode.OK)]
            [ProducesResponseType((int)HttpStatusCode.NotFound)]
            public async Task<ActionResult> GetPurchase(int id)
            {
                if (_context.Purchase == null)
                {
                    _logger.LogError("Error: Purchases table does not exist");
                    return NotFound();
                }

                var purchase = await _context.Purchase
            .Where(p => p.Id == id)
            .Include(p => p.PurchaseItems)           // Join with PurchaseDevices
                .ThenInclude(pd => pd.Device)          // Then join Devices table
            .Select(p => new PurchaseForDetailDTO(
                p.Id,
                p.ApplicationUser.UserName,
                p.ApplicationUser.Surname,
                p.DeliveryAddress,
                p.PurchaseDate,
                p.TotalPrice,
                p.PurchaseItems
                    .Select(pd => new PurchaseDeviceDTO(
                        pd.Device.Id,
                        pd.Device.Name,
                        pd.Device.Brand,
                        pd.Device.Color,
                        pd.Device.Year,
                        pd.Device.Model.NameModel,
                        pd.Device.PriceForPurchase,
                        pd.Quantity))
                    .ToList(),
                (PaymentMethodTypes)p.PaymentMethod))
            .FirstOrDefaultAsync();


                if (purchase == null)
                {
                    _logger.LogError($"Error: Purchase with id {id} does not exist");
                    return NotFound();
                }

                return Ok(purchase);
            }

            [HttpPost]
            [Route("[action]")]
            [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
            [ProducesResponseType(typeof(string), (int)HttpStatusCode.Conflict)]
            [ProducesResponseType(typeof(PurchaseForDetailDTO), (int)HttpStatusCode.Created)]
            public async Task<ActionResult> CreatePurchase(PurchaseForCreateDTO purchaseForCreate)
            {
                
                if (purchaseForCreate == null)
                {
                    ModelState.AddModelError("Body", "Error! Request body is required.");
                    return BadRequest(new ValidationProblemDetails(ModelState));
                }

                if (purchaseForCreate.PurchaseDevices == null || purchaseForCreate.PurchaseDevices.Count == 0)
                    ModelState.AddModelError("PurchaseDevices", "Error! You must purchase at least one device");

                
                if (purchaseForCreate.PurchaseDate > DateTime.Today.AddDays(1))
                    ModelState.AddModelError("PurchaseDate", "Error! Purchase date cannot be in the far future");

                var user = await _context.ApplicationUser
                    .FirstOrDefaultAsync(u => u.Name == purchaseForCreate.Name && u.Surname == purchaseForCreate.Surname);

                if (user == null)
                    ModelState.AddModelError("User", "Error! User not found");

                if (!ModelState.IsValid)
                    return BadRequest(new ValidationProblemDetails(ModelState));

                
                var deviceNames = purchaseForCreate.PurchaseDevices.Select(pd => pd.Model).ToList();

                var devices = await _context.Device
                    .Include(d => d.Model)
                    .Where(d => deviceNames.Contains(d.Name))
                    .ToListAsync();

                
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
                    var device = devices.FirstOrDefault(d => d.Name == item.Model);

                    if (device == null)
                    {
                        ModelState.AddModelError("PurchaseDevices", $"Error! Device named '{item.Model}' does not exist in the database");
                        continue;
                    }

                    // Quantity del request (si no viene, por defecto 1)
                    var qty = item.Quantity > 0 ? item.Quantity : 1;

                    
                    purchase.PurchaseItems.Add(new PurchaseItem
                    {
                        Device = device,
                        Quantity = qty,
                        Price = device.PriceForPurchase  
                    });
                }

                if (ModelState.ErrorCount > 0)
                    return BadRequest(new ValidationProblemDetails(ModelState));

                
                purchase.TotalPrice = purchase.PurchaseItems.Sum(li => li.Price * li.Quantity);

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

               
                var purchaseCreated = await _context.Purchase
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
                                pi.Device.Name,
                                pi.Device.Brand,
                                pi.Device.Color,
                                pi.Device.Year,
                                pi.Device.Model.NameModel,   
                                pi.Price,
                                pi.Quantity))
                            .ToList(),
                        p.PaymentMethod))
                    .FirstOrDefaultAsync();

                return CreatedAtAction("GetPurchase", new { id = purchase.Id }, purchaseCreated);
            }

        }
    
}
