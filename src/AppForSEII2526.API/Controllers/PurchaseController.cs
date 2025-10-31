using AppForSEII2526.API.DTOs.PurchaseDTO;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace AppForSEII2526.API.Controllers
{
    public class PurchaseController : Controller
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
                        pd.Device.PriceForPurchase,
                        pd.Device.Brand,
                        pd.Device.Model.NameModel,
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
        }

    }
}
