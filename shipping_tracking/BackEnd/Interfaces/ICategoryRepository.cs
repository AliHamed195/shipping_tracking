namespace shipping_tracking.BackEnd.Interfaces
{
    public interface ICategoryRepository
    {
        public Task<bool> getAllAsync();
        public Task<bool> getCategoryByIdAsync(int CategoryId);
        public Task<bool> updateCategoryByIdAsync(int CategoryId);
        public Task<bool> deleteCategoryByIdAsync(int CategoryId);
    }
}
