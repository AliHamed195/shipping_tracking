using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using shipping_tracking.Models;
using shipping_tracking.Models.ViewModels;

namespace shipping_tracking.Controllers
{
    [Route("Orders")]
    public class OrdersController : Controller
    {
        private readonly MyDbContext _context;

        public OrdersController(MyDbContext context)
        {
            _context = context;
        }

        [HttpPost("SubmitOrder")]
        public async Task<IActionResult> SubmitOrder([FromBody] List<OrderItemViewModel> orderItems)
        {
            return Json(new { success = true, message = "Order created successfuly ..." });
        }


    }
}
