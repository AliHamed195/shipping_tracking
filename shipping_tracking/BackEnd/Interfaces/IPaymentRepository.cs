namespace shipping_tracking.BackEnd.Interfaces
{
    public interface IPaymentRepository
    {
        public Task<bool> getAllAsync();
        public Task<bool> getPaymentByIdAsync(int PaymentId);
        public Task<bool> updatePaymentByIdAsync(int PaymentId);
        public Task<bool> deletePaymentByIdAsync(int PaymentId);
    }
}
