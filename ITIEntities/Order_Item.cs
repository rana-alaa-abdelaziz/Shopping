using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ITIEntities
{
    public class Order_Item
    {
        [Key]
        public int OrderItemID { get; set; }
        public virtual Order Order { get; set; }
        [ForeignKey(nameof(Order))]
        public int OrderId { get; set; }

        public virtual Product Product { get; set; }
        [ForeignKey(nameof(Product))]
        public int ProductId { get; set; }

        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal LineTotal { get; set; }


    }
}
