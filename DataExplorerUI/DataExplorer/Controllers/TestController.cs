using Microsoft.AspNetCore.Mvc;

namespace DataExplorer.Controllers
{
    [ApiController]
    [Route("api/test")]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public IActionResult Ping()
        {
            return Ok("✅ API is running!");
        }
    }
}
