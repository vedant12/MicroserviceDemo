using Mango.Web.Models;

namespace Mango.Web.Service.IService
{
    public interface IGenericService<T> where T : class
    {
        Task<object> GetAll();
        Task<object> GetByIdAsync(int id);
        //Task<ResponseDto<T>> GetCouponByCodeAsync(string code);
        Task<ResponseDto<T>> UpdateAsync(T model);
        Task<ResponseDto<T>> CreateAsync(T model);
        Task<ResponseDto<T>> DeleteAsync(int id);
    }
}
