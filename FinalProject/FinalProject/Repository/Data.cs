using FinalProject.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;


//Seperates the logic of retriving the data from DB ,from the app
namespace FinalProject.Repository
{
    //Implements interface methods using the User manager 
    public class Data : IData
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public Data(UserManager<ApplicationUser>manager )
        {
            _userManager = manager;
        }

        //The method retrives the ApplicatioUser based on provided info about the user using ClaimsPrincipal
        //ClaimsPrincipal contains info about current user ,identity and claims
        public async Task<ApplicationUser> GetUser(ClaimsPrincipal claims)
        {
            return await _userManager.GetUserAsync(claims);
        }
    }
}
