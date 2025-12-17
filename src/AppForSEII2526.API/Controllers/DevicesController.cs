using AppForSEII2526.API.DTOs.DeviceDTO;
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
        [ProducesResponseType(typeof(IList<DeviceForReviewDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetDevicesForReview(string? filtroBrand, int? filtroYear)
        {
            var devices = await _context.Device
                .Where(d => (d.Brand.Contains(filtroBrand) || filtroBrand == null) && (d.Year == filtroYear || filtroYear == null))
                .Select(d => new DeviceForReviewDTO(d.Id, d.Name, d.Brand, d.Color, d.Year, d.Model.NameModel))
                .ToListAsync();
            return Ok(devices);
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<DeviceForRentalDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetDevicesForRental(string? filtroModel, double? filtroPrice)
        {
            var devices = await _context.Device
                .Where(d => (string.IsNullOrEmpty(filtroModel) || d.Name.Contains(filtroModel) || d.Model.NameModel.Contains(filtroModel)) 
                            && (filtroPrice == null || d.PriceForRent <= filtroPrice))
                .Select(d => new DeviceForRentalDTO(d.Id, d.Name, d.Brand, d.Color, d.Year, d.Model.NameModel, d.PriceForRent))
                .ToListAsync();
            return Ok(devices);
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<DeviceForPurchaseDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<List<DeviceForPurchaseDTO>>> GetDevicesForPurchase(string? name, string? color)
        {
            var query = _context.Device
                .Include(d => d.Model)
                .AsQueryable();

            if (!string.IsNullOrEmpty(name))
                query = query.Where(d => d.Name.Contains(name));

            if (!string.IsNullOrEmpty(color))
                query = query.Where(d => d.Color == color);

            var devices = await _context.Device
                .Include(d => d.Model)
                .Where(d =>
                    (string.IsNullOrEmpty(name) || d.Name.Contains(name)) &&
                    (string.IsNullOrEmpty(color) || d.Color.Contains(color))
                )
                .OrderBy(d => d.Name)
                .ThenBy(d => d.Brand)
                .Select(d => new DeviceForPurchaseDTO
                (
                    d.Id, d.Name, d.Color, (decimal)d.PriceForPurchase, d.Model.NameModel, d.Brand, d.Year
                )).ToListAsync();

         /*   // Error filtro de color -> no hay resultados  
            if (!string.IsNullOrEmpty(color) && !devices.Any())
                return BadRequest("El color indicado no existe en el catálogo de dispositivos.");

            // Error filtro de nombre -> no hay resultados 
            if (!string.IsNullOrEmpty(name) && !devices.Any())
                return BadRequest("No existe ningún dispositivo con ese nombre.");
         */
            // Si no hay filtros (name == null, color == null) y la tabla está vacía,
            // devolveremos Ok([]) y tu test de tabla vacía lo comprobará.
            return Ok(devices);

        }
       

    }
}
