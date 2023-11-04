using shipping_tracking.BackEnd.Interfaces;

namespace shipping_tracking.BackEnd.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        public Task<bool> deleteOrderByIdAsync(int OrderId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> getAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> getOrderByIdAsync(int OrderId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> updateOrderByIdAsync(int OrderId)
        {
            throw new NotImplementedException();
        }
    }
}
