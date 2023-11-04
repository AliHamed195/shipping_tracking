namespace shipping_tracking.BackEnd.Interfaces
{
    public interface IRoleRepository
    {
        public Task<bool> getAllAsync();
        public Task<bool> getRoleByIdAsync(int RoleId);
        public Task<bool> updateRoleByIdAsync(int RoleId);
        public Task<bool> deleteRoleByIdAsync(int RoleId);
    }
}
