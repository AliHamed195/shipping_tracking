using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shipping_tracking.Models;
using shipping_tracking.Models.ViewModels;

namespace shipping_tracking.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Customer, Admin, Employee")]
    public class CartController : ControllerBase
    {

        /// <summary>
        /// In my project there is no delete action. insted there is isDeleted(PUT)
        /// </summary>
        private readonly MyDbContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;

        public CartController(MyDbContext dbContext, UserManager<IdentityUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        [HttpGet("Get")] // Get All
        public async Task<IActionResult> GetUserOrders()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user is null)
            {
                return BadRequest();
            }

            int userId = -1;
            var userInfo = _dbContext.Users.Where(u => u.AspNetUserId == user.Id).FirstOrDefault();

            if (userInfo is null)
            {
                return BadRequest();
            }

            userId = userInfo.Id;

            var orders = await _dbContext.Orders
                              .Where(o => o.UserID == userId)
                              .AsNoTracking()
                              .ToListAsync();
            return Ok(orders);
        }


        [HttpGet("GetInfo/{id}")] // Get One By User Id
        public async Task<IActionResult> GetUserOrderInfo(int id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user is null)
            {
                return BadRequest();
            }

            int userId = -1;
            var userInfo = _dbContext.Users.Where(u => u.AspNetUserId == user.Id).FirstOrDefault();

            if (userInfo is null)
            {
                return BadRequest();
            }

            userId = userInfo.Id;

            var orderDetailsTask = await _dbContext.Orders
                                .Where(o => o.OrderID == id)
                                .Select(o => new OrderDetailsViewModel
                                {
                                    OrderID = o.OrderID,
                                    TotalPrice = o.TotalPrice,
                                    OrderStatus = o.OrderStatus,
                                    CreatedOn = o.CreatedOn,
                                    IsDeleted = o.IsDeleted
                                })
                                .FirstOrDefaultAsync();

            var orderItemsTask = await _dbContext.OrderItems
                                .Where(oi => oi.OrderID == id)
                                .Select(oi => new OrderItemViewModel
                                {
                                    ProductId = oi.ProductID,
                                    Quantity = oi.Quantity,
                                    ProductPrice = oi.Price
                                })
                                .ToListAsync();

            var paymentDetailsTask = await _dbContext.Payments
                                    .Where(p => p.OrderID == id)
                                    .Select(p => new PaymentViewModel
                                    {
                                        PaymentMethod = p.PaymentMethod,
                                        PaymentStatus = p.PaymentStatus,
                                        TransactionID = p.TransactionID
                                    })
                                    .FirstOrDefaultAsync();

            var shippingDetailsTask = await _dbContext.Shippings
                                    .Where(s => s.OrderID == id)
                                    .Select(s => new ShippingViewModel
                                    {
                                        ShippingAddress = s.ShippingAddress,
                                        ShippingStatus = s.ShippingStatus,
                                        ShippingTrackingNumber = s.ShippingTrackingNumber,
                                        EstimatedDeliveryDate = s.EstimatedDeliveryDate
                                    })
                                    .FirstOrDefaultAsync();


            var viewModel = new OrderInfoViewModel
            {
                OrderDetailsViewModel = orderDetailsTask,
                OrderItemViewModel = orderItemsTask,
                PaymentViewModel = paymentDetailsTask,
                ShippingViewModel = shippingDetailsTask
            };

            return Ok(viewModel);
        }


        [HttpPut("Cancel/{id}")] // Delete(PUT "isDeleted") Action
        public async Task<IActionResult> DeleteOrder(int id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user is null)
            {
                return BadRequest();
            }

            int userId = -1;
            var userInfo = _dbContext.Users.Where(u => u.AspNetUserId == user.Id).FirstOrDefault();

            if (userInfo is null)
            {
                return BadRequest();
            }

            userId = userInfo.Id;

            var order = await _dbContext.Orders
                              .Where(o => o.UserID == userId && o.IsDeleted == false && o.OrderID == id)
                              .FirstOrDefaultAsync();

            if (order is null)
            {
                return BadRequest();
            }

            order.IsDeleted = true;
            order.OrderStatus = "Cancelled";

            await _dbContext.SaveChangesAsync();

            return Ok();
        }


    }
}
