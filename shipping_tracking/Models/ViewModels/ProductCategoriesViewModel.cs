namespace shipping_tracking.Models.ViewModels
{
    public class ProductCategoriesViewModel
    {
        public Product Product { get; set; }

        public IEnumerable<Category> Categories { get; set; }
    }
}
