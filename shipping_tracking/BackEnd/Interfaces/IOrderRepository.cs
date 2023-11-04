namespace shipping_tracking.BackEnd.Interfaces
{
    public interface IOrderRepository
    {
        public Task<bool> getAllAsync();
        public Task<bool> getOrderByIdAsync(int OrderId);
        public Task<bool> updateOrderByIdAsync(int OrderId);
        public Task<bool> deleteOrderByIdAsync(int OrderId);
    }
}
