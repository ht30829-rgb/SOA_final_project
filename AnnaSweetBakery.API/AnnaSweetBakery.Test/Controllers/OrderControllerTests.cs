using AnnaSweetBakery.API.Controllers;
using AnnaSweetBakery.API.ContextDBConfig;
using AnnaSweetBakery.API.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace AnnaSweetBakery.Test.Controllers
{
    public class OrderControllerTests
    {
        [Fact]
        public async Task MyOrders_ReturnsUnauthorized_WhenUserNotLoggedIn()
        {
            var options = new DbContextOptionsBuilder<FinalDBContext>()
                .UseInMemoryDatabase("OrderDb1")
                .Options;

            var context = new FinalDBContext(options);

            var dataMock = new Mock<IData>();

            var controller =
                new OrderController(context, dataMock.Object);

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            var result = await controller.MyOrders();

            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task Checkout_ReturnsUnauthorized_WhenUserNotLoggedIn()
        {
            var options = new DbContextOptionsBuilder<FinalDBContext>()
                .UseInMemoryDatabase("OrderDb2")
                .Options;

            var context = new FinalDBContext(options);

            var dataMock = new Mock<IData>();

            var controller =
                new OrderController(context, dataMock.Object);

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            var result = await controller.Checkout();

            Assert.IsType<UnauthorizedResult>(result);
        }
    }
}