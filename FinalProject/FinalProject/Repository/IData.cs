using FinalProject.Models;
using System.Security.Claims;

namespace FinalProject.Repository
{
    //Declares what kind of operations can be performed on data related to users 
    //Declares 1 method that takes info about a user .
    public interface IData
    {
        Task<ApplicationUser> GetUser(ClaimsPrincipal claims);

    }
}
