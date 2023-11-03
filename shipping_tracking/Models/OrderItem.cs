using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace shipping_tracking.Models
{
    public class OrderItem
    {
        [Key]
        public int OrderItemID { get; set; }

        [ForeignKey("Order")]
        public int OrderID { get; set; }

        public Order? Order { get; set; }

        [ForeignKey("Product")]
        public int ProductID { get; set; }
        public Product? Product { get; set; }

        public int Quantity { get; set; }

        [Required(ErrorMessage = "Price OrderItem is required.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}
