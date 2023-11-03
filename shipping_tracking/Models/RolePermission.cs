using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace shipping_tracking.Models
{
    public class RolePermission
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Role")]
        public int RoleId { get; set; }
        public Role? Role { get; set; }

        [ForeignKey("Permission")]
        public int PermissionId { get; set; }
        public Permission? Permission { get; set; }

        public int IsDeleted { get; set; } = 0;
    }
}
