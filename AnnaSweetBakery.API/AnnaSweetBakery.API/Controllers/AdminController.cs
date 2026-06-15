using AnnaSweetBakery.API.ContextDBConfig;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace AnnaSweetBakery.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(
    AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
    Roles = "Admin"
)]
    public class AdminController : ControllerBase
    {
        private readonly FinalDBContext _context;

        public AdminController(FinalDBContext context)
        {
            _context = context;
        }

        [HttpGet("orders")]
        public IActionResult GetOrders()
        {
            return Ok("ADMIN ROUTE WORKS");
        }
    }
}