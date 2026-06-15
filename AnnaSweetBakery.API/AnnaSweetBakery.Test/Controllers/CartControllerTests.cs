using AnnaSweetBakery.API.Controllers;
using AnnaSweetBakery.API.ContextDBConfig;
using AnnaSweetBakery.API.Models;
using AnnaSweetBakery.API.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;
using Microsoft.AspNetCore.Mvc;

namespace AnnaSweetBakery.Test.Controllers
{
    public class CartControllerTests
    {
        [Fact]
        public async Task GetCart_ReturnsUnauthorized_WhenUserNotLoggedIn()
        {
            var options = new DbContextOptionsBuilder<FinalDBContext>()
                .UseInMemoryDatabase("CartDb1")
                .Options;

            var context = new FinalDBContext(options);

            var dataMock = new Mock<IData>();

            dataMock.Setup(x =>
                x.GetUser(It.IsAny<System.Security.Claims.ClaimsPrincipal>()))
                .ReturnsAsync((ApplicationUser?)null);

            var controller =
                new CartController(dataMock.Object, context);

            controller.ControllerContext = new()
            {
                HttpContext = new DefaultHttpContext()
            };

            var result = await controller.GetCart();

            Assert.IsType<UnauthorizedObjectResult>(result);
        }

        [Fact]
        public async Task GetAddedCartsReturnsUnauthorizedWhenUserNotLoggedIn()
        {
            var options = new DbContextOptionsBuilder<FinalDBContext>()
                .UseInMemoryDatabase("CartDb2")
                .Options;

            var context = new FinalDBContext(options);

            var dataMock = new Mock<IData>();

            dataMock.Setup(x =>
                x.GetUser(It.IsAny<System.Security.Claims.ClaimsPrincipal>()))
                .ReturnsAsync((ApplicationUser?)null);

            var controller =
                new CartController(dataMock.Object, context);

            controller.ControllerContext = new()
            {
                HttpContext = new DefaultHttpContext()
            };

            var result = await controller.GetAddedCarts();

            Assert.IsType<UnauthorizedObjectResult>(result);
        }
    }
}