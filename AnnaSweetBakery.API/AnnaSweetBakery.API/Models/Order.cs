using System.ComponentModel.DataAnnotations;

namespace AnnaSweetBakery.API.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        public string UserId { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;
        public ApplicationUser User { get; set; }

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        public decimal TotalAmount { get; set; }

        public List<OrderItem> OrderItems { get; set; } = new();
    }
}