using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shipping_tracking.Models;

namespace shipping_tracking.Controllers
{
    [Route("/Role")]
    public class RoleController : Controller
    {
        private readonly MyDbContext _dbContext;

        public RoleController(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        [HttpGet("All")]
        public async Task<IActionResult> AllRoles()
        {
            try
            {
                var roles = await _dbContext.Roles.ToListAsync() ?? Enumerable.Empty<Role>();

                return View(roles);
            }
            catch (Exception)
            {
                return View(Enumerable.Empty<Role>());
            }

        }

        [HttpGet("Create")]
        public async Task<IActionResult> CreateRole()
        {
            // The null paramiter means only get empty role and IEnumerable of permissions
            return View();
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateRole(string roleName, int[] permissionIds)
        {

            var success = false;
            if (success)
            {
                return RedirectToAction(nameof(AllRoles));
            }

            if (success)
            {
                return Json(new { success = true, redirectUrl = Url.Action(nameof(AllRoles)) });
            }
            else
            {
                Response.StatusCode = 400; // Bad Request
                return Json(new { success = false, message = "There was an error saving the role" });
            }
        }
    }
}
