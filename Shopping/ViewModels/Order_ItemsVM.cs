using ITIEntities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shopping.ViewModels
{
    public class Order_ItemsVM
    {


        public int OrderItemID { get; set; }
        public virtual Order? Order { get; set; }
        public int OrderId { get; set; }

        public virtual Product? Product { get; set; }
        public int ProductId { get; set; }

        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal LineTotal { get; set; }
    }
}
