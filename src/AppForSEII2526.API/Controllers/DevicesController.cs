using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AppForSEII2526.API.DTOs;
using AppForSEII2526.API.DTOs.PurchaseDTO;


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
        [ProducesResponseType(typeof(IList<DeviceForPurchaseDTO>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetDevicesForPurchase()
        {
            var devices = await _context.Device
                .Select(d => new DeviceForPurchaseDTO
                (
                    d.Id,d.Name,d.Color,d.PriceForPurchase, d.Model.NameModel,d.Brand
                ))
                .ToListAsync();
            return Ok(devices);

        }
    }
}
