using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace shipping_tracking.Models
{
    public class UserInfo
    {
        [Key]
        public int Id { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.Now;

        public bool isDeleted { get; set; } = false;

        [Display(Name = "Profile Image")]
        public string? ImagePath { get; set; }

        [Required(ErrorMessage = "User Address is required.")]
        public string Address { get; set; }

        [NotMapped]
        public IFormFile? ImageFile { get; set; }

        // Foreign key for AspNetUsers
        public string AspNetUserId { get; set; }

        // Navigation property for AspNetUsers
        [ForeignKey("AspNetUserId")]
        public virtual IdentityUser AspNetUser { get; set; }

        public virtual ICollection<IdentityRole> Roles { get; set; }


    }
}
