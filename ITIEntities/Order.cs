using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ITIEntities
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        public virtual App_User App_User { get; set; }
        [ForeignKey(nameof(App_User))]
        public String UserId { get; set; }
        public virtual Address Address { get; set; }
        [ForeignKey(nameof(Address))]
        public int? ShippingAddressId { get; set; }

        public String OrderNumber { get; set; }

        public string Status { get; set; }

        public DateTime OrderDate { get; set; }
        public DateTime OrderTime { get; set; }

        public decimal TotalAmount { get; set; }
        public virtual ICollection<Order_Item> OrderItems { get; set; }
    }
}
