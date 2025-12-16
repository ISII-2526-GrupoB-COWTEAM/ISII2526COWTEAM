using AppForSEII2526.API.DTOs.RentalDTO;
using AppForSEII2526.API.DTOs.RentalDTOs;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.IdentityModel.Tokens;
using System.Net;

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
                        rd.Price,
                        rd.Quantity
                    )).ToList(),
                    r.PaymentMethod
                ))
                .FirstOrDefaultAsync();

            if (rental == null)
            {
                _logger.LogError("Rental with id {RentalId} not found.", id); //antes era LogWarning
                return NotFound();
            }
            _logger.LogInformation("Rental with id {Id} retrieved successfully.", id);//nuevo para distribuidos


            return Ok(rental);
        }

        
        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Conflict)]
        [ProducesResponseType(typeof(RentalForDetailDTO), (int)HttpStatusCode.Created)]
        public async Task<ActionResult> CreateRental(RentalForCreateDTO rentalForCreate)
        {

            if (rentalForCreate == null)
            {
                ModelState.AddModelError("Body", "Error! Request body is required.");
                return BadRequest(new ValidationProblemDetails(ModelState));
            }
            
            if (rentalForCreate.DeliveryAddress != null && !rentalForCreate.DeliveryAddress.Contains("Calle") && !rentalForCreate.DeliveryAddress.Contains("Carretera"))
            {
                ModelState.AddModelError("DeliveryAddress", "Error! Por favor, introduce una dirección válida incluyendo las palabras Calle o Carretera");
                return BadRequest(new ValidationProblemDetails(ModelState));
            }
            
            var rentalDevices = rentalForCreate.RentalDevices ?? new List<RentalDeviceDTO>();

            
            if (rentalForCreate.RentalDateFrom <= DateTime.Today)
                ModelState.AddModelError("RentalDateFrom", "Error! Your rental date must start later than today");

            if (rentalForCreate.RentalDateFrom >= rentalForCreate.RentalDateTo)
                ModelState.AddModelError("RentalDateTo", "Error! The end date must be after the start date");

            if (!rentalDevices.Any())
                ModelState.AddModelError("RentalDevices", "Error! You must rent at least one device");

            var user = await _context.ApplicationUser
                .FirstOrDefaultAsync(u => u.Name == rentalForCreate.Name &&
                                          u.Surname == rentalForCreate.Surname);

            if (user == null)
                ModelState.AddModelError("User", "Error! User not found");

            
            if (ModelState.ErrorCount > 0)
                return BadRequest(new ValidationProblemDetails(ModelState));

            
            var deviceNames = rentalDevices.Select(rd => rd.Name).ToList();

            var devices = await _context.Device
                .Include(d => d.Model)
                .Where(d => deviceNames.Contains(d.Name))
                .ToListAsync();

            
            var rental = new Rental(
                rentalForCreate.DeliveryAddress,
                rentalForCreate.PaymentMethod,
                user,
                rentalForCreate.RentalDate,
                rentalForCreate.RentalDateFrom,
                rentalForCreate.RentalDateTo,
                new List<RentDevice>());

            var numDays = (rental.RentalDateTo - rental.RentalDateFrom).TotalDays;
            rental.TotalPrice = 0;

            foreach (var item in rentalDevices)
            {
                var device = devices.FirstOrDefault(d => d.Name == item.Name);

                if (device == null)
                {
                    
                    ModelState.AddModelError(
                        "RentalDevices",
                        $"Error! Device named '{item.Name}' is not available for being rented from {rentalForCreate.RentalDateFrom.ToShortDateString()} to {rentalForCreate.RentalDateTo.ToShortDateString()}");
                }
                else
                {
                    rental.RentDevices.Add(new RentDevice
                    {
                        Device = device,
                        Quantity = 1,
                        Price = device.PriceForRent
                    });

                    
                    item.PriceForRent = device.PriceForRent;
                }
            }

            
            if (ModelState.ErrorCount > 0)
                return BadRequest(new ValidationProblemDetails(ModelState));

            
            rental.TotalPrice = rental.RentDevices.Sum(rd => rd.Price * numDays);

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

            
            var rentalDetail = new RentalForDetailDTO(
                rental.Id,
                user.Name,
                user.Surname,
                rental.DeliveryAddress,
                rental.TotalPrice,
                rental.RentalDate,
                rental.RentalDateFrom,
                rental.RentalDateTo,
                rentalDevices,
                rental.PaymentMethod);

            return CreatedAtAction("GetRental", new { id = rental.Id }, rentalDetail);
        }
    }
}
