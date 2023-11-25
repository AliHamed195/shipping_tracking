namespace shipping_tracking.Models.ViewModels
{
    public class RolePermissionViewModel
    {
        public Role Role { get; set; }

        public IEnumerable<Permission> Permissions { get; set; }
    }
}
