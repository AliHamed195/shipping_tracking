using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using shipping_tracking.Models;
using shipping_tracking.Models.ViewModels;
using shipping_tracking.Services.Interfaces;
using System.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace shipping_tracking.Controllers
{
    [Route("/User")]
    public class UserInfoController : Controller
    {
        private readonly MyDbContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<UserInfoController> _logger;

        public UserInfoController(MyDbContext dbContext, RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager, ILogger<UserInfoController> logger)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        [HttpGet("All")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AllUsers()
        {
            var users = await _dbContext.Users
                                        .Where(x => x.isDeleted == false)
                                        .Include(u => u.AspNetUser) 
                                        .AsNoTracking()
                                        .ToListAsync();

            return View(users);
        }


        [HttpGet("Create")]
        [Authorize(Roles = "Admin")]
        public IActionResult CreateUser()
        {
            var roles = _roleManager.Roles.ToList();
            ViewBag.Roles = new SelectList(roles, "Name", "Name");

            return View();
        }


        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateUser(UserInfo userInfo, string roleName, string password)
        {
            // TODO: ====================================
            // need to fix the posible empty fields .....
            // TODO: ====================================
            // need to add alert for success .....

            try
            {
                if (userInfo.ImageFile != null && userInfo.ImageFile.Length > 0)
                {
                    var fileName = Path.GetFileName(userInfo.ImageFile.FileName);
                    var uniqueFileName = $"{Guid.NewGuid()}_{fileName}";
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", uniqueFileName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await userInfo.ImageFile.CopyToAsync(stream);
                    }

                    userInfo.ImagePath = $"/images/{uniqueFileName}";
                }

                var user = new IdentityUser { UserName = userInfo.AspNetUser.UserName, Email = userInfo.AspNetUser.Email };
                var result = await _userManager.CreateAsync(user, password);

                if (result.Succeeded)
                {
                    // Assign the role
                    await _userManager.AddToRoleAsync(user, roleName);

                    // Save additional user info
                    userInfo.AspNetUserId = user.Id;
                    userInfo.AspNetUser = user;
                    _dbContext.Users.Add(userInfo);
                    await _dbContext.SaveChangesAsync();

                    return RedirectToAction(nameof(AllUsers));
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    var roles = _roleManager.Roles.ToList();
                    ViewBag.Roles = new SelectList(roles, "Name", "Name");
                }

                return View(userInfo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a user");
                ModelState.AddModelError(string.Empty, "An error occurred while processing your request.");

                return View(userInfo);
            }
        }

        [HttpGet("Update/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var userInfo = await _dbContext.Users.FirstOrDefaultAsync(u => u.AspNetUserId == id);
            if (userInfo == null)
            {
                return NotFound();
            }

            var roles = await _userManager.GetRolesAsync(user);
            var roleList = _roleManager.Roles.ToList();
            ViewBag.Roles = new SelectList(roleList, "Name", "Name", roles.FirstOrDefault());

            return View(userInfo);
        }

        [HttpPost("Update/{id}")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateUser(string id, UserInfo userInfo, string roleName)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user is null)
                {
                    return NotFound();
                }

                user.Email = userInfo.AspNetUser.Email;
                user.UserName = userInfo.AspNetUser.UserName;

                var updateResult = await _userManager.UpdateAsync(user);
                if (!updateResult.Succeeded)
                {
                    foreach (var error in updateResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View(userInfo);
                }

                var oldUserInfo = await _dbContext.Users.FirstOrDefaultAsync(u => u.AspNetUserId == id);
                if (oldUserInfo is null)
                {
                    return NotFound();
                }

                if (userInfo.ImageFile != null && userInfo.ImageFile.Length > 0)
                {
                    if (!string.IsNullOrEmpty(oldUserInfo.ImagePath))
                    {
                        var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", oldUserInfo.ImagePath);
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    var fileName = Path.GetFileName(userInfo.ImageFile.FileName);
                    var uniqueFileName = $"{Guid.NewGuid()}_{fileName}";
                    var newPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", uniqueFileName);

                    using (var stream = new FileStream(newPath, FileMode.Create))
                    {
                        await userInfo.ImageFile.CopyToAsync(stream);
                    }

                    userInfo.ImagePath = $"/images/{uniqueFileName}";
                }

                oldUserInfo.ImagePath = userInfo.ImagePath;
                oldUserInfo.Address = userInfo.Address;

                await _dbContext.SaveChangesAsync();

                return RedirectToAction(nameof(AllUsers));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the user");
                ModelState.AddModelError(string.Empty, "An error occurred while processing your request.");
                return View(userInfo);
            }
        }



        // need to create user details 

        // need to create delete user

    }
}
