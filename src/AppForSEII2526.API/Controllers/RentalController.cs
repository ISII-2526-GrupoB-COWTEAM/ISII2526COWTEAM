using AppForSEII2526.API.DTOs.RentalDTO;
using AppForSEII2526.API.DTOs.RentalDTOs;
using AppForSEII2526.API.Models;
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
        [ProducesResponseType(typeof(RentalForDetailDTO), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult> GetRental(int id)
        {
            var rental = await _context.Rental
                .Where(r => r.Id == id)
                .Select(r => new RentalForDetailDTO(
                    r.Id,
                    r.ApplicationUser.Name,
                    r.ApplicationUser.Surname,
                    r.DeliveryAddress,
                    r.TotalPrice,
                    r.RentalDate,
                    r.RentalDateFrom,
                    r.RentalDateTo,
                    r.RentDevices.Select(rd => new RentalDeviceDTO(
                        rd.Device.Id,
                        rd.Device.Name,
                        rd.Device.Brand,
                        rd.Device.Color,
                        rd.Device.Year,
                        rd.Device.Model.NameModel,
                        rd.Price
                    )).ToList(),
                    r.PaymentMethod
                ))
                .FirstOrDefaultAsync(); 

            if (rental == null)
            {
                _logger.LogWarning("Rental with id {RentalId} not found.", id);
                return NotFound();
            }
            return Ok(rental);
        }

        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Conflict)]
        [ProducesResponseType(typeof(RentalForDetailDTO), (int)HttpStatusCode.Created)]
        public async Task<ActionResult> CreateRental(RentalForCreateDTO rentalForCreate)
        {
            //Validaciones iniciales
            if (rentalForCreate.RentalDateFrom <=DateTime.Today)
                ModelState.AddModelError("RentalDateFrom", "Error! Your rental date must start later than today");
            if (rentalForCreate.RentalDateFrom >= rentalForCreate.RentalDateTo)
                ModelState.AddModelError("RentalDateTo", "Error! The end date must be after the start date");
            if (rentalForCreate.RentalDevices.Count == 0)
                ModelState.AddModelError("RentalDevices", "Error! You must rent at least one device");

            var user =await _context.ApplicationUser.FirstOrDefaultAsync(u => u.Name == rentalForCreate.Name && u.Surname == rentalForCreate.Surname);
            if (user == null)
                ModelState.AddModelError("User", "Error! User not found");

            var deviceNames = rentalForCreate.RentalDevices.Select(rd => rd.Name).ToList();


            var devices =  _context.Device
                .Include(d => d.Model)
                .Where(d => deviceNames.Contains(d.Name))
                .ToList();

            Rental rental = new Rental(
                rentalForCreate.DeliveryAddress,
                rentalForCreate.PaymentMethod,
                user,
                rentalForCreate.RentalDate,
                rentalForCreate.RentalDateFrom,
                rentalForCreate.RentalDateTo,
                new List<RentDevice>());

           foreach(var item in rentalForCreate.RentalDevices)
            {
                var device = devices.FirstOrDefault(d => d.Name == item.Name);

                if (device == null)
                {
                    ModelState.AddModelError("ReviewItems", $"Error! Device named '{item.Name}' does not exist in the database");
                }
                else
                {
                    rental.RentDevices.Add(new RentDevice
                    {
                        Device = device,
                        Quantity = 1,
                        Price = device.PriceForRent
                    });
                }
            }
            if (ModelState.ErrorCount > 0)
                return BadRequest(new ValidationProblemDetails(ModelState));
            _context.Add(rental);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "An error occurred while creating a new rental.");
                return Conflict("A database error occurred while processing your request.");
            }

            var rentalCreated = await _context.Rental
                .Where(r => r.Id == rental.Id)
                .Select(r => new RentalForDetailDTO(
                    r.Id,
                    r.ApplicationUser.Name,
                    r.ApplicationUser.Surname,
                    r.DeliveryAddress,
                    r.TotalPrice,
                    r.RentalDate,
                    r.RentalDateFrom,
                    r.RentalDateTo,

                    r.RentDevices.Select(rd => new RentalDeviceDTO(
                        rd.Device.Id,
                        rd.Device.Name,
                        rd.Device.Brand,
                        rd.Device.Color,
                        rd.Device.Year,
                        rd.Device.Model.NameModel,
                        rd.Price
                    )).ToList(),
                    r.PaymentMethod

                ))
                .FirstOrDefaultAsync();

            return CreatedAtAction("GetRental", new { id = rental.Id }, rentalCreated);
        }
    }
}
