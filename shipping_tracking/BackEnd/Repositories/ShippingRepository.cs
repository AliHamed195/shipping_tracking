using shipping_tracking.BackEnd.Interfaces;

namespace shipping_tracking.BackEnd.Repositories
{
    public class ShippingRepository : IShippingRepository
    {
        public Task<bool> deleteShippingByIdAsync(int ShippingId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> getAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> getShippingByIdAsync(int ShippingId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> updateShippingByIdAsync(int ShippingId)
        {
            throw new NotImplementedException();
        }
    }
}
