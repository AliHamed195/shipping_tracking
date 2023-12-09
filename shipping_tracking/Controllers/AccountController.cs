using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace shipping_tracking.Controllers
{
    [Route("Account")]
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
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
                        return RedirectToAction("HomePage", "Home");
                    }
                    if (result.IsLockedOut)
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
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("HomePage", "Home");
        }
    }
}
