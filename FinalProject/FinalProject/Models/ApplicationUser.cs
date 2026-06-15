using Microsoft.AspNetCore.Identity;
namespace FinalProject.Models
{
    //The actual User Data model!
    public class ApplicationUser : IdentityUser
    {
        public string? Name { get; set; }
        public string? Address { get; set; }
    }
}
