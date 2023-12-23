namespace shipping_tracking.Models.ViewModels
{
    public class OrderDetailsViewModel
    {
        public int OrderID { get; set; }
        public decimal TotalPrice { get; set; }
        public string OrderStatus { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsDeleted { get; set; }

        public List<OrderItemViewModel> OrderItems { get; set; }
        public PaymentViewModel Payment { get; set; }
        public ShippingViewModel Shipping { get; set; }
    }
}
