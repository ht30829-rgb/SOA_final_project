using AnnaSweetBakery.API.Controllers;
using AnnaSweetBakery.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace AnnaSweetBakery.Test.Controllers
{
    public class CustomerControllerTests
    {
        [Fact]
        public async Task Profile_ReturnsUnauthorized_WhenUserNotLoggedIn()
        {
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
                new CustomerController(userManager.Object);

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            var result = await controller.Profile();

            Assert.IsType<UnauthorizedResult>(result);
        }
    }
}