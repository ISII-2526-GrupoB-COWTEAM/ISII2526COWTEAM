using AppForSEII2526.API.DTOs.ReceiptDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace AppForSEII2526.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReceiptController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<RepairController> _logger;
        
        public ReceiptController(ApplicationDbContext context, ILogger<RepairController> logger)
        {
            _context = context;
            _logger = logger;
        }


        [HttpGet]
        [Route("[action]")]
        [ProducesResponseType(typeof(IList<ReceiptDetailDTO>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetReceiptDetail(int id) {

            var receipt = await _context.Receipt
                .Where(r => r.Id == id)
                    .Include(r => r.ReceiptItem)
                        .ThenInclude(ri => ri.Repair)
                            .ThenInclude(repair => repair.Scale)
                .Select(r => new ReceiptDetailDTO(
                        r.Id,
                        r.ApplicationUser.Name,
                        r.ApplicationUser.Surname,
                        r.DeliveryAddress,
                        r.ReceiptDate,
                        r.TotalPrice,
                        r.ReceiptItem
                        .Select(ri => new ReceiptItemDTO(
                            ri.Repair.Id,
                            ri.Repair.Name,
                            ri.Repair.Scale.Name,
                            ri.Repair.Cost,
                            ri.Model))
                        .ToList<ReceiptItemDTO>()))
                .FirstOrDefaultAsync();

            if (receipt == null)
            {
                _logger.LogError($"Error: Receipt with id {id} does not exist");
                return NotFound();
            }

            return Ok(receipt);
        }





    }
}
