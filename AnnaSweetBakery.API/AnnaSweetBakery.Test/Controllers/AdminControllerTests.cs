using AnnaSweetBakery.API.Controllers;
using AnnaSweetBakery.API.ContextDBConfig;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace AnnaSweetBakery.Tests.Controllers
{
    public class AdminControllerTests
    {
        [Fact]
        public void GetOrders_ReturnsOkObjectResult()
        {
            var options = new DbContextOptionsBuilder<FinalDBContext>()
                .UseInMemoryDatabase(databaseName: "AdminTestDb")
                .Options;

            var context = new FinalDBContext(options);

            var controller = new AdminController(context);

            var result = controller.GetOrders();

            Assert.IsType<OkObjectResult>(result);
        }
    }
}