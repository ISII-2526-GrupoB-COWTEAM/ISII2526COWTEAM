using AppForSEII2526.API.DTOs.RentalDTO;
using AppForSEII2526.API.DTOs.RentalDTOs;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

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
        [ProducesResponseType(typeof(RentalForDetailDTO), (int)HttpStatusCode.OK]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetRental(int id)
        {
                RentalForDetailDTO? rental = await _context.Rental
                .Where(r => r.Id == id)

                .Include(r => r.RentDevices)
                .ThenInclude(rd => rd.Device)
                .ThenInclude(device =>device.Model)

                .Include(r => r.ApplicationUser)

                .Select(r => new RentalForDetailDTO(r.Id, r.ApplicationUser.Surname,
                r.DeliveryAddress, r.PaymentMethod, r.RentalDateFrom, r.RentalDateTo, r.RentalDate, r.TotalPrice,
                        
                        r.ApplicationUser.Name!,
                        r.RentDevices.Select(rd => new RentalDeviceDTO(rd.Device.Id, rd.Device.Name, rd.Device.Brand, rd.Device.Color, rd.Device.Year, rd.Device.Model.NameModel, rd.Quantity)).ToList())
                ).FirstOrDefaultAsync();
                
                if (rental == null)
                {
                    _logger.LogWarning("Rental with id {RentalId} not found.", id);
                    return NotFound();
                
                }
                return Ok(rental);
        }
    }
}
