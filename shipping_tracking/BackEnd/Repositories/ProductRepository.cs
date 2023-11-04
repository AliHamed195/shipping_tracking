using shipping_tracking.BackEnd.Interfaces;

namespace shipping_tracking.BackEnd.Repositories
{
    public class ProductRepository : IProductRepository
    {
        public Task<bool> deleteProductByIdAsync(int ProductId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> getAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> getProductByIdAsync(int ProductId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> updateProductByIdAsync(int ProductId)
        {
            throw new NotImplementedException();
        }
    }
}
