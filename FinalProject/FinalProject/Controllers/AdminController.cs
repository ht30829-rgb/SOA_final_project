using FinalProject.ContextDBConfig;
using FinalProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly FinalProjectDBContext _context;

    public AdminController(FinalProjectDBContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> ViewOrders(int? pageIndex)
    {
        // Set the default page index to 1 if not provided
        int pageSize = 10; // Number of orders per page
        int currentPage = pageIndex ?? 1;

        // Fetch orders from the database
        var ordersQuery = _context.Orders
            .Include(o => o.User) // Include user details
            .OrderByDescending(o => o.OrderDate); // Order by most recent

        // Apply pagination
        var paginatedOrders = await PaginatedList<Order>.CreateAsync(ordersQuery, currentPage, pageSize);

        return View(paginatedOrders);
    }
}