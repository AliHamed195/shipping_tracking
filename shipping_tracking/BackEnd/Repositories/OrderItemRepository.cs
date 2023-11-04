using shipping_tracking.BackEnd.Interfaces;

namespace shipping_tracking.BackEnd.Repositories
{
    public class OrderItemRepository : IOrderItemRepository
    {
        public Task<bool> deleteOrderItemByIdAsync(int OrderItemId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> getAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> getOrderItemByIdAsync(int OrderItemId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> updateOrderItemByIdAsync(int OrderItemId)
        {
            throw new NotImplementedException();
        }
    }
}
