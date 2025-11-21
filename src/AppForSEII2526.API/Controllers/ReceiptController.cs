using AppForSEII2526.API.DTOs.ReceiptDTO;
using AppForSEII2526.API.DTOs.ReviewDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace AppForSEII2526.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReceiptController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ReceiptController> _logger;
        
        public ReceiptController(ApplicationDbContext context, ILogger<ReceiptController> logger)
        {
            _context = context;
            _logger = logger;
        }


        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<ReceiptDetailDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetReceiptDetail(int id) {

            var receipt = await _context.Receipt
                .Where(r => r.Id == id)
                    .Include(r => r.ReceiptItem)
                        .ThenInclude(ri => ri.Repair)
                            .ThenInclude(repair => repair.Scale)
                .Select(r => new ReceiptDetailDTO(
                        r.Id,
                        r.ApplicationUser.Name,
                        r.ApplicationUser.Surname,
                        r.DeliveryAddress,
                        r.ReceiptDate,
                        r.TotalPrice,
                        r.ReceiptItem
                        .Select(ri => new ReceiptItemDTO(
                            ri.Repair.Id,
                            ri.Repair.Name,
                            ri.Repair.Scale.Name,
                            ri.Repair.Cost,
                            ri.Model))
                        .ToList<ReceiptItemDTO>()))
                .FirstOrDefaultAsync();

            if (receipt == null)
            {
                _logger.LogError($"Error: Receipt with id {id} does not exist");
                return NotFound();
            }

            return Ok(receipt);
        }


        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(ReceiptDetailDTO), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Conflict)]
        public async Task<ActionResult> CreateReceipt(ReceiptForCreate receiptForCreate)
        {
            // 1️⃣ Validaciones básicas
            if (string.IsNullOrWhiteSpace(receiptForCreate.ApplicationUserName))
                ModelState.AddModelError("ApplicationUserName", "Error! The user's name is required");

            if (string.IsNullOrWhiteSpace(receiptForCreate.ApplicationUserSurname))
                ModelState.AddModelError("ApplicationUserSurname", "Error! The user's surname is required");


            if (receiptForCreate.ReceiptItems == null || receiptForCreate.ReceiptItems.Count == 0)
                ModelState.AddModelError("ReceiptItems", "Error! You must include at least one repair to contract");

            
            if (string.IsNullOrWhiteSpace(receiptForCreate.DeliveryAddress))
                ModelState.AddModelError("DeliveryAddress", "Error! The delivery address is required");
            else if (receiptForCreate.DeliveryAddress != null && !(receiptForCreate.DeliveryAddress.Contains("Calle") || (receiptForCreate.DeliveryAddress.Contains("Avenida"))))
                ModelState.AddModelError("DeliveryAdress", "Error en la dirección de envío. Por favor, introduce una dirección válida incluyendo las palabras Calle o Avenida");


            if (ModelState.ErrorCount > 0)
                return BadRequest(new ValidationProblemDetails(ModelState));

            // 2️⃣ Buscar usuario existente
            var user = await _context.ApplicationUser
                .FirstOrDefaultAsync(u => u.Name == receiptForCreate.ApplicationUserName
                                       && u.Surname == receiptForCreate.ApplicationUserSurname);

            if (user == null)
            {
                ModelState.AddModelError("ApplicationUser", "Error! No user found with that name and surname.");
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            // 3️⃣ Recuperar las reparaciones desde la BD incluyendo Scale
            var repairIds = receiptForCreate.ReceiptItems.Select(ri => ri.RepairID).ToList();
            var repairs = await _context.Repair
                .Include(r => r.Scale) // 🔹 importante
                .Where(r => repairIds.Contains(r.Id))
                .ToListAsync();

            // 4️⃣ Crear el Receipt principal
            var receipt = new Receipt
            {
                ApplicationUser = user,
                DeliveryAddress = receiptForCreate.DeliveryAddress,
                PaymentMethod = PaymentMethodTypes.CreditCard, // o sacar del DTO si lo incluyes
                ReceiptDate = DateTime.Now,
                ReceiptItem = new List<ReceiptItem>()
            };

            // 5️⃣ Crear cada ReceiptItem
            foreach (var item in receiptForCreate.ReceiptItems)
            {
                var repair = repairs.FirstOrDefault(r => r.Id == item.RepairID);

                if (repair == null)
                {
                    ModelState.AddModelError("ReceiptItems", $"Error! Repair with ID {item.RepairID} not found.");
                    continue;
                }

                if (string.IsNullOrWhiteSpace(item.Model))
                {
                    ModelState.AddModelError("DeviceModel", $"Error! You must include the device model for repair '{repair.Name}'.");
                    continue;
                }

                receipt.ReceiptItem.Add(new ReceiptItem(repair, receipt, item.Model));
            }

            if (ModelState.ErrorCount > 0)
                return BadRequest(new ValidationProblemDetails(ModelState));

            // 6️⃣ Calcular total
            receipt.TotalPrice = receipt.ReceiptItem.Sum(ri => ri.Repair.Cost);

            // 7️⃣ Guardar en la BD
            _context.Add(receipt);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Conflict("Error while saving the receipt: " + ex.Message);
            }

            // 8️⃣ Mapear al DTO de salida
            var receiptDetail = new ReceiptDetailDTO(
                receipt.Id,
                user.Name,
                user.Surname,
                receipt.DeliveryAddress,
                receipt.ReceiptDate,
                receipt.TotalPrice,
                receipt.ReceiptItem.Select(ri => new ReceiptItemDTO(
                    ri.Repair.Id,
                    ri.Repair.Name,
                    ri.Repair.Scale?.Name ?? "Unknown", 
                    ri.Repair.Cost,
                    ri.Model
                )).ToList()
            );

            return Created("", receiptDetail); // La URL queda vacía

        }






    }
}
