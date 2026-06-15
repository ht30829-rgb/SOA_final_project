using FinalProject.Models;
using FinalProject.Repository;
using Microsoft.AspNetCore.Mvc;
using FinalProject.Repository;
using Microsoft.AspNetCore.Authorization;
using FinalProject.ContextDBConfig;

namespace FinalProject.Controllers
{
    //Controller handling actions related to Favorites ,only accesible by authenticated users
    [Authorize]
    public class CartController : Controller
    {
        //Using the repository interface and the DB connection
        private readonly IData data;
        private readonly FinalProjectDBContext context;
        public CartController(IData data, FinalProjectDBContext context)
        {
            this.data = data;
            this.context = context;
        }
        //Action which retrives the current user, takes the fav items from db associated to that user and passes to Index view
        public async  Task<IActionResult> Index()
        {
            var user = await data.GetUser(HttpContext.User);
            var cartsList = context.Carts.Where(c=>c.UserId==user.Id).ToList();
            return View(cartsList);
        }
        //Handles the saving of favorited items by setting the current users ID and adding the fav item to DB
        [HttpPost]
        public async Task<IActionResult> SaveCart(Cart cart)
        {
            var user = await data.GetUser(HttpContext.User);
            cart.UserId= user?.Id;
            if (ModelState.IsValid)
            {
                context.Carts.Add(cart);
                context.SaveChanges();
                return Ok();
            }
            return BadRequest();
        }
        //Retrives the list of items by its id's that have been added to fav list of current user
        [HttpGet]
        public async Task<IActionResult> GetAddedCarts()
        {
            var user= await data.GetUser(HttpContext.User);
            var carts = context.Carts.Where(c => c.UserId == user.Id).Select(c=>c.RecipeId).ToList();
            return Ok(carts);
        }
        //Removes fav item from the list based on given ID
        [HttpPost]
        public IActionResult RemoveCartFromList(string Id)
        {
            if (!string.IsNullOrEmpty(Id))
            {
                var cart=context.Carts.Where(c=>c.RecipeId == Id).FirstOrDefault(); 
                if(cart != null)
                {
                    context.Carts.Remove(cart);
                    context.SaveChanges();
                    return Ok();
                }
            }
            return BadRequest();
        }
        //Retrives the partial view of user's favorite items ,limited to 3 items shown 
        [HttpGet]
        public async Task<IActionResult> GetCartList()
        {
            var user = await data.GetUser(HttpContext.User);
            var cartList = context.Carts.Where(c => c.UserId == user.Id).Take(3).ToList();
            return PartialView("_CartList",cartList);
        }
    }
}
