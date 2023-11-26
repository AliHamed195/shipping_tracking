using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace shipping_tracking.Models
{
    public class Product
    {
        [Key]
        public int ProductID { get; set; }

        [Required(ErrorMessage = "Product name is required.")]
        [Display(Name = "Product Name")]
        [StringLength(256)]
        public string ProductName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description name is required.")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Product price is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Stock quantity is required.")]
        [Display(Name = "Stock Quantity")]
        public int StockQuantity { get; set; }

        [Display(Name = "Product Image")]
        public string imagePath { get; set; }

        [NotMapped]
        public IFormFile? ImageFile { get; set; }

        [ForeignKey("Category")]
        public int CategoryID { get; set; }

        public Category? Category { get; set; }

        public DateTime CreatedOn { get; set; } = DateTime.Now;

        public bool IsDeleted { get; set; } = false;
    }
}
