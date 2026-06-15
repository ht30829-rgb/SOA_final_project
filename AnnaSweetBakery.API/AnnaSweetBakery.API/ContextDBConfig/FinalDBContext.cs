using AnnaSweetBakery.API.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AnnaSweetBakery.API.ContextDBConfig
{
    public class FinalDBContext : IdentityDbContext<ApplicationUser>
    {
        public FinalDBContext(
            DbContextOptions<FinalDBContext> options)
            : base(options)
        {
        }

        public DbSet<Cart> Carts { get; set; }

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }

        public DbSet<Recipe> Recipes { get; set; }
    }
}