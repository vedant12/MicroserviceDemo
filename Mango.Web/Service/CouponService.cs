using Mango.Web.Models;
using Mango.Web.Service.IService;
using static Mango.Web.Utility.SD;

namespace Mango.Web.Service
{
    public class CouponService<T>(IBaseService<T> baseService, IConfiguration _configuration) : IGenericService<T> where T : class
    {

        public Task<ResponseDto<T>> CreateAsync(T model)
        {
            throw new NotImplementedException();
        }

        public Task<ResponseDto<T>> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<object> GetAll()
        {
            var response = baseService.SendAsync(new RequestDto<T>
            {
                ApiType = ApiType.GET,
                Url = _configuration.GetValue<string>("CouponApiUrl")
            });

            return response;
        }

        public Task<object> GetByIdAsync(int id)
        {
            var response = baseService.SendAsync(new RequestDto<T>
            {
                ApiType = ApiType.GET,
                Url = _configuration.GetValue<string>("CouponApiUrl") + $"{id}"
            });

            return response;
        }
        public Task<ResponseDto<T>> UpdateAsync(T model)
        {
            throw new NotImplementedException();
        }

    }
}
