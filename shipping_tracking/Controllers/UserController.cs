using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shipping_tracking.Models;

namespace shipping_tracking.Controllers
{
    [Route("/User")]
    public class UserController : Controller
    {
        private readonly MyDbContext _dbContext;
        private readonly ILogger<UserController> _logger;

        public UserController(MyDbContext dbContext, ILogger<UserController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        [HttpGet("All")]
        public async Task<IActionResult> AllUsers()
        {
            try
            {
                var users = await _dbContext.Users
                    .Where(c => c.isDeleted == false)
                    .Include(p => p.Role)
                    .ToListAsync()
                    ?? Enumerable.Empty<User>();

                return View(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(exception: ex, message: "An error occurred while getting all users.");
                return View(Enumerable.Empty<User>());
            }
        }


    }
}
