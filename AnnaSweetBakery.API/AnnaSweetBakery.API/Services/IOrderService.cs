using AnnaSweetBakery.API.DTOs;
using AnnaSweetBakery.API.Models;

namespace AnnaSweetBakery.API.Services
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(string userId, CreateOrderDto dto);
    }
}
