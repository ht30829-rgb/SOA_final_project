using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using FinalProject.Models;
using Microsoft.VisualStudio.Web.CodeGeneration.Design;
using Microsoft.Win32;
using System;
using AnnaSweetBakery.API.Models;

namespace FinalProject.ContextDBConfig
{
    //The file which will inherit from IndentityDbContext to establish conection between the app data(Order/Cart) and identity data(users), (like orders to users)
    public class FinalProjectDBContext : IdentityDbContext<ApplicationUser>
    {
        public FinalProjectDBContext(
            DbContextOptions<FinalProjectDBContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<Cart> Carts { get; set; }

        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
    }
}
