using shipping_tracking.BackEnd.Interfaces;

namespace shipping_tracking.BackEnd.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        public Task<bool> deleteCategoryByIdAsync(int CategoryId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> getAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> getCategoryByIdAsync(int CategoryId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> updateCategoryByIdAsync(int CategoryId)
        {
            throw new NotImplementedException();
        }
    }
}
