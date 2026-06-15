using Microsoft.AspNetCore.Mvc;
using AnnaSweetBakery.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using AnnaSweetBakery.API.ContextDBConfig;
using Microsoft.EntityFrameworkCore;
using AnnaSweetBakery.API.DTOs;

namespace AnnaSweetBakery.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecipeController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly FinalDBContext _context;

        public RecipeController(
            UserManager<ApplicationUser> userManager,
            FinalDBContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        // GET: api/recipe
        [HttpGet]
        public async Task<IActionResult> GetRecipes()
        {
            var recipes = await _context.Recipes.ToListAsync();

            return Ok(recipes);
        }

        // GET: api/recipe/search?recipe=cake
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string recipe)
        {
            if (string.IsNullOrEmpty(recipe))
            {
                return BadRequest("Recipe search term is required.");
            }

            var recipes = await _context.Recipes
                .Where(r =>
                    r.Title != null &&
                    r.Title.Contains(recipe))
                .ToListAsync();

            return Ok(recipes);
        }

        // GET: api/recipe/order/1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRecipeById(string id)
        {
            var recipe = await _context.Recipes
                .FirstOrDefaultAsync(r => r.Id == id);

            if (recipe == null)
            {
                return NotFound("Recipe not found.");
            }

            return Ok(recipe);
        }

        // POST: api/recipe/show-order
        [HttpPost("preview-order")]
        public async Task<IActionResult> ShowOrder(
            [FromBody] OrderRecipeDetails orderRecipeDetails)
        {
            Random random = new Random();

            decimal price = random.Next(3, 15);

            var user = await _userManager.GetUserAsync(HttpContext.User);

            return Ok(new
            {
                Recipe = orderRecipeDetails,
                Price = price,
                UserId = user?.Id,
                Address = user?.Address,
                Message = "Thank you for your purchase!"
            });
        }

        
    }
}