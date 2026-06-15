using Microsoft.AspNetCore.Mvc;

namespace AnnaSweetBakery.API.DTOs
{
    public class CreateOrderDto
    {
        public string Address { get; set; }
        public List<CreateOrderItemDto> Items { get; set; } = new();

    } 

}
