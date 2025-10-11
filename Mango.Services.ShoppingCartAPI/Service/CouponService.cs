using Mango.Services.ShoppingCartAPI.Dtos;
using Mango.Services.ShoppingCartAPI.Service.IService;
using Newtonsoft.Json;

namespace Mango.Services.ShoppingCartAPI.Service
{
    public class CouponService : ICouponService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CouponService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<CouponDto> GetCoupon(string couponCode)
        {
            var client = _httpClientFactory.CreateClient("Coupon");

            var response = await client.GetAsync($"/api/coupon/GetByCode/{couponCode}");

            var apiContent = await response.Content.ReadAsStringAsync();

            var coupon = JsonConvert.DeserializeObject<ResponseDto>(apiContent);

            if (coupon.IsSuccess)
                return JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(coupon.Result));
            else
                return new CouponDto();
        }
    }
}
