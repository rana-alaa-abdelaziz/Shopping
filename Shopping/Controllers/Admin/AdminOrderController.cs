using ITIEntities.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Shopping.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    public class AdminOrderController : Controller
    {
        private readonly ITIContext _context;
        public AdminOrderController(ITIContext context) => _context = context;
        public IActionResult Index()
        {
            var allOrders = _context.Orders
                .Include(o => o.App_User) 
                .Where(o => o.Status != "Cart") 
                .OrderByDescending(o => o.OrderDate)
                .ToList();

            return View(allOrders);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int orderId, string newStatus)
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order != null)
            {
                order.Status = newStatus;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
