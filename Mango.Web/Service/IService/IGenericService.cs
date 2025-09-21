using Mango.Web.Models;

namespace Mango.Web.Service.IService
{
    public interface IGenericService<T> where T : class
    {
        Task<object> GetAll();
        Task<object> GetByIdAsync(int id);
        //Task<ResponseDto<T>> GetCouponByCodeAsync(string code);
        Task<object> UpdateAsync(T model);
        Task<object> CreateAsync(T model);
        Task<object> DeleteAsync(int id);
    }
}
