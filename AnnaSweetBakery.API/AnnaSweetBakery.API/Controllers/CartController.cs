using AnnaSweetBakery.API.ContextDBConfig;
using AnnaSweetBakery.API.DTOs;
using AnnaSweetBakery.API.Models;
using AnnaSweetBakery.API.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AnnaSweetBakery.API.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly IData _data;
        private readonly FinalDBContext _context;

        public CartController(IData data, FinalDBContext context)
        {
            _data = data;
            _context = context;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            var user = await _data.GetUser(HttpContext.User);

            if (user == null)
            {
                return Unauthorized(new
                {
                    message = "User is not authenticated"
                });
            }

            var carts = await _context.Carts
                .Where(c => c.UserId == user.Id)
                .ToListAsync();

            return Ok(carts);
        }

        [HttpGet("added")]
        public async Task<IActionResult> GetAddedCarts()
        {
            var user = await _data.GetUser(HttpContext.User);

            if (user == null)
            {
                return Unauthorized(new
                {
                    message = "User is not authenticated"
                });
            }

            var carts = await _context.Carts
                .Where(c => c.UserId == user.Id)
                .Select(c => c.RecipeId)
                .ToListAsync();

            return Ok(carts);
        }
        [HttpPost]
        public async Task<IActionResult> AddToCart([FromBody] CreateCartDto dto)
        {
            var user = await _data.GetUser(HttpContext.User);

            if (user == null)
            {
                return Unauthorized(new
                {
                    message = "User is not authenticated"
                });
            }

            var cart = new Cart
            {
                Image_url = dto.Image_url,
                Publisher = dto.Publisher,
                Title = dto.Title,
                RecipeId = dto.RecipeId,
                UserId = user.Id
            };

            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();

            return Ok(cart);
        }
    }
}