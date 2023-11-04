namespace shipping_tracking.BackEnd.Interfaces
{
    public interface IShippingRepository
    {
        public Task<bool> getAllAsync();
        public Task<bool> getShippingByIdAsync(int ShippingId);
        public Task<bool> updateShippingByIdAsync(int ShippingId);
        public Task<bool> deleteShippingByIdAsync(int ShippingId);
    }
}
