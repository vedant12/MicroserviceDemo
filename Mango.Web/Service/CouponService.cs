using Mango.Web.Models;
using Mango.Web.Service.IService;
using Newtonsoft.Json;
using static Mango.Web.Utility.SD;

namespace Mango.Web.Service
{
    public class CouponService<T>(IBaseService<T> baseService, IConfiguration _configuration) : IGenericService<T> where T : class
    {

        public async Task<object> CreateAsync(T model)
        {
            var response = await baseService.SendAsync(new RequestDto<T>
            {
                ApiType = ApiType.POST,
                Data = model,
                Url = _configuration.GetValue<string>("CouponApiUrl")
            });

            return response;
        }

        public async Task<object> DeleteAsync(int id)
        {
            var response = await baseService.SendAsync(new RequestDto<T>
            {
                ApiType = ApiType.DELETE,
                Url = _configuration.GetValue<string>("CouponApiUrl") + $"{id}"
            });

            return response;
        }

        public async Task<object> GetAll()
        {
            var response = await baseService.SendAsync(new RequestDto<T>
            {
                ApiType = ApiType.GET,
                Url = _configuration.GetValue<string>("CouponApiUrl")
            });

            return response;
        }

        public async Task<object> GetByIdAsync(int id)
        {
            var response = await baseService.SendAsync(new RequestDto<T>
            {
                ApiType = ApiType.GET,
                Url = _configuration.GetValue<string>("CouponApiUrl") + $"{id}"
            });

            return response;
        }
        public async Task<object> UpdateAsync(T model)
        {
            var response = await baseService.SendAsync(new RequestDto<T>
            {
                ApiType = ApiType.PUT,
                Data = model,
                Url = _configuration.GetValue<string>("CouponApiUrl")
            });

            return response;
        }

    }
}
