namespace User_tracking.BackEnd.Interfaces
{
    public interface IUserRepository
    {
        public Task<bool> getAllAsync();
        public Task<bool> getUserByIdAsync(int UserId);
        public Task<bool> updateUserByIdAsync(int UserId);
        public Task<bool> deleteUserByIdAsync(int UserId);
    }
}
