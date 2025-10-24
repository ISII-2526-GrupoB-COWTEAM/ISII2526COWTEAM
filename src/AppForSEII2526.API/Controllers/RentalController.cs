using AppForSEII2526.API.DTOs.RentalDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppForSEII2526.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RentalController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DevicesController> _logger;

        public RentalController(ApplicationDbContext context, ILogger<DevicesController> logger)
        {
            _context = context;
            _logger = logger;
        }
        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<RentalDeviceDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetDevicesForRental(string? filtroBrand, double? filtroPrice)
            {
            var devices = await _context.Device
                .Where(d => (d.Brand.Contains(filtroBrand) || filtroBrand == null) && (d.PriceForRent <= filtroPrice || filtroPrice == null))
                .Select(d => new RentalDeviceDTO(d.Id, d.Name, d.Brand, d.Color, d.Year, d.Model.NameModel, d.PriceForRent))
                .ToListAsync();
            return Ok(devices);
        }

    }
}
