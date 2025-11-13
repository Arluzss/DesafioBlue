using Microsoft.AspNetCore.Mvc;
using Infrastructure.Context;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthController : ControllerBase
    {
        private readonly AppDbContext _context;

        public HealthController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Check()
        {
            bool canConnect = _context.Database.CanConnect(); 
            return Ok(new { connected = canConnect });
            
        }
    }
}
