using Microsoft.AspNetCore.Mvc;

namespace AnnaSweetBakery.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // GET: api/home
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Anna's Sweet Bakery API is running...");
        }

        // GET: api/home/privacy
        [HttpGet("privacy")]
        public IActionResult Privacy()
        {
            return Ok("Privacy endpoint");
        }
    }
}