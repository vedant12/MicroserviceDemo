using Mango.Web.Models;

namespace Mango.Web.Service.IService
{
    public interface ICouponService<T> where T : class
    {
        Task<ResponseDto<T>> GetAllCoupons();
        Task<ResponseDto<T>> GetCouponAsync(int id);
        Task<ResponseDto<T>> GetCouponByCodeAsync(string code);
        Task<ResponseDto<T>> UpdateCouponAsync(CouponDto model);
        Task<ResponseDto<T>> CreateCouponAsync(CouponDto model);
        Task<ResponseDto<T>> DeleteCouponAsync(int id);
    }
}
