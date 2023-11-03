using System.ComponentModel.DataAnnotations;

namespace shipping_tracking.Models
{
    public class Category
    {
        [Key]
        public int CategoryID { get; set; }

        [Required(ErrorMessage = "Please enter a Category Name")]
        [Display(Name = "Category Name")]
        [StringLength(256)]
        public string CategoryName { get; set; } 

        [Required(ErrorMessage = "Please enter a Description")]
        public string Description { get; set; } 

        public bool IsDeleted { get; set; } = false;

        public DateTime CreatedOn { get; set; } = DateTime.Now;
    }
}
