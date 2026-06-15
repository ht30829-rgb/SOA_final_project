using AnnaSweetBakery.API.Controllers;
using AnnaSweetBakery.API.ContextDBConfig;
using AnnaSweetBakery.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace AnnaSweetBakery.Test.Controllers
{
    public class RecipeControllerTests
    {
        [Fact]
        public async Task GetRecipes_ReturnsOkResult()
        {
            var options = new DbContextOptionsBuilder<FinalDBContext>()
                .UseInMemoryDatabase("RecipeDb")
                .Options;

            var context = new FinalDBContext(options);

            var userStore =
                new Mock<IUserStore<ApplicationUser>>();

            var userManager =
                new Mock<UserManager<ApplicationUser>>(
                    userStore.Object,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null,
                    null);

            var controller =
                new RecipeController(
                    userManager.Object,
                    context);

            var result = await controller.GetRecipes();

            Assert.IsType<OkObjectResult>(result);
        }
    }
}