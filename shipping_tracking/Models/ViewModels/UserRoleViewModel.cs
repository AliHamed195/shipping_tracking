namespace shipping_tracking.Models.ViewModels
{
    public class UserRoleViewModel
    {
        public User User { get; set; }

        public IEnumerable<Role> Roles { get; set; }
    }
}
