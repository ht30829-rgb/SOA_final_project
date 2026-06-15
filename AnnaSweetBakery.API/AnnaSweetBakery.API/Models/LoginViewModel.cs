using System.ComponentModel.DataAnnotations;
namespace AnnaSweetBakery.API.Models
{
    //Login Data
    public class LoginViewModel
    {
        [Required,EmailAddress]
        public string? Email { get; set; } = string.Empty;
        [Required]
        public string? Password { get; set; } = string.Empty;

    }
}
