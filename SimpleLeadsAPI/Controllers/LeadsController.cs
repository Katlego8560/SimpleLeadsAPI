using Microsoft.AspNetCore.Mvc;
using SimpleLeadsAPI.Services;


namespace SimpleLeadsAPI.Controllers
{
    [ApiController]  
    [Route("api/[controller]")]  
    public class LeadsController : ControllerBase 
    {
        private readonly ApplicationDbContext _context; 
        public LeadsController(ApplicationDbContext context)
        {
            _context = context;
        }
        
        [HttpGet("{CellNumber}")] 
        public IActionResult GetLead(string CellNumber) 
        {
            var leads = _context
                .Leads
                .FirstOrDefault(item => item.CellNumber == CellNumber);

            if (leads == null)
            {
                return Problem(
                    "Lead not found",
                    string.Empty,
                    404
                    );
            }
            else
            { 
                return Ok(leads);
            }
        }
    }
}
