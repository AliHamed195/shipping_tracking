using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using shipping_tracking.Models;
using shipping_tracking.Models.ViewModels;

namespace shipping_tracking.Controllers
{
    [Route("Orders")]
    public class OrdersController : Controller
    {
        private readonly MyDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public OrdersController(MyDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost("SubmitOrder")]
        public async Task<IActionResult> SubmitOrder([FromBody] List<OrderItemViewModel> orderItems)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null)
            {
                var test = userIdClaim.Value;
            }

            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }

            var user = await _userManager.GetUserAsync(User);
           
            int userId = -1;
            var userInfo = _context.Users.Where(u => u.AspNetUserId == user.Id).FirstOrDefault();

            if (userInfo is null)
            {
                return Json(new { success = false, message = "An error happend. Please try again later..." });
            }

            if (userId == -1)
            {
                return Json(new { success = false, message = "An error happend. Please try again later..." });
            }

            var order = new Order
            {
                UserID = userId,
                TotalPrice = orderItems.Sum(i => i.Quantity * i.ProductPrice),
                OrderStatus = "In Process",
                CreatedOn = DateTime.Now,
                IsDeleted = false
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            int newOrderId = order.OrderID;

            foreach (var item in orderItems)
            {
                var orderItem = new OrderItem
                {
                    OrderID = newOrderId,
                    ProductID = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.ProductPrice,
                    IsDeleted = false
                };
                _context.OrderItems.Add(orderItem);
            }

            var payment = new Payment
            {
                OrderID = newOrderId,
                PaymentMethod = "Cash on Delivery",
                PaymentStatus = "Unpaid",
                TransactionID = Guid.NewGuid().ToString(),
                CreatedOn = DateTime.Now,
                IsDeleted = false
            };
            _context.Payments.Add(payment);

            var shippingAddress = await GetUserShippingAddress(userId);
            var shipping = new Shipping
            {
                OrderID = newOrderId,
                ShippingAddress = shippingAddress,
                ShippingStatus = "Not Shipped",
                ShippingTrackingNumber = GenerateShippingTrackingNumber(),
                EstimatedDeliveryDate = DateTime.Now.AddDays(2),
                IsDeleted = 0
            };
            _context.Shippings.Add(shipping);

            
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Order created successfully ..." });
        }

        private async Task<string> GetUserShippingAddress(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user != null)
            {
                return user.Address;
            }
            return "No address exist for this user";
        }

        private string GenerateShippingTrackingNumber()
        {
            return Guid.NewGuid().ToString();
        }


    }
}
