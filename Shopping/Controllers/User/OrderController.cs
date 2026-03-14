using ITIEntities;
using ITIEntities.Data;
using ITIEntities.Repo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Shopping.ViewModels;
using System.Security.Claims;

namespace Shopping.Controllers.User
{
    public class OrderController : Controller
    {
        private readonly IRepo<Order_Item> _CartRepo;
        private readonly ITIContext _context;
        public OrderController(IRepo<Order_Item> cartRepo, ITIContext context)
        {
            _CartRepo = cartRepo;
            _context = context;
        }

        public IActionResult Checkout()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var cart = _context.Orders.Include(o => o.OrderItems).ThenInclude(i => i.Product).FirstOrDefault(o => o.UserId == userId && o.Status == "Cart");

            if (cart == null)
            {
                return RedirectToAction("Index", "Cart");
            }
            ViewBag.CartItems = cart.OrderItems.Select(i => new Order_ItemsVM
            {
                Product = i.Product,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice,
                LineTotal = i.UnitPrice * i.Quantity
            }).ToList();

            ViewBag.UserAddresses = _context.Addresses
         .Where(a => a.UserId == userId)
         .ToList();

            var model = new CheckoutVM
            {
                TotalAmount = cart.OrderItems.Sum(i => i.UnitPrice * i.Quantity),
                OrderDate = DateTime.Now,
                 Address = _context.Addresses.FirstOrDefault(a => a.UserId == userId && a.isDefault) ?? new Address()
            };
            return View(model);

        }
        [HttpPost]
        public async Task <IActionResult> Checkout(CheckoutVM model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var CartOrder = _context.Orders.Include(o => o.OrderItems).ThenInclude(i => i.Product).FirstOrDefault(o => o.UserId == userId && o.Status == "Cart");
            if(CartOrder==null || !CartOrder.OrderItems.Any())
            {
                return RedirectToAction("Index", "Catalog");
            }

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                if (model.Address != null && model.ShippingAddressId == null)
                {
                    model.Address.UserId = userId;
                    _context.Addresses.Add(model.Address);
                    await _context.SaveChangesAsync();
                    CartOrder.ShippingAddressId = model.Address.AddressId;
                }
                else
                {
                    CartOrder.ShippingAddressId = model.ShippingAddressId;
                }
                CartOrder.OrderDate = DateTime.Now.Date;
                CartOrder.OrderTime = DateTime.Now;
                CartOrder.OrderNumber = "ORD-" + Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
                CartOrder.Status = "Pending";
                decimal finalTotal = 0;
                foreach (var item in CartOrder.OrderItems)
                {

                    item.UnitPrice = item.Product.Price;
                    finalTotal += (item.UnitPrice * item.Quantity);
                }
                CartOrder.TotalAmount = finalTotal;

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return RedirectToAction("Index", "Cart");

            }
            catch (Exception)
            { 
                await transaction.RollbackAsync();
                return View("Error");
            }
        }
        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var model = _context.Orders.Where(o=>o.UserId== userId && o.Status != "Cart").OrderByDescending(o=>o.OrderDate).ToList();
            
            return View(model);
        }
          
        public IActionResult Details(int id)
        {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var order = _context.Orders
                    .Include(o => o.Address)              
                    .Include(o => o.OrderItems)          
                    .ThenInclude(oi => oi.Product)       
                    .FirstOrDefault(o => o.OrderId == id && o.UserId == userId);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }
    }
}
