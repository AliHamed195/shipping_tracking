using Microsoft.AspNetCore.Mvc;
using shipping_tracking.Models;

namespace shipping_tracking.Controllers
{
    public class UserController : Controller
    {
        private readonly MyDbContext _dbContext;

        public UserController(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(User userProfile)
        {
            if (userProfile.ImageFile != null && userProfile.ImageFile.Length > 0)
            {
                var fileName = Path.GetFileName(userProfile.ImageFile.FileName);
                var uniqueFileName = $"{Guid.NewGuid()}_{fileName}";
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", uniqueFileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await userProfile.ImageFile.CopyToAsync(stream);
                }

                // Save the path to the database
                userProfile.ImagePath = $"/images/{uniqueFileName}";
                _dbContext.Users.Add(userProfile);
                await _dbContext.SaveChangesAsync();

                return Content("Done");
            }

            return View(userProfile);
        }
    }
}
