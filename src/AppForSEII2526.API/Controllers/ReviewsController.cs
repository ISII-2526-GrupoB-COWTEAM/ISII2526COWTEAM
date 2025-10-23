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
                .Select(r => new ReviewDetailDTO( r.Id, r.ApplicationUser.Name,
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

    }
}
