namespace shipping_tracking.Models.ViewModels
{
    public class ShippingViewModel
    {
        public string ShippingAddress { get; set; }
        public string ShippingStatus { get; set; }
        public string ShippingTrackingNumber { get; set; }
        public DateTime? EstimatedDeliveryDate { get; set; }
    }
}
