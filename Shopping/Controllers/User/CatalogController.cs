using ITIEntities;
using ITIEntities.Repo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shopping.ViewModels;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Shopping.Controllers.User
{

    public class CatalogController : Controller
    {
        private readonly IRepo<Product> _productRepo;
        
        public CatalogController(IRepo<Product> ProductRepo)
        {
            _productRepo = ProductRepo;
        }
        public IActionResult Index(int? categoryId,string SearchQuery ,string  sort ,int page= 1)
        {
            ViewBag.Categories = _productRepo.GetQueryable()
                        .Select(p => p.Category)
                        .Distinct()
                        .ToList();
            var products = _productRepo.GetQueryable().Where(p => p.IsActive);
            if (categoryId.HasValue)
            {
                products = products.Where(p => p.CategoryId == categoryId.Value);
            }
            if (!string.IsNullOrEmpty(SearchQuery))
            {
                products = products.Where(p => p.IsActive && p.Name.Contains(SearchQuery));
            }

            products = sort switch
            {
                "price_asc" => products.OrderBy(p => p.Price),
                "price_desc" => products.OrderByDescending(p => p.Price),
                _ => products.OrderByDescending(p => p.CreatedAt) 
            };

            int pageSize = 12;
            int totalItems = products.Count();
            int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = totalPages;
            ViewBag.CurrentSearch = SearchQuery;
            ViewBag.CurrentSort = sort;
            ViewBag.CurrentCategory = categoryId;

            var finalResult = products
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new ProductVM 
                {
                    ProductId = p.ProductId,
                    Name = p.Name,
                    Price = p.Price,
                    StockQuantity = p.StockQuantity,
                    ImageUrl =p.ImageUrl,
                    SKU = p.SKU
                }).ToList();
            return View(finalResult);
        }

        public IActionResult Details(int? id)
        {
            if (id == null) return BadRequest();

            var product = _productRepo.GetAll(p => p.Category)
                              .FirstOrDefault(p => p.ProductId == id.Value);

            if (product == null) return NotFound();
            var model = new ProductVM
            {
                ProductId = product.ProductId,
                Name = product.Name,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                SKU = product.SKU,
                ImageUrl = product.ImageUrl,
                CategoryId = product.CategoryId,
                Category = product.Category
            };
            return View(model);
        }
    }
}
