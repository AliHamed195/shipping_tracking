using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using shipping_tracking.Models;
using System.Security.Claims;

namespace shipping_tracking.Controllers
{
    [Route("Account")]
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly MyDbContext _context;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager, MyDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _context = context;
        }

        [HttpGet("Login")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost("Login")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, password, isPersistent: false, lockoutOnFailure: false);
                    if (result.Succeeded)
                    {
                        // send it to the home page so he can start shopping....
                        // i need to add alert

                        // need to fix the insert user first.
                        //var userInfo = await _context.Users.FirstOrDefaultAsync(u => u.AspNetUserId == user.Id);
                        //var userIdClaim = new Claim(ClaimTypes.NameIdentifier, userInfo.Id.ToString());
                        //var identity = new ClaimsIdentity();
                        //identity.AddClaim(userIdClaim);

                        await _signInManager.RefreshSignInAsync(user);
                        TempData["LoginSuccess"] = $"Welcome Back {user.UserName}";
                        // Store a string in the session: (to activate the session)
                        HttpContext.Session.SetString("UserName", "Ali");
                        return RedirectToAction("HomePage", "Home");
                    }
                    else if (result.IsLockedOut)
                    {
                        ModelState.AddModelError(string.Empty, "IsLockedOut.");
                    }
                    else if (result.IsNotAllowed)
                    {
                        // the problem is here ...
                        ModelState.AddModelError(string.Empty, "IsNotAllowed.");
                    }
                    else if (result.RequiresTwoFactor)
                    {
                        ModelState.AddModelError(string.Empty, "RequiresTwoFactor.");
                    }
                }

                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View();
            }
            catch (Exception)
            {
                // need to add alert to the view
                return View();
            }
        }

        [HttpGet("Register")]
        public IActionResult Register()
        {
            var roles = _roleManager.Roles.ToList();
            ViewBag.Roles = new SelectList(roles, "Name", "Name");

            return View();
        }


        [HttpPost("Register")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(UserInfo userInfo, string roleName, string password)
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
                    _context.Users.Add(userInfo);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Index));
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
                ModelState.AddModelError(string.Empty, "An error occurred while processing your request.");

                return View(userInfo);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            if (User.Identity.IsAuthenticated)
            {
                await _signInManager.SignOutAsync();
                // Clear the session
                HttpContext.Session.Clear();
            }
            
            return RedirectToAction("HomePage", "Home");
        }
    }
}
