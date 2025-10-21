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

    }
}
