using Microsoft.AspNetCore.Mvc;
using shipping_tracking.Models;
using System.Diagnostics;

namespace shipping_tracking.Controllers
{
    public class HomeController : Controller
    {
        private readonly MyDbContext _dbContext;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, MyDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public IActionResult HomePage()
        {
            var categories = _dbContext.Categories.ToList();
            return View(categories);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}