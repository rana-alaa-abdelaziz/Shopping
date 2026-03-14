using Microsoft.AspNetCore.Mvc.Rendering;

namespace Shopping.ViewModels
{
    public class CategoryVM
    {
        public int CategoryId { get; set; }

        public string Name { get; set; }
        public int? ParentCategoryId { get; set; }

        public List<SelectListItem>? ParentCategories { get; set; }

    }
}
