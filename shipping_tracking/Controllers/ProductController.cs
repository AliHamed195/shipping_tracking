using Microsoft.AspNetCore.Mvc;
using shipping_tracking.Models;

namespace shipping_tracking.Controllers
{
    public class ProductController : Controller
    {
        private readonly MyDbContext _dbContext;
        private readonly ILogger<ProductController> _logger;

        public ProductController(MyDbContext dbContext, ILogger<ProductController> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
