namespace shipping_tracking.Models.ViewModels
{
    public class OrderInfoViewModel
    {
        public OrderDetailsViewModel OrderDetailsViewModel { get; set;}
        public IEnumerable<OrderItemViewModel> OrderItemViewModel { get; set;}
        public PaymentViewModel PaymentViewModel { get; set;}
        public ShippingViewModel ShippingViewModel { get; set;}
    }
}
