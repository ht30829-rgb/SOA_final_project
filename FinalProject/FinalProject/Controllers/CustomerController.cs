using FinalProject.ContextDBConfig;
using FinalProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

[Authorize(Roles = "Customer")]
public class CustomerController : Controller
{
    private readonly FinalProjectDBContext _context;
    private readonly ILogger<CustomerController> _logger;

    public CustomerController(FinalProjectDBContext context, ILogger<CustomerController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> PlaceOrder(string recipeId, string recipeName, string address, int price, int quantity)
    {
        // Log the start of the order process
        _logger.LogInformation("Starting PlaceOrder for user {UserId}.", User.FindFirstValue(ClaimTypes.NameIdentifier));

        // Log the received order details
        _logger.LogInformation("Order details received:");
        _logger.LogInformation("RecipeId: {RecipeId}", recipeId);
        _logger.LogInformation("RecipeName: {RecipeName}", recipeName);
        _logger.LogInformation("Address: {Address}", address);
        _logger.LogInformation("Price: {Price}", price);
        _logger.LogInformation("Quantity: {Quantity}", quantity);

        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get the current user's ID
        _logger.LogInformation("User ID: {UserId}", userId);

        // Calculate the total amount
        int totalAmount = price * quantity;
        _logger.LogInformation("Total amount calculated: {TotalAmount}", totalAmount);

        // Create a new order
        var order = new Order
        {
            RecipeId = recipeId,
            RecipeName = recipeName,
            UserId = userId,
            Address = address,
            Price = price,
            Quantity = quantity,
            TotalAmount = totalAmount,
            OrderDate = DateTime.Now // Set the order date to the current date and time
        };

        // Add the order to the database
        _context.Orders.Add(order);
        await _context.SaveChangesAsync(); // Save the order to the database
        _logger.LogInformation("Order {OrderId} saved to the database.", order.Id);

        // Redirect to MyOrders after placing the order
        return RedirectToAction("MyOrders");
    }

    public async Task<IActionResult> MyOrders(int? pageIndex)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get the current user's ID
        _logger.LogInformation("Fetching orders for user {UserId}...", userId);

        // Set the default page index to 1 if not provided
        int pageSize = 5; // Number of orders per page
        int currentPage = pageIndex ?? 1;

        // Fetch orders for the current user
        var ordersQuery = _context.Orders
            .Where(o => o.UserId == userId)
            .OrderByDescending(o => o.OrderDate); // Order by most recent

        // Apply pagination
        var paginatedOrders = await PaginatedList<Order>.CreateAsync(ordersQuery, currentPage, pageSize);

        return View(paginatedOrders);
    }
}