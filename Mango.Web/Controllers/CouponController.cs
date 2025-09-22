using Mango.Web.Models;
using Mango.Web.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Reflection;

namespace Mango.Web.Controllers
{
    public class CouponController(ICouponService _couponService) : Controller
    {
        public async Task<IActionResult> Index()
        {
            List<CouponDto> coupons = new();

            ResponseDto? response = await _couponService.GetAllCouponsAsync();

            if (response != null && response.IsSuccess)
            {
                coupons = JsonConvert.DeserializeObject<List<CouponDto>>(Convert.ToString(response.Result));                
            }
            else
            {
                TempData["error"] = response?.Message;
            }

            return View(coupons);
        }

        public async Task<IActionResult> CouponCreate()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CouponCreate(CouponDto model)
        {
            if (ModelState.IsValid)
            {
                ResponseDto response = await _couponService.CreateCouponsAsync(model);

                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = $"Coupon {model.CouponCode} Created";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["error"] = response?.Message;
                }
            }

            return View(model);
        }

        public async Task<IActionResult> CouponDelete(int couponId)
        {
            CouponDto coupon = new();

            ResponseDto? response = await _couponService.GetCouponByIdAsync(couponId);

            if (response != null && response.IsSuccess)
            {
                coupon = JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(response.Result));
            }
            else
            {
                TempData["error"] = response?.Message;
            }

            return View(coupon);
        }

        [HttpPost]
        public async Task<IActionResult> CouponDelete(CouponDto model)
        {
            ResponseDto? response = await _couponService.DeleteCouponsAsync(model.CouponId);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Coupon Deleted";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["error"] = response?.Message;
            }

            return View(model);
        }

        public async Task<IActionResult> CouponUpdate(int couponId)
        {
            CouponDto coupon = new();

            ResponseDto? response = await _couponService.GetCouponByIdAsync(couponId);

            if (response != null && response.IsSuccess)
            {
                coupon = JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(response.Result));
            }
            else
            {
                TempData["error"] = response?.Message;
            }

            return View(coupon);
        }

        [HttpPost]
        public async Task<IActionResult> CouponUpdate(CouponDto model)
        {
            if (ModelState.IsValid)
            {
                ResponseDto response = await _couponService.UpdateCouponsAsync(model);

                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = $"Coupon {model.CouponCode} Updated";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["error"] = response?.Message;
                }
            }

            return View(model);
        }

        public async Task<IActionResult> ViewPartial(int id)
        {
            CouponDto coupon = new();

            var response = await _couponService.GetCouponByIdAsync(id);
            
            coupon = JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(response.Result));

            return PartialView("_ViewCouponPartial", coupon);
        }
    }
}
