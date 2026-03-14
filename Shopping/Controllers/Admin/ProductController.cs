using ITIEntities;
using ITIEntities.Data;
using ITIEntities.Repo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shopping.ViewModels;

namespace Shopping.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        private readonly IRepo<Product> _productRepo;
        private readonly ITIContext _context;
        public ProductController(IRepo<Product> ProductRepo, ITIContext context)
        {
            _productRepo = ProductRepo;
            _context = context;
        }
        public IActionResult Create(int categoryId)
        {
            var category = _context.Categories
                         .FirstOrDefault(c => c.CategoryId == categoryId);

            var model = new ProductVM
            {
                CategoryId = categoryId,
                Category = category
            };


            return View(model);


        }
        [HttpPost]
        public IActionResult Create(ProductVM productVM)
        {
            ModelState.Remove("Category");
          
            string fileName = "default.png";
            if (ModelState.IsValid)
            {

                if (productVM.ImageFile != null && productVM.ImageFile.Length > 0)
                {
                    string uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");

                    fileName = Guid.NewGuid().ToString() + Path.GetExtension(productVM.ImageFile.FileName);

                    string filePath = Path.Combine(uploads, fileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        productVM.ImageFile.CopyTo(fileStream);
                    }
                }
                var product = new Product
                {
                    Name = productVM.Name,
                    SKU = productVM.SKU,
                    Price = productVM.Price,
                    StockQuantity = productVM.StockQuantity,
                    IsActive = productVM.IsActive,
                    CreatedAt = DateTime.Now,
                    CategoryId = productVM.CategoryId,
                    ImageUrl = fileName
                };
                _productRepo.Add(product);
                return RedirectToAction("Details", "Categories", new { id = productVM.CategoryId });
            }
            return View(productVM);

        }
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var model = _productRepo.GetAll(p => p.Category).FirstOrDefault(p => p.ProductId == id.Value); if (model == null)
            {
                return NotFound();
            }
            return View(model);

        }

        [HttpPost]
        [ActionName("Delete")]
        public IActionResult DeleteConfirm(int id)
        {
            var product = _productRepo.GetById(id);
            if (product == null)
            {
                return NotFound();
            }
            int categoryId = product.CategoryId;

            _productRepo.Delete(id);
            return RedirectToAction("Details", "Categories", new { id = categoryId });

        }


        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null) return BadRequest();

            var product = _productRepo.GetAll(p => p.Category)
                                      .FirstOrDefault(p => p.ProductId == id.Value);

            if (product == null) return NotFound();

            var model = new ProductVM
            {
                ProductId = product.ProductId,
                CategoryId = product.CategoryId,
                Name = product.Name,
                SKU = product.SKU,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                IsActive = product.IsActive,
                CreatedAt = product.CreatedAt,
                Category = product.Category
            };

            return View(model);
        }

   
        [HttpPost]
        public IActionResult Edit(ProductVM productVM)
        {
            ModelState.Remove("Category");
        


            if (!ModelState.IsValid)
            {
                var message = string.Join(" | ", ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));

                System.Diagnostics.Debug.WriteLine("Validation Errors: " + message);
            }
            if (ModelState.IsValid)
            {
                var product = _productRepo.GetById(productVM.ProductId);
                if (product == null) return NotFound();

                if (productVM.ImageFile != null)
                {
                    if (!string.IsNullOrEmpty(product.ImageUrl) && product.ImageUrl != "default.png")
                    {
                        string oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", product.ImageUrl);
                        if (System.IO.File.Exists(oldPath))
                        {
                            System.IO.File.Delete(oldPath);
                        }
                    }

                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(productVM.ImageFile.FileName);
                    string newPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                    using (var stream = new FileStream(newPath, FileMode.Create))
                    {
                        productVM.ImageFile.CopyTo(stream);
                    } 

                    product.ImageUrl = fileName;
                } 
                product.Name = productVM.Name;
                product.SKU = productVM.SKU;
                product.Price = productVM.Price;
                product.StockQuantity = productVM.StockQuantity;
                product.IsActive = productVM.IsActive;

                _productRepo.Update(product);

                return RedirectToAction("Details", "Categories", new { id = product.CategoryId });
            } 

            return View(productVM);
        } 

    }
    } 

