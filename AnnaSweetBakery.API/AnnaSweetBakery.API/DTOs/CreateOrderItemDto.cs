namespace AnnaSweetBakery.API.DTOs
{
    public class CreateOrderItemDto
    {
        public string RecipeId { get; set; }
        public string RecipeName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
