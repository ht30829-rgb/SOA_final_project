using System.ComponentModel.DataAnnotations;
namespace FinalProject.Models
{
    //Login Data
    public class LoginViewModel
    {
        [Required,EmailAddress]
        public string? Email { get; set; }
        [Required]
        public string? Password { get; set; }

    }
}
