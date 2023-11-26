using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using shipping_tracking.Models;
using shipping_tracking.Models.ViewModels;

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
                var products = await _dbContext.Products.Include(p => p.Category).ToListAsync() ?? Enumerable.Empty<Product>();
                return View(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting all products.");
                return View(Enumerable.Empty<Product>());
            }
        }

        [HttpGet("Create")]
        public async Task<IActionResult> CreateProduct()
        {
            try
            {
                var categories = await _dbContext.Categories.ToListAsync();

                if (categories is null)
                {
                    // need to handle the error
                    return NotFound();
                }

                var viewModel = new ProductCategoriesViewModel()
                {
                    Categories = categories
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting all categories.");
                return View(new ProductCategoriesViewModel() { Categories = Enumerable.Empty<Category>() });
            }
        }

        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProduct(Product product)
        {
            try
            {
                // Check if category name already exists
                bool productExists = await _dbContext.Products
                                           .AnyAsync(p => p.ProductName == product.ProductName && p.CategoryID == product.CategoryID);
                if (productExists)
                {
                    ModelState.AddModelError("ProductName", "The name already exists. Please enter another one.");
                    return View(product);
                }

                if (product.ImageFile != null && product.ImageFile.Length > 0)
                {
                    var fileName = Path.GetFileName(product.ImageFile.FileName);
                    var uniqueFileName = $"{Guid.NewGuid()}_{fileName}";
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", uniqueFileName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await product.ImageFile.CopyToAsync(stream);
                    }

                    // Save the path to the database
                    product.imagePath = $"/images/{uniqueFileName}";
                }

                // Create category
                await _dbContext.Products.AddAsync(product);
                int created = await _dbContext.SaveChangesAsync();

                if (created > 0)
                {
                    TempData["successMessage"] = "Product created successfully";
                    return RedirectToAction(nameof(AllProducts));
                }
                else
                {
                    ModelState.AddModelError("", "Unable to create a new Product. Please try again later.");
                    return View(product);
                }
            }
            catch (Exception ex)
            {
                // Log the exception details
                _logger.LogError(ex, "An error occurred while creating new product.");
                ViewBag.ExceptionError = "An error occurred while creating the product. Please try again later.";
                return View(product);
            }
        }

        [HttpGet("Update/{id}")]
        public async Task<IActionResult> UpdateProduct(int id)
        {
            try
            {
                var product = await _dbContext.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.ProductID == id);
                if (product is null)
                {
                    // need to handle the error
                    return NotFound();
                }

                var categories = await _dbContext.Categories.ToListAsync();

                if (categories is null)
                {
                    // need to handle the error
                    return NotFound();
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
                _logger.LogError(ex, $"An error occurred while getting the product with ID {id}.");
                return RedirectToAction(nameof(AllProducts));
            }
        }


        [HttpPost("Update/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateProduct(int id, ProductCategoriesViewModel viewModel)
        {
            // need to check the uniqu name
            try
            {
                var existingProduct = await _dbContext.Products.FirstOrDefaultAsync(p => p.ProductID == id);

                // need to handel the error
                if (existingProduct is null)
                {
                    var categories = await _dbContext.Categories.ToListAsync();

                    if (categories is null)
                    {
                        // need to handle the error
                        return NotFound();
                    }

                    viewModel.Categories = categories;

                    return View(viewModel);
                }

                // Handle image file
                if (viewModel.Product.ImageFile != null && viewModel.Product.ImageFile.Length > 0)
                {
                    if (existingProduct is not null && !string.IsNullOrWhiteSpace(existingProduct.imagePath))
                    {
                        // Delete existing image
                        var existingFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", existingProduct.imagePath);
                        if (System.IO.File.Exists(existingFilePath))
                        {
                            System.IO.File.Delete(existingFilePath);
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

                existingProduct.imagePath = viewModel.Product.imagePath;
                existingProduct.ProductName = viewModel.Product.ProductName;
                existingProduct.Description = viewModel.Product.Description;
                existingProduct.Price = viewModel.Product.Price;
                existingProduct.StockQuantity = viewModel.Product.StockQuantity;
                existingProduct.CategoryID = viewModel.Product.CategoryID;

                int result = await _dbContext.SaveChangesAsync();

                if (result > 0)
                {
                    TempData["successMessage"] = "Product updated successfully";
                    return RedirectToAction(nameof(AllProducts));
                }
                else
                {
                    var categories = await _dbContext.Categories.ToListAsync();

                    if (categories is null)
                    {
                        // need to handle the error
                        return NotFound();
                    }

                    // need to handle the error
                    viewModel.Categories = categories;
                    return View(viewModel);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating the product with ID {id}.");
                viewModel.Categories = Enumerable.Empty<Category>();
                return View(viewModel);
            }
        }



    }
}
