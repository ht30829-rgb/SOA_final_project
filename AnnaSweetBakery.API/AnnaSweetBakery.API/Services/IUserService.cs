using AnnaSweetBakery.API.Models;
using System.Security.Claims;

namespace AnnaSweetBakery.API.Services
{
    public interface IUserService
    {
        Task<ApplicationUser> GetCurrentUser(ClaimsPrincipal claims);
    }
}
