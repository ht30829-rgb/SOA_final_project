using AnnaSweetBakery.API.ContextDBConfig;
using AnnaSweetBakery.API.DTOs;
using AnnaSweetBakery.API.Models;
using Microsoft.EntityFrameworkCore;

namespace AnnaSweetBakery.API.Services
{
    public class OrderService : IOrderService
    {
        private readonly FinalDBContext _context;

        public OrderService(FinalDBContext context)
        {
            _context = context;
        }

        public async Task<Order> CreateOrderAsync(string userId, CreateOrderDto dto)
        {
            var order = new Order
            {
                UserId = userId,
                Address = dto.Address,
                OrderDate = DateTime.Now,
                OrderItems = new List<OrderItem>()
            };

            foreach (var item in dto.Items)
            {
                order.OrderItems.Add(new OrderItem
                {
                    RecipeId = item.RecipeId,
                    RecipeName = item.RecipeName,
                    Quantity = item.Quantity,
                    Price = item.Price
                });
            }

            order.TotalAmount = order.OrderItems.Sum(x => x.Price * x.Quantity);

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return order;
        }
    }
}
