using Mango.Web.Models;
using Mango.Web.Service.IService;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Mango.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IGenericService<CouponDto> _coupon;

        public HomeController(ILogger<HomeController> logger, IGenericService<CouponDto> coupon)
        {
            _logger = logger;
            _coupon = coupon;
        }

        public async Task<IActionResult> Index()
        {
            var coupons = await _coupon.GetAll();
            var coupon = await _coupon.GetByIdAsync(2);
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
