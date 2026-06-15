using System.ComponentModel.DataAnnotations;

namespace AnnaSweetBakery.API.Models
{
    public class OrderItem
    {
        [Key]
        public int Id { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; }

        public string RecipeId { get; set; } = string.Empty;

        public string RecipeName { get; set; } = string.Empty;

        public int Quantity { get; set; }

        public decimal Price { get; set; }
    }
}
