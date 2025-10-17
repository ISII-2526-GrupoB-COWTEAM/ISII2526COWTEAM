using AppForSEII2526.API.DTOs.RepairDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppForSEII2526.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class RepairController : ControllerBase
    {

        private readonly ApplicationDbContext _context;
        private readonly ILogger<RepairController> _logger;

        public RepairController(ApplicationDbContext context, ILogger<RepairController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<RepairDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetRepairsDTO(decimal op1, decimal op2)
        {

            var repairs = await _context.Repair.
                Select(r => new RepairDTO(
                    r.Id,
                    r.Name,
                    r.Scale.Name,
                    r.Cost,
                    r.Description
                )).
                ToListAsync();

            return Ok(repairs);

        }
    }
}
