namespace Shopping.ViewModels
{
    public class OrderVM
    {
        public int? ShippingAddressId { get; set; }

        public string Status { get; set; } 

        public decimal TotalAmount { get; set; } = 0;
    }
}
