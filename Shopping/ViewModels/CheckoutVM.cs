using ITIEntities;

namespace Shopping.ViewModels
{
    public class CheckoutVM
    {

        public string Status { get; set; }

        public DateTime OrderDate { get; set; }
        public DateTime OrderTime { get; set; }

        public decimal TotalAmount { get; set; }
        public int? ShippingAddressId { get; set; }

        public String OrderNumber { get; set; }
        public virtual Address Address { get; set; }

    }
}
