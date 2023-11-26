using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shipping_tracking.Models;
using shipping_tracking.Models.ViewModels;
using shipping_tracking.Services.Interfaces;

namespace shipping_tracking.Controllers
{
    [Route("/User")]
    public class UserController : Controller
    {
        private readonly MyDbContext _dbContext;
        private readonly IPasswordService _passwordService;
        private readonly ILogger<UserController> _logger;

        public UserController(MyDbContext dbContext, IPasswordService passwordService, ILogger<UserController> logger)
        {
            _dbContext = dbContext;
            _passwordService = passwordService;
            _logger = logger;
        }

        /// <summary>
        /// GET: /User/All
        /// Retrieves all non-deleted users and displays them in a view
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// GET: /User/Create
        /// Returns a view for creating a new user
        /// </summary>
        /// <returns></returns>
        [HttpGet("Create")]
        public async Task<IActionResult> CreateUser()
        {
            try
            {
                var roles = await _dbContext.Roles
                    .Where(c => c.IsDeleted == false)
                    .ToListAsync()
                    ?? Enumerable.Empty<Role>();

                if (roles is null)
                {
                    TempData["ExceptionError"] = "An error occurred while getting all roles.";
                }

                var viewModel = new UserRoleViewModel()
                {
                    Roles = roles
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(exception: ex, message: "An error occurred while getting all roles.");
                TempData["ExceptionError"] = "An error occurred while getting all roles.";

                return View(new UserRoleViewModel() { Roles = Enumerable.Empty<Role>() });
            }
        }

        /// <summary>
        /// POST: /User/Create
        /// Validates and adds a new user to the database
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUser(UserRoleViewModel viewModel)
        {
            try
            {
                var roles = await _dbContext.Roles
                                            .Where(c => c.IsDeleted == false)
                                            .ToListAsync()
                                            ?? Enumerable.Empty<Role>();

                if (roles.Count() == 0)
                {
                    TempData["ExceptionError"] = "An error occurred while getting all roles.";
                }

                bool userExists = await _dbContext.Users
                    .AnyAsync(u => u.isDeleted == false && u.Email == viewModel.User.Email);

                if (userExists)
                {
                    ModelState.AddModelError("User.Email", "The email already exists. Please enter another one.");
                    viewModel.Roles = roles;

                    return View(viewModel);
                }

                if (viewModel.User.ImageFile != null && viewModel.User.ImageFile.Length > 0)
                {
                    var fileName = Path.GetFileName(viewModel.User.ImageFile.FileName);
                    var uniqueFileName = $"{Guid.NewGuid()}_{fileName}";
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", uniqueFileName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await viewModel.User.ImageFile.CopyToAsync(stream);
                    }

                    // Save the path to the database
                    viewModel.User.ImagePath = $"/images/{uniqueFileName}";
                }

                // Create user
                viewModel.User.Password = _passwordService.HashPassword(viewModel.User.Password);

                await _dbContext.Users.AddAsync(viewModel.User);
                int created = await _dbContext.SaveChangesAsync();

                if (created > 0)
                {
                    TempData["successMessage"] = "User created successfully";
                    return RedirectToAction(nameof(AllUsers));
                }
                else
                {
                    ModelState.AddModelError("", "Unable to create a new User. Please try again later.");
                    viewModel.Roles = roles;

                    return View(viewModel);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(exception: ex, message: "An error occurred while createing new user.");
                TempData["ExceptionError"] = "An error occurred while createing new user.";
                viewModel.Roles = Enumerable.Empty<Role>();

                return View(viewModel);
            }
        }

        /// <summary>
        /// GET: /User/Update/{id}
        /// Retrieves a user by ID for updating
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("Update/{id}")]
        public async Task<IActionResult> UpdateUser(int id)
        {
            try
            {
                var user = _dbContext.Users
                    .Include(u => u.Role)
                    .SingleOrDefault(u => u.Id == id && u.isDeleted == false);

                if (user is null)
                {
                    TempData["ExceptionError"] = "User not found or an error occurred while getting the user";
                    return RedirectToAction(nameof(AllUsers));
                }

                var roles = await _dbContext.Roles
                                            .Where(c => c.IsDeleted == false)
                                            .ToListAsync()
                                            ?? Enumerable.Empty<Role>();

                if (roles.Count() == 0)
                {
                    TempData["ExceptionError"] = "An error occurred while getting all roles.";
                }

                var viewModel = new UserRoleViewModel()
                {
                    User = user,
                    Roles = roles
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(exception: ex, message: $"An error occurred while getting the user with ID {id}.");
                TempData["ExceptionError"] = "An error occurred while getting the user";

                return RedirectToAction(nameof(AllUsers));
            }
        }

        /// <summary>
        /// POST: /User/Update/{id}
        /// Validates and updates a user in the database
        /// </summary>
        /// <param name="id"></param>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [HttpPost("Update/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateUser(int id, UserRoleViewModel viewModel)
        {
            try
            {
                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(exception: ex, message: $"An error occurred while update the user.");
                TempData["ExceptionError"] = "An error occurred while update the user";

                return RedirectToAction(nameof(AllUsers));
            }
        }


    }
}
