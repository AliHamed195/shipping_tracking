using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shipping_tracking.Models;

namespace shipping_tracking.Controllers
{
    [Route("/Category")]
    public class CategoryController : Controller
    {
        private readonly MyDbContext _dbContext;
        private readonly ILogger<CategoryController> _logger;

        /// <summary>
        /// Constructor: Initializes the controller with database context and logger
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="logger"></param>
        public CategoryController(MyDbContext dbContext, ILogger<CategoryController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }


        /// <summary>
        /// GET: /Category/All
        /// Retrieves all non-deleted categories and displays them in a view
        /// </summary>
        /// <returns></returns>
        [HttpGet("All")]
        public async Task<IActionResult> AllCategories()
        {
            try
            {
                var categories = await _dbContext.Categories
                    .Where(c => c.IsDeleted == false)
                    .ToListAsync()
                    ?? Enumerable.Empty<Category>();

                return View(categories);
            }
            catch (Exception ex)
            {
                _logger.LogError(exception: ex, message: "An error occurred while getting all categories.");
                return View(Enumerable.Empty<Category>());
            }
        }

        /// <summary>
        /// GET: /Category/Create
        /// Returns a view for creating a new category
        /// </summary>
        /// <returns></returns>
        [HttpGet("Create")]
        public IActionResult CreateCategory()
        {
            return View();
        }

        /// <summary>
        /// POST: /Category/Create
        /// Validates and adds a new category to the database
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCategory(Category category)
        {
            try
            {
                // Check if category name already exists
                bool categoryExists = await _dbContext.Categories
                                           .AnyAsync(c => c.CategoryName == category.CategoryName && c.IsDeleted == false);
                if (categoryExists)
                {
                    ModelState.AddModelError("CategoryName", "The name already exists. Please enter another one.");
                    return View(category);
                }

                // Create category
                await _dbContext.Categories.AddAsync(category);
                int created = await _dbContext.SaveChangesAsync();

                if (created > 0)
                {
                    TempData["successMessage"] = "Category created successfully";
                    return RedirectToAction(nameof(AllCategories));
                }
                else
                {
                    ModelState.AddModelError("", "Unable to create a new category. Please try again later.");
                    return View(category);
                }
            }
            catch (Exception ex)
            {
                // Log the exception details
                _logger.LogError(exception: ex, message: "An error occurred in create new category.");
                ViewBag.ExceptionError = "An error occurred while creating the category. Please try again later.";
                return View(category);
            }
        }

        /// <summary>
        /// GET: /Category/Update/{id}
        /// Retrieves a category by ID for updating
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("Update/{id}")]
        public async Task<IActionResult> UpdateCategory(int id)
        {
            try
            {
                var category = await _dbContext.Categories
                    .FirstOrDefaultAsync(c => c.CategoryID == id && c.IsDeleted == false);

                if (category is null)
                {
                    category = new Category();
                    ViewBag.ExceptionError = "An error occurred while getting the category. Please try again later.";
                }

                return View(category);
            }
            catch (Exception ex)
            {
                // Log the exception details
                _logger.LogError(exception: ex, message: "An error occurred while getting the category.");
                ViewBag.ExceptionError = "An error occurred while getting the category. Please try again later.";
                return View(new Category());
            }
        }

        /// <summary>
        /// POST: /Category/Update/{id}
        /// Validates and updates a category in the database
        /// </summary>
        /// <param name="id"></param>
        /// <param name="category"></param>
        /// <returns></returns>
        [HttpPost("Update/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateCategory(int id, Category category)
        {
            try
            {
                // Check if another category with the same name exists
                bool nameExists = await _dbContext.Categories
                                      .AnyAsync(c =>
                                      c.CategoryID != id &&
                                      c.CategoryName == category.CategoryName &&
                                      c.IsDeleted == false
                                      );

                if (nameExists)
                {
                    ModelState.AddModelError("CategoryName", "A category with the same name already exists. Please choose a different name.");
                    return View(category);
                }

                var oldCategory = await _dbContext.Categories
                    .FirstOrDefaultAsync(c => c.CategoryID == id && c.IsDeleted == false);

                int result = -1;

                if (oldCategory is not null)
                {
                    oldCategory.CategoryName = category.CategoryName;
                    oldCategory.Description = category.Description;

                    result = await _dbContext.SaveChangesAsync();
                }

                if (result > 0)
                {
                    TempData["successMessage"] = "Category updated successfully";
                    return RedirectToAction(nameof(AllCategories));
                }
                else
                {
                    ModelState.AddModelError("", "Unable to update the category. Please try again later.");
                    return View(category);
                }
            }
            catch (Exception ex)
            {
                // Log the exception details
                _logger.LogError(exception: ex, message: "An error occurred while updateing the category.");
                ViewBag.ExceptionError = "An error occurred while updateing the category. Please try again later.";
                return View(category);
            }
        }

        /// <summary>
        /// GET: /Category/Details/{id}
        /// Retrieves details of a specific category by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("Details/{id}")]
        public async Task<IActionResult> DetailsCategory(int id)
        {

            try
            {
                var category = await _dbContext.Categories
                    .FirstOrDefaultAsync(c => c.CategoryID == id && c.IsDeleted == false);

                if (category is null)
                {
                    category = new Category();
                    ViewBag.ExceptionError = "An error occurred while getting the category. Please try again later.";
                }

                return View(category);
            }
            catch (Exception ex)
            {
                // Log the exception details
                _logger.LogError(exception: ex, message: "An error occurred while getting the category.");
                ViewBag.ExceptionError = "An error occurred while getting the category. Please try again later.";
                return View(new Category());
            }
        }

        /// <summary>
        /// POST: /Category/Delete/{id}
        /// Marks a category as deleted in the database
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("Delete/{id}")]
        public async Task<JsonResult> DeleteCategory(int id)
        {
            try
            {
                var category = await _dbContext.Categories
                    .FirstOrDefaultAsync(c => c.CategoryID == id && c.IsDeleted == false);

                if (category is null)
                {
                    return Json(new { success = false, message = "Category not found." });
                }

                category.IsDeleted = true;
                int result = await _dbContext.SaveChangesAsync();

                if (result > 0)
                {
                    return Json(new { success = true, message = "Category deleted successfully." });
                }
                else
                {
                    return Json(new { success = false, message = "An error occurred while deleting the category." });
                }
            }
            catch (Exception ex)
            {
                // Log the exception details here
                _logger.LogError(exception: ex, message: "An error occurred while deleting the category.");

                return Json(new { success = false, message = "An error occurred while deleting the category. Please try again later." });
            }
        }


    }
}
