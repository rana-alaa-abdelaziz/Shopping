using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ITIEntities
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        public string ImageUrl { get; set; }
        public virtual Category Category { get; set; }
        [ForeignKey(nameof(Category))]
        public int CategoryId { get; set; }
        public String Name { get; set; }
        public String SKU { get; set; } 
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }

        public ICollection<Order_Item> Order_Item { get; set; }

    }
}
