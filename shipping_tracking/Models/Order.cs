using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace shipping_tracking.Models
{
    public class Order
    {
        [Key]
        public int OrderID { get; set; }

        [ForeignKey("User")]
        public int UserID { get; set; }

        public UserInfo? User { get; set; }

        [Required(ErrorMessage = "Total Price is required.")]
        [Display(Name = "Total Price")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice { get; set; }

        [Required(ErrorMessage = "OrderStatus is erquired.")]
        [Display(Name = "Order Status")]
        public string OrderStatus { get; set; } 

        public DateTime CreatedOn { get; set; } = DateTime.Now;

        public bool IsDeleted { get; set; } = false;
    }
}
