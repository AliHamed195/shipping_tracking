using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shipping_tracking.Models;

namespace shipping_tracking.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly MyDbContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;

        public CartController(MyDbContext dbContext, UserManager<IdentityUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        [HttpGet("Get")]
        public async Task<IActionResult> GetUserOrders()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
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
                              .Where(o => o.UserID == userId && o.IsDeleted == false)
                              .AsNoTracking()
                              .ToListAsync();
            return Ok(orders);
        }


    }
}
