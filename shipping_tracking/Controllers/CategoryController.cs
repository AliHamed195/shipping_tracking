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

        public CategoryController(MyDbContext dbContext, ILogger<CategoryController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }


        [HttpGet("All")]
        public async Task<IActionResult> AllCategories()
        {
            try
            {
                var categories = await _dbContext.Categories.ToListAsync() ?? Enumerable.Empty<Category>();
                return View(categories);
            }
            catch (Exception)
            {
                // I should handel the error by using sweetAlert2.
                return View(Enumerable.Empty<Category>());
            }
        }

        [HttpGet("Create")]
        public async Task<IActionResult> CreateCategory()
        {
            return View();
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCategory(Category category)
        {
            try
            {
                // Check if category name already exists
                bool categoryExists = await _dbContext.Categories
                                           .AnyAsync(c => c.CategoryName == category.CategoryName);
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
                _logger.LogError(ex, "An error occurred in create new category.");
                ViewBag.ExceptionError = "An error occurred while creating the category. Please try again later.";
                return View(category);
            }
        }


        [HttpGet("Update/{id}")]
        public async Task<IActionResult> UpdateCategory(int id)
        {
            try
            {
                var category = await _dbContext.Categories.FirstOrDefaultAsync(c => c.CategoryID == id);
                
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
                _logger.LogError(ex, "An error occurred while getting the category.");
                ViewBag.ExceptionError = "An error occurred while getting the category. Please try again later.";
                return View(new Category());
            }
        }

        [HttpPost("Update/{id}")]
        public async Task<IActionResult> UpdateCategory(int id, Category category)
        {
            try
            {
                // Check if another category with the same name exists
                bool nameExists = await _dbContext.Categories
                                      .AnyAsync(c => c.CategoryID != id && c.CategoryName == category.CategoryName);

                if (nameExists)
                {
                    ModelState.AddModelError("CategoryName", "A category with the same name already exists. Please choose a different name.");
                    return View(category);
                }

                var oldCategory = await _dbContext.Categories.FirstOrDefaultAsync(c => c.CategoryID == id);
                int result = -1;

                if (oldCategory is not null)
                {
                    oldCategory.CategoryName = category.CategoryName;
                    oldCategory.Description = category.Description;

                    result = await _dbContext.SaveChangesAsync();
                }

                if(result > 0)
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
                _logger.LogError(ex, "An error occurred while updateing the category.");
                ViewBag.ExceptionError = "An error occurred while updateing the category. Please try again later.";
                return View(category);
            }
        }

        [HttpGet("Details/{id}")]
        public async Task<IActionResult> DetailsCategory(int id)
        {

            try
            {
                var category = await _dbContext.Categories.FirstOrDefaultAsync(c => c.CategoryID == id);

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
                _logger.LogError(ex, "An error occurred while getting the category.");
                ViewBag.ExceptionError = "An error occurred while getting the category. Please try again later.";
                return View(new Category());
            }
        }
        [HttpPost("Delete/{id}")]
        public async Task<JsonResult> DeleteCategory(int id)
        {
            try
            {
                var category = await _dbContext.Categories.FindAsync(id);

                if (category == null)
                {
                    return Json(new { success = false, message = "Category not found." });
                }

                _dbContext.Categories.Remove(category);
                await _dbContext.SaveChangesAsync();

                return Json(new { success = true, message = "Category deleted successfully." });
            }
            catch (Exception ex)
            {
                // Log the exception details here
                _logger.LogError(ex, "An error occurred while deleting the category.");

                return Json(new { success = false, message = "An error occurred while deleting the category. Please try again later." });
            }
        }


    }
}
