using System.ComponentModel.DataAnnotations;

namespace shipping_tracking.Models
{
    public class Role
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Role name is required.")]
        [Display(Name = "Role Name")]
        [StringLength(256)]
        public string Name { get; set; }

        public bool IsDeleted { get; set; } = false;

        // Role have many permissions
        public virtual ICollection<RolePermission>? RolePermissions { get; set; }

        // Role have many users
        public virtual ICollection<User>? Users { get; set; }
    }
}
