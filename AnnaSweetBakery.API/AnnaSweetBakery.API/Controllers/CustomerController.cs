using AnnaSweetBakery.API.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AnnaSweetBakery.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(
        AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
        Roles = "Customer"
    )]
    public class CustomerController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public CustomerController(
            UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet("profile")]
        public async Task<IActionResult> Profile()
        {
            var userId =
                User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var user =
                await _userManager.FindByIdAsync(userId);

            if (user == null)
                return NotFound();

            return Ok(new
            {
                user.Name,
                user.Email,
                user.Address
            });
        }
    }
}