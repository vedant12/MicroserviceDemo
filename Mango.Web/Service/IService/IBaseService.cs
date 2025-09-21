using Mango.Web.Models;

namespace Mango.Web.Service.IService
{
    public interface IBaseService<T> where T : class
    {
        Task<object> SendAsync(RequestDto<T> model);
    }
}
