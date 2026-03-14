using ITIEntities;
using ITIEntities.Repo;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Shopping.ViewModels;

namespace Shopping.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    public class CategoriesController : Controller
    {
        private readonly IRepo<Category> _categoryRepo;

        public CategoriesController(IRepo<Category> categoryRepo)
        {
            _categoryRepo = categoryRepo;
        }
        public IActionResult Index()
        {
            var model = _categoryRepo.GetAll();
            return View(model);
        }

        public IActionResult Create()
        {
            var categories = _categoryRepo.GetAll()
        .Select(c => new SelectListItem
        {
            Value = c.CategoryId.ToString(),
            Text = c.Name
        }).ToList();

            ViewBag.ParentCategories = categories;

            return View();
        }
        [HttpPost]
        public IActionResult Create(CategoryVM categoryVM)
        {
            if (ModelState.IsValid)
            {
                Category category = new Category
                {
                    Name = categoryVM.Name,
                    ParentCategoryId = categoryVM.ParentCategoryId
                };
                _categoryRepo.Add(category);
                return RedirectToAction(nameof(Index));
            }
            var categories = _categoryRepo.GetAll()
        .Select(c => new SelectListItem
        {
            Value = c.CategoryId.ToString(),
            Text = c.Name,
            Selected = c.CategoryId == categoryVM.ParentCategoryId
        }).ToList();
            ViewBag.Categories = _categoryRepo.GetAll();
            return View(categoryVM);
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null)
                return BadRequest();

            var category = _categoryRepo.GetById(id.Value);

            if (category == null)
                return NotFound();

            var model = new CategoryVM
            {
                CategoryId = category.CategoryId, 
                Name = category.Name,
                ParentCategoryId = category.ParentCategoryId
            };

            var categories = _categoryRepo.GetAll()
        .Where(c => c.CategoryId != id.Value)
        .Select(c => new SelectListItem
        {
            Value = c.CategoryId.ToString(),
            Text = c.Name,
            Selected = c.CategoryId == category.ParentCategoryId
        }).ToList();

            ViewBag.ParentCategories = categories;

            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(int id, CategoryVM categoryVM)
        {
            if (ModelState.IsValid)
            {
                var category = _categoryRepo.GetById(id);
                   if(category == null)
                {
                    return NotFound();
                }
                category.Name = categoryVM.Name;
                category.ParentCategoryId = categoryVM.ParentCategoryId;

                _categoryRepo.Update(category);
                return RedirectToAction(nameof(Index));
            }

            ViewBag.ParentCategories = _categoryRepo.GetAll()
        .Where(c => c.CategoryId != categoryVM.CategoryId)
        .Select(c => new SelectListItem
        {
            Value = c.CategoryId.ToString(),
            Text = c.Name
        }).ToList();

            return View(categoryVM);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var model = _categoryRepo.GetById(id.Value);
            if (model == null)
            {
                return NotFound();
            }
            return View(model);

        }

        [HttpPost]
        [ActionName("Delete")]
        public IActionResult DeleteConfirm(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            _categoryRepo.Delete(id.Value);
            return RedirectToAction(nameof(Index));


        }
        public IActionResult Details(int? id)
        {
            if (id == null) return BadRequest();

            var category = _categoryRepo.GetAll(c => c.Products)
                                            .FirstOrDefault(c => c.CategoryId == id.Value);

            if (category == null) return NotFound();

            return View(category);
        }


    }
}
