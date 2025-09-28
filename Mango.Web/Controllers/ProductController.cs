using Mango.Web.Models;
using Mango.Web.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Mango.Web.Controllers
{
    public class ProductController(IProductService _ProductService) : Controller
    {
        public async Task<IActionResult> Index()
        {
            List<ProductDto> Products = new();

            ResponseDto? response = await _ProductService.GetAllProductsAsync();

            if (response != null && response.IsSuccess)
            {
                Products = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(response.Result));                
            }
            else
            {
                TempData["error"] = response?.Message;
            }

            return View(Products);
        }

        public async Task<IActionResult> ProductCreate()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ProductCreate(ProductDto model)
        {
            if (ModelState.IsValid)
            {
                ResponseDto response = await _ProductService.CreateProductsAsync(model);

                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = $"Product {model.Name} Created";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["error"] = response?.Message;
                }
            }

            return View(model);
        }

        public async Task<IActionResult> ProductDelete(int productId)
        {
            ProductDto Product = new();

            ResponseDto? response = await _ProductService.GetProductByIdAsync(productId);

            if (response != null && response.IsSuccess)
            {
                Product = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result));
            }
            else
            {
                TempData["error"] = response?.Message;
            }

            return View(Product);
        }

        [HttpPost]
        public async Task<IActionResult> ProductDelete(ProductDto model)
        {
            ResponseDto? response = await _ProductService.DeleteProductsAsync(model.ProductId);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Product Deleted";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                TempData["error"] = response?.Message;
            }

            return View(model);
        }

        public async Task<IActionResult> ProductUpdate(int productId)
        {
            ProductDto Product = new();

            ResponseDto? response = await _ProductService.GetProductByIdAsync(productId);

            if (response != null && response.IsSuccess)
            {
                Product = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result));
            }
            else
            {
                TempData["error"] = response?.Message;
            }

            return View(Product);
        }

        [HttpPost]
        public async Task<IActionResult> ProductUpdate(ProductDto model)
        {
            if (ModelState.IsValid)
            {
                ResponseDto response = await _ProductService.UpdateProductsAsync(model);

                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = $"Product {model.Name} Updated";
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
            ProductDto Product = new();

            var response = await _ProductService.GetProductByIdAsync(id);
            
            Product = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(response.Result));

            return PartialView("_ViewProductPartial", Product);
        }
    }
}
