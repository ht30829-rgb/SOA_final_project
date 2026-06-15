using AnnaSweetBakery.API.ContextDBConfig;
using AnnaSweetBakery.API.Models;
using AnnaSweetBakery.API.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AnnaSweetBakery.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(
        AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,
        Roles = "Customer"
    )]
    public class OrderController : ControllerBase
    {
        private readonly FinalDBContext _context;
        private readonly IData _data;

        public OrderController(
            FinalDBContext context,
            IData data)
        {
            _context = context;
            _data = data;
        }

        // POST: api/order/checkout
        [HttpPost("checkout")]
        public async Task<IActionResult> Checkout()
        {
            var userId =
                User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var cartItems = await _context.Carts
                .Where(c => c.UserId == userId)
                .ToListAsync();

            if (!cartItems.Any())
            {
                return BadRequest(new
                {
                    message = "Cart is empty"
                });
            }

            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.UtcNow,
                TotalAmount = 0,
                OrderItems = new List<OrderItem>()
            };

            decimal total = 0;

            foreach (var item in cartItems)
            {
                var orderItem = new OrderItem
                {
                    RecipeId = item.RecipeId,
                    RecipeName = item.Title,
                    Quantity = 1,
                    Price = 10
                };

                total += orderItem.Price;

                order.OrderItems.Add(orderItem);
            }

            order.TotalAmount = total;

            _context.Orders.Add(order);

            _context.Carts.RemoveRange(cartItems);

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Order created successfully",
                orderId = order.Id,
                total = order.TotalAmount
            });
        }

        // GET: api/order/my
        [HttpGet("my")]
        public async Task<IActionResult> MyOrders()
        {
            var userId =
                User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var orders = await _context.Orders
                .Include(o => o.OrderItems)
                .Where(o => o.UserId == userId)
                .ToListAsync();

            return Ok(orders);
        }
    }
}