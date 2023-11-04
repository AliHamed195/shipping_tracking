using shipping_tracking.BackEnd.Interfaces;

namespace shipping_tracking.BackEnd.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        public Task<bool> deletePaymentByIdAsync(int PaymentId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> getAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> getPaymentByIdAsync(int PaymentId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> updatePaymentByIdAsync(int PaymentId)
        {
            throw new NotImplementedException();
        }
    }
}
