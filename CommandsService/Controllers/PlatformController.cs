using Microsoft.AspNetCore.Mvc;

namespace CommandsService.Controllers
{
    [Route("api/c/[controller]")]
    [ApiController]
    public class PlatformController : ControllerBase
    {
        public PlatformController()
        {
                
        }
        [HttpPost]
        public IActionResult GetAllPlatforms()
        {
            return Ok("List of platforms");
        }
    }
}
