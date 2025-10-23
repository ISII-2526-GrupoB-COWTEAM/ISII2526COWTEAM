using AppForSEII2526.API.DTOs.DeviceDTO;
using AppForSEII2526.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
                .Select(d => new ReviewDetailDTO())

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
