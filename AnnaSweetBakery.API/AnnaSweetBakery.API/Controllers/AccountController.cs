using AnnaSweetBakery.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AnnaSweetBakery.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        // ---------------- REGISTER ----------------
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                Name = model.Name,
                Address = model.Address
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            // ADD ROLE
            if (user.Email == "admin@test.com")
            {
                await _userManager.AddToRoleAsync(user, "Admin");
            }
            else
            {
                await _userManager.AddToRoleAsync(user, "Customer");
            }

            return Ok(new
            {
                message = "User registered successfully"
            });
        }

        // ---------------- LOGIN ----------------
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginViewModel login)
        {
            if (login == null ||
                string.IsNullOrWhiteSpace(login.Email) ||
                string.IsNullOrWhiteSpace(login.Password))
            {
                return BadRequest(new
                {
                    message = "Email and password are required"
                });
            }

            // FIND USER
            var user = await _userManager.FindByEmailAsync(login.Email);

            if (user == null)
            {
                return Unauthorized(new
                {
                    message = "Invalid email or password"
                });
            }

            // CHECK PASSWORD
            var validPassword = await _userManager.CheckPasswordAsync(user, login.Password);

            if (!validPassword)
            {
                return Unauthorized(new
                {
                    message = "Invalid email or password"
                });
            }

            // GENERATE TOKEN
            var token = GenerateJwtToken(user);

            return Ok(new
            {
                token = token,
                email = user.Email
            });
        }

        // ---------------- MAKE ADMIN ----------------
        [HttpPost("make-admin/{email}")]
        public async Task<IActionResult> MakeAdmin(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
                return NotFound();

            await _userManager.AddToRoleAsync(user, "Admin");

            return Ok(new
            {
                message = "User is now Admin"
            });
        }

        // ---------------- DELETE USER ----------------
        [HttpDelete("{email}")]
        public async Task<IActionResult> DeleteUser(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return NotFound(new
                {
                    message = "User not found"
                });
            }

            var result = await _userManager.DeleteAsync(user);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            return Ok(new
            {
                message = "User deleted successfully"
            });
        }

        // ---------------- JWT GENERATOR ----------------
        private string GenerateJwtToken(ApplicationUser user)
        {
            Console.WriteLine("JWT KEY = " + _configuration["Jwt:Key"]);
            Console.WriteLine("JWT ISSUER = " + _configuration["Jwt:Issuer"]);
            Console.WriteLine("JWT AUDIENCE = " + _configuration["Jwt:Audience"]);

            var key = _configuration["Jwt:Key"];

            if (string.IsNullOrEmpty(key))
                throw new Exception("JWT Key is missing");

            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];

            var securityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(key));

            var credentials = new SigningCredentials(
                securityKey,
                SecurityAlgorithms.HmacSha256);

            // GET USER ROLES
            var roles = _userManager.GetRolesAsync(user).Result;

            // ADD CLAIMS
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email ?? ""),
                new Claim(ClaimTypes.Name, user.UserName ?? "")
            };

            // ADD ROLE CLAIMS
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            // CREATE TOKEN
            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}