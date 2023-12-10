namespace shipping_tracking.Models.ViewModels
{
    public class OrderItemViewModel
    {
        public int ProductId { get; set; }
        public decimal ProductPrice { get; set; }
        public int Quantity { get; set; }
    }
}
