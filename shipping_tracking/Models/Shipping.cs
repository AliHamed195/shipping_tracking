using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace shipping_tracking.Models
{
    public class Shipping
    {
        [Key]
        public int ShippingID { get; set; }

        [ForeignKey("Order")]
        public int OrderID { get; set; }

        public Order? Order { get; set; }

        [Required(ErrorMessage = "The address is requierd.")]
        [Display(Name = "Shipping Address")]
        public string ShippingAddress { get; set; } 

        [Display(Name = "Shipping Status")]
        public string ShippingStatus { get; set; }  

        [Display(Name = "Shipping Tracking Number")]
        public string ShippingTrackingNumber { get; set; } 

        [Display(Name = "Estimated Delivery Date")]
        public DateTime? EstimatedDeliveryDate { get; set; }

        public int IsDeleted { get; set; } = 0;
    }
}
