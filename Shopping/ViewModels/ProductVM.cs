using ITIEntities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shopping.ViewModels
{
    public class ProductVM
    {
        public int ProductId { get; set; }

        public virtual Category Category { get; set; }
        public int CategoryId { get; set; }
        public String Name { get; set; }
        public String SKU { get; set; } 
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? ImageUrl { get; set; }
        public IFormFile? ImageFile { get; set; }
    }
}
