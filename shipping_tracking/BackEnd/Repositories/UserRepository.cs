using User_tracking.BackEnd.Interfaces;

namespace shipping_tracking.BackEnd.Repositories
{
    public class UserRepository : IUserRepository
    {
        public Task<bool> deleteUserByIdAsync(int UserId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> getAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> getUserByIdAsync(int UserId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> updateUserByIdAsync(int UserId)
        {
            throw new NotImplementedException();
        }
    }
}
