using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shipping_tracking.Models;
using shipping_tracking.Models.ViewModels;
using System;

namespace shipping_tracking.Controllers
{
    [Route("/Product")]
    public class ProductController : Controller
    {
        private readonly MyDbContext _dbContext;
        private readonly ILogger<ProductController> _logger;

        public ProductController(MyDbContext dbContext, ILogger<ProductController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        [HttpGet("All")]
        public async Task<IActionResult> AllProducts()
        {
            try
            {
                var products = await _dbContext.Products
                    .Where(c => c.IsDeleted == false)
                    .Include(p => p.Category)
                    .ToListAsync()
                    ?? Enumerable.Empty<Product>();

                return View(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(exception: ex, message: "An error occurred while getting all products.");
                return View(Enumerable.Empty<Product>());
            }
        }

        [HttpGet("Create")]
        public async Task<IActionResult> CreateProduct()
        {
            try
            {
                var categories = await _dbContext.Categories
                    .Where(c => c.IsDeleted == false)
                    .ToListAsync()
                    ?? Enumerable.Empty<Category>();

                if (categories is null)
                {
                    TempData["ExceptionError"] = "An error occurred while getting all categories.";
                }

                var viewModel = new ProductCategoriesViewModel()
                {
                    Categories = categories
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(exception: ex, message: "An error occurred while getting all categories.");
                TempData["ExceptionError"] = "An error occurred while getting all categories.";

                return View(new ProductCategoriesViewModel() { Categories = Enumerable.Empty<Category>() });
            }
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProduct(ProductCategoriesViewModel viewModel)
        {
            try
            {
                var categories = await _dbContext.Categories
                    .Where(c => c.IsDeleted == false)
                    .ToListAsync()
                    ?? Enumerable.Empty<Category>();

                if (categories is null)
                {
                    TempData["ExceptionError"] = "An error occurred while getting all categories.";
                }

                // Check if category name already exists
                bool productExists = await _dbContext.Products
                                           .AnyAsync(p =>
                                           p.ProductName == viewModel.Product.ProductName &&
                                           p.CategoryID == viewModel.Product.CategoryID
                                           );
                if (productExists)
                {
                    ModelState.AddModelError("Product.ProductName", "The name already exists. Please enter another one.");
                    viewModel.Categories = categories;

                    return View(viewModel);
                }

                if (viewModel.Product.ImageFile != null && viewModel.Product.ImageFile.Length > 0)
                {
                    var fileName = Path.GetFileName(viewModel.Product.ImageFile.FileName);
                    var uniqueFileName = $"{Guid.NewGuid()}_{fileName}";
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", uniqueFileName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await viewModel.Product.ImageFile.CopyToAsync(stream);
                    }

                    // Save the path to the database
                    viewModel.Product.imagePath = $"/images/{uniqueFileName}";
                }

                // Create category
                await _dbContext.Products.AddAsync(viewModel.Product);
                int created = await _dbContext.SaveChangesAsync();

                if (created > 0)
                {
                    TempData["successMessage"] = "Product created successfully";
                    return RedirectToAction(nameof(AllProducts));
                }
                else
                {
                    ModelState.AddModelError("", "Unable to create a new Product. Please try again later.");
                    viewModel.Categories = categories;

                    return View(viewModel);
                }
            }
            catch (Exception ex)
            {
                // Log the exception details
                _logger.LogError(exception: ex, message: "An error occurred while creating new product.");
                TempData["ExceptionError"] = "An error occurred while creating the product. Please try again later.";
                viewModel.Categories = Enumerable.Empty<Category>();

                return View(viewModel);
            }
        }

        [HttpGet("Update/{id}")]
        public async Task<IActionResult> UpdateProduct(int id)
        {
            try
            {
                var product = await _dbContext.Products
                    .Where(p => p.IsDeleted == false)
                    .Include(c => c.Category)
                    .FirstOrDefaultAsync(p => p.ProductID == id);

                if (product is null)
                {
                    TempData["ExceptionError"] = "An error occurred while getting the product.";
                    return View(new ProductCategoriesViewModel() { Categories = Enumerable.Empty<Category>() });
                }

                var categories = await _dbContext.Categories
                    .Where(c => c.IsDeleted == false)
                    .ToListAsync()
                    ?? Enumerable.Empty<Category>();

                if (categories is null)
                {
                    TempData["ExceptionError"] = "An error occurred while getting all categories.";
                }

                var viewModel = new ProductCategoriesViewModel
                {
                    Product = product,
                    Categories = categories
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(exception: ex, message: $"An error occurred while getting the product with ID {id}.");
                TempData["ExceptionError"] = "An error occurred while getting the product";

                return RedirectToAction(nameof(AllProducts));
            }
        }


        [HttpPost("Update/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProduct(int id, ProductCategoriesViewModel viewModel)
        {
            try
            {
                var categories = await _dbContext.Categories
                    .Where(c => c.IsDeleted == false)
                    .ToListAsync()
                    ?? Enumerable.Empty<Category>();

                if (categories is null)
                {
                    TempData["ExceptionError"] = "An error occurred while getting all categories.";
                }

                // check if the product name is already exist
                var existingProductName = await _dbContext.Products
                    .FirstOrDefaultAsync(p =>
                    p.ProductID != id &&
                    p.ProductName == viewModel.Product.ProductName &&
                    p.IsDeleted == false &&
                    p.CategoryID == viewModel.Product.CategoryID
                    );

                // need to handel the error
                if (existingProductName is not null)
                {
                    ModelState.AddModelError("Product.ProductName", "The name already exists. Please enter another one.");
                    viewModel.Categories = categories;

                    return View(viewModel);
                }

                var oldProduct = await _dbContext.Products
                    .FirstOrDefaultAsync(p => p.ProductID == id && p.IsDeleted == false);

                if (oldProduct is null)
                {
                    TempData["ExceptionError"] = "An error occurred while getting the Product.";
                    viewModel.Categories = categories;

                    return View(viewModel);
                }

                // Handle image file
                if (viewModel.Product.ImageFile != null && viewModel.Product.ImageFile.Length > 0)
                {
                    if (!string.IsNullOrWhiteSpace(oldProduct.imagePath))
                    {
                        // Delete existing image
                        var existingFilePath = $"wwwroot{oldProduct.imagePath}";
                        if (System.IO.File.Exists(existingFilePath))
                        {
                            try
                            {
                                System.IO.File.Delete(existingFilePath);
                            }
                            catch (Exception ex)
                            {
                                _logger.LogError(ex, "Failed to delete existing image at {FilePath}", existingFilePath);
                                // need to handle this error
                            }
                        }
                    }

                    // Save new image
                    var fileName = Path.GetFileName(viewModel.Product.ImageFile.FileName);
                    var uniqueFileName = $"{Guid.NewGuid()}_{fileName}";
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", uniqueFileName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await viewModel.Product.ImageFile.CopyToAsync(stream);
                    }

                    viewModel.Product.imagePath = $"/images/{uniqueFileName}";
                }

                oldProduct.imagePath = viewModel.Product.imagePath;
                oldProduct.ProductName = viewModel.Product.ProductName;
                oldProduct.Description = viewModel.Product.Description;
                oldProduct.Price = viewModel.Product.Price;
                oldProduct.StockQuantity = viewModel.Product.StockQuantity;
                oldProduct.CategoryID = viewModel.Product.CategoryID;

                int result = await _dbContext.SaveChangesAsync();

                if (result > 0)
                {
                    TempData["successMessage"] = "Product updated successfully";
                    return RedirectToAction(nameof(AllProducts));
                }
                else
                {
                    TempData["ExceptionError"] = "An error occurred while updateing the Product.";
                    viewModel.Categories = categories;

                    return View(viewModel);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(exception: ex, message: $"An error occurred while updating the product with ID {id}.");
                TempData["ExceptionError"] = "An error occurred while updateing the Product.";
                viewModel.Categories = Enumerable.Empty<Category>();

                return View(viewModel);
            }
        }



    }
}
