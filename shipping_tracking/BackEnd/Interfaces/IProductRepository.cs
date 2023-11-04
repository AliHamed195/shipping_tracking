namespace shipping_tracking.BackEnd.Interfaces
{
    public interface IProductRepository
    {
        public Task<bool> getAllAsync();
        public Task<bool> getProductByIdAsync(int ProductId);
        public Task<bool> updateProductByIdAsync(int ProductId);
        public Task<bool> deleteProductByIdAsync(int ProductId);
    }
}
