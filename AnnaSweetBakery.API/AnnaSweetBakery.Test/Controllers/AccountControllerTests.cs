using AnnaSweetBakery.API.Controllers;
using AnnaSweetBakery.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace AnnaSweetBakery.Test.Controllers
{
    public class AccountControllerTests
    {
        [Fact]
        public async Task Login_ReturnsBadRequest_WhenModelIsNull()
        {
            var userStore =
                new Mock<IUserStore<ApplicationUser>>();

            var userManager =
                new Mock<UserManager<ApplicationUser>>(
                    userStore.Object,
                    null, null, null, null,
                    null, null, null, null);

            var signInManager =
                new Mock<SignInManager<ApplicationUser>>(
                    userManager.Object,
                    Mock.Of<Microsoft.AspNetCore.Http.IHttpContextAccessor>(),
                    Mock.Of<IUserClaimsPrincipalFactory<ApplicationUser>>(),
                    null, null, null, null);

            var config =
                new ConfigurationBuilder().Build();

            var controller =
                new AccountController(
                    userManager.Object,
                    signInManager.Object,
                    config);

            var result = await controller.Login(null);

            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}