using ITIEntities;
using ITIEntities.Data;
using ITIEntities.Repo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shopping.ViewModels;
using System.Security.Claims;

namespace Shopping.Controllers.User
{
    [Authorize]

    public class CartController : Controller
    {
        private readonly IRepo<Order_Item> _CartRepo;
        private readonly ITIContext _context;
        public CartController(IRepo<Order_Item> cartRepo, ITIContext context)
        {
            _CartRepo = cartRepo;
            _context = context;
        }

        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var cart = _context.Orders
                .FirstOrDefault(o => o.UserId == userId && o.Status == "Cart");

            if (cart == null)
                return View(new List<Order_ItemsVM>());

            var model = _CartRepo.GetAll()
                .Include(p => p.Product)
                .Where(i => i.OrderId == cart.OrderId)
                .Select(i => new Order_ItemsVM
                {
                    OrderItemID = i.OrderItemID,
                    OrderId = i.OrderId,
                    ProductId = i.ProductId,
                    Product = i.Product,
                    UnitPrice = i.UnitPrice,
                    Quantity = i.Quantity,
                    LineTotal = i.UnitPrice * i.Quantity
                })
                .ToList();

            return View(model);
        }

   
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(Order_ItemsVM orderVM)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Index", "Catalog");

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var cartId = GetOrCreateCart(userId);

            var existingItem = _CartRepo.GetAll()
                .FirstOrDefault(c => c.ProductId == orderVM.ProductId && c.OrderId == cartId);

            if (existingItem != null)
            {
                existingItem.Quantity += orderVM.Quantity;
                existingItem.LineTotal = existingItem.UnitPrice * existingItem.Quantity;

                _CartRepo.Update(existingItem);
            }
            else
            {
                var newItem = new Order_Item
                {
                    ProductId = orderVM.ProductId,
                    Quantity = orderVM.Quantity,
                    UnitPrice = orderVM.UnitPrice,
                    LineTotal = orderVM.UnitPrice * orderVM.Quantity,
                    OrderId = cartId
                };

                _CartRepo.Add(newItem);
            }

            return RedirectToAction("Index","Cart");
        }
        private int GetOrCreateCart(string userId)
        {
            
            var cart = _context.Orders
                .FirstOrDefault(o => o.UserId == userId && o.Status == "Cart");

            if (cart != null)
                return cart.OrderId;

            var newCart = new Order
            {
                UserId = userId,
                Status = "Cart",
                OrderDate = DateTime.Now,
                OrderTime = DateTime.Now,
                TotalAmount = 0,
                OrderItems = new List<Order_Item>(),
                OrderNumber = "CART-" + Guid.NewGuid().ToString().Substring(0, 8)
            };

            _context.Orders.Add(newCart);
            _context.SaveChanges();

            return newCart.OrderId;
        }
        [HttpPost]
        public IActionResult Update(int itemId, int qty)
        {
            var item = _CartRepo.GetById(itemId);

            if (item == null)
                return RedirectToAction("Index");

            item.Quantity = qty;
            item.LineTotal = item.UnitPrice * qty;

            _CartRepo.Update(item);

            return RedirectToAction("Index");
        }

        public IActionResult Remove(int itemId)
        {
            var item = _CartRepo.GetById(itemId);

            if (item != null)
            {
                _CartRepo.Delete(itemId);
            }

            return RedirectToAction("Index");
        }




    }
    }