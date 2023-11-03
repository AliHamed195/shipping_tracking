using System.ComponentModel.DataAnnotations;

namespace shipping_tracking.Models
{
    public class Permission
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(256)]
        public string Name { get; set; }

        [Required]
        [StringLength(256)]
        public string ActionType { get; set; }

        // Permission have many Roles
        public ICollection<RolePermission> RolePermissions { get; set; }
    }
}
