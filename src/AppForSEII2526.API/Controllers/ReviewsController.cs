using AppForSEII2526.API.DTOs;
using AppForSEII2526.API.DTOs.ReviewDTO;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace AppForSEII2526.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DevicesController> _logger;

        public ReviewsController(ApplicationDbContext context, ILogger<DevicesController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<ReviewDetailDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetReview(int id)
        {
            var review = await _context.Review
                .Where(r => r.Id == id)
                    .Include(r => r.ReviewItems)
                        .ThenInclude(ri => ri.Device)
                            .ThenInclude(device => device.Model)
                .Select(r => new ReviewDetailDTO(r.Id, r.ApplicationUser.Name,
                        r.ApplicationUser.Country,
                        r.ReviewTitle, r.DateOfReview,
                        r.ReviewItems
                        .Select(ri => new ReviewItemDTO(ri.Device.Id,
                            ri.Device.Name, ri.Device.Model.NameModel,
                            ri.Device.Year, ri.Rating,
                            ri.Comments)).ToList<ReviewItemDTO>()))
                .FirstOrDefaultAsync();

            if (review == null)
            {
                _logger.LogError($"Error: Review with id {id} does not exist");
                return NotFound();
            }

            return Ok(review);
        }


        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(ReviewDetailDTO), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Conflict)]
        public async Task<ActionResult> CreateReview(ReviewForCreateDTO reviewForCreate)
        {
            // Validaciones iniciales
            if (string.IsNullOrWhiteSpace(reviewForCreate.ReviewTitle))
                ModelState.AddModelError("Title", "Error! The review title is required");

            if (string.IsNullOrWhiteSpace(reviewForCreate.ApplicationUserCountry))
                ModelState.AddModelError("Country", "Error! The country is required");

            if (reviewForCreate.ReviewItems == null || reviewForCreate.ReviewItems.Count == 0)
                ModelState.AddModelError("ReviewItems", "Error! You must include at least one device to review");

            // Validación del usuario (si se proporciona un nombre de cliente opcional)
            ApplicationUser user = null;
            if (!string.IsNullOrWhiteSpace(reviewForCreate.ApplicationUserName))
            {
                user = _context.ApplicationUser.FirstOrDefault(u => u.Name == reviewForCreate.ApplicationUserName);
                if (user == null)
                    ModelState.AddModelError("ReviewApplicationUser", "Error! The provided username is not registered");
            }

            if (ModelState.ErrorCount > 0)
                return BadRequest(new ValidationProblemDetails(ModelState));

            // Obtener los dispositivos incluidos en la reseña (traemos las entidades Device completas)
            var deviceNames = reviewForCreate.ReviewItems.Select(ri => ri.Name).ToList();

            var devices = _context.Device
                .Include(d => d.Model)
                .Where(d => deviceNames.Contains(d.Name))
                .ToList();

            // Crear el objeto Review principal
            Review review = new Review(
                user,
                DateTime.Now,
                reviewForCreate.ReviewTitle,
                new List<ReviewItem>()
            );

            // Procesar cada ReviewItem
            foreach (var item in reviewForCreate.ReviewItems)
            {
                var device = devices.FirstOrDefault(d => d.Name == item.Name);

                if (device == null)
                {
                    ModelState.AddModelError("ReviewItems", $"Error! Device named '{item.Name}' does not exist in the database");
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(item.Comments))
                        ModelState.AddModelError("Comment", $"Error! You must include a comment for device '{item.Name}'");

                    if (item.Rating <= 0 || item.Rating > 5)
                        ModelState.AddModelError("Rating", $"Error! Rating for device '{item.Name}' must be between 1 and 5");

                    // Si todo es válido, creamos el ReviewItem
                    if (ModelState.ErrorCount == 0)
                    {
                        review.ReviewItems.Add(new ReviewItem(
                            device,
                            review,
                            item.Comments,
                            item.Rating
                        ));
                    }
                }
            }

            // Si hubo errores en los items, devolvemos BadRequest
            if (ModelState.ErrorCount > 0)
                return BadRequest(new ValidationProblemDetails(ModelState));

            // Guardamos en la base de datos
            _context.Add(review);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                ModelState.AddModelError("Review", "Error! There was an error while saving your review, please try again later");
                return Conflict("Error: " + ex.Message);
            }

            // Creamos el DTO de salida
            var reviewDetail = new ReviewDetailDTO(
                review.Id,
                review.ReviewTitle,
                review.ApplicationUser.Country,
                review.ApplicationUser.Name,
                review.DateOfReview,
                review.ReviewItems.Select(ri => new ReviewItemDTO(
                    ri.Device.Id,
                    ri.Device.Name,
                    ri.Device.Model.NameModel,
                    ri.Device.Year,
                    ri.Rating,
                    ri.Comments
                )).ToList()
            );

            return CreatedAtAction("GetReview", new { id = review.Id }, reviewDetail);
        }

    }
}