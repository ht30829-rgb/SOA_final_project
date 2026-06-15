using AnnaSweetBakery.API.Models;
using AnnaSweetBakery.API.Repository;
using System.Security.Claims;

namespace AnnaSweetBakery.API.Services
{
    public class UserService : IUserService
    {
        private readonly IData _data;

        public UserService(IData data)
        {
            _data = data;
        }

        public async Task<ApplicationUser> GetCurrentUser(ClaimsPrincipal claims)
        {
            return await _data.GetUser(claims);
        }
    }
}
