using Microsoft.AspNetCore.Mvc;
using FinalProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using FinalProject.ContextDBConfig;

namespace FinalProject.Controllers
{
    public class RecipeController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly FinalProjectDBContext context;

        public RecipeController(
            UserManager<ApplicationUser> userManager,
            FinalProjectDBContext dBContext)
        {
            _userManager = userManager;
            context = dBContext;
        }

        public IActionResult Index()
        {
            ViewBag.OrderConfirmationMessage =
                TempData["OrderConfirmationMessage"] as string;

            return View();
        }

        [HttpPost]
        public IActionResult GetRecipeCard([FromBody] List<Recipe> recipes)
        {
            return PartialView("_RecipeCard", recipes);
        }

        public IActionResult Search([FromQuery] string recipe)
        {
            ViewBag.Recipe = recipe;
            return View();
        }

        public IActionResult Order([FromQuery] string id)
        {
            ViewBag.Id = id;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ShowOrder(OrderRecipeDetails orderRecipeDetails)
        {
            Random random = new Random();

            ViewBag.Price = random.Next(3, 15);

            var user = await _userManager.GetUserAsync(HttpContext.User);

            ViewBag.UserId = user?.Id;
            ViewBag.Address = user?.Address;

            TempData["OrderConfirmationMessage"] =
                "Thank you for your purchase! Order is being processed...";

            return PartialView("_ShowOrder", orderRecipeDetails);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Order([FromForm] Order order)
        {
            order.OrderDate = DateTime.Now;

            if (ModelState.IsValid)
            {
                context.Orders.Add(order);
                context.SaveChanges();

                return RedirectToAction("Index", "Recipe");
            }

            return RedirectToAction("Order", "Recipe", new { id = order.Id });
        }
    }
}