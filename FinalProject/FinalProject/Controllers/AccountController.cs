using Microsoft.AspNetCore.Mvc;
using FinalProject.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace FinalProject.Controllers
{
    // Handles user authentication actions
    public class AccountController : Controller
    {
        // Using the services provided for managing users and sign-ins
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager; // Add RoleManager for role management

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager) // Inject RoleManager
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        // Login action responsible for rendering the login view and handling the login submission
        public IActionResult Login()
        {
            return View();
        }

        // Using SignInManager to perform sign-in operation
        // If login successful, redirects to Home; if not, displays an error
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel login, string? returnUrl)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(login.Email, login.Password, false, false);
                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(returnUrl))
                        return LocalRedirect(returnUrl);
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "Invalid LogIn Attempt");
            }
            return View(login);
        }

        // Logout action using SignInManager to sign out the user and redirect to Home
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        // Register action for rendering the registration view and handling registration submission
        public IActionResult Register()
        {
            return View();
        }

        // Using UserManager to create a new user using the model ApplicationUser and sign in successfully
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel register)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser()
                {
                    Name = register.Name,
                    Address = register.Address,
                    Email = register.Email,
                    UserName = register.Email
                };

                var result = await _userManager.CreateAsync(user, register.Password);

                if (result.Succeeded)
                {
                    // Assign role based on email
                    if (register.Email.ToLower() == "admin@example.com")
                    {
                        await EnsureRoleExists("Admin"); // Ensure the Admin role exists
                        await _userManager.AddToRoleAsync(user, "Admin"); // Assign Admin role
                    }
                    else
                    {
                        await EnsureRoleExists("Customer"); // Ensure the Customer role exists
                        await _userManager.AddToRoleAsync(user, "Customer"); // Assign Customer role
                    }

                    // Sign in the user
                    await _signInManager.PasswordSignInAsync(user, register.Password, false, false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var err in result.Errors)
                    {
                        ModelState.AddModelError("", err.Description);
                    }
                }
            }
            return View(register);
        }

        // Helper method to ensure a role exists
        private async Task EnsureRoleExists(string roleName)
        {
            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                await _roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }
    }
}