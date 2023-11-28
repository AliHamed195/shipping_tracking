using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace shipping_tracking.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "User name is required.")]
        [Display(Name = "Full Name")]
        [StringLength(256)]
        public string Name { get; set; }

        [Required(ErrorMessage = "User Email Address is required.")]
        [Display(Name = "Email Address")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "User Password is required.")]
        public string Password { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.Now;

        public bool isDeleted { get; set; } = false;

        [Required(ErrorMessage = "User Phone Number is required.")]
        [Display(Name = "Phone Number")]
        [Phone]
        public string Phone { get; set; }

        [Display(Name = "Profile Image")]
        public string? ImagePath { get; set; }

        [Required(ErrorMessage = "User Address is required.")]
        public string Address { get; set; }

        [NotMapped]
        public IFormFile? ImageFile { get; set; }

    }
}
