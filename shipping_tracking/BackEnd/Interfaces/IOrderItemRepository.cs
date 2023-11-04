namespace shipping_tracking.BackEnd.Interfaces
{
    public interface IOrderItemRepository
    {
        public Task<bool> getAllAsync();
        public Task<bool> getOrderItemByIdAsync(int OrderItemId);
        public Task<bool> updateOrderItemByIdAsync(int OrderItemId);
        public Task<bool> deleteOrderItemByIdAsync(int OrderItemId);
    }
}
