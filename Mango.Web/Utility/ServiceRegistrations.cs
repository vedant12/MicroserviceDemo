using Mango.Web.Models;
using Mango.Web.Service;
using Mango.Web.Service.IService;

namespace Mango.Web.Utility
{
    public static class ServiceRegistrations
    {
        public static IServiceCollection AddServiceRegistrations(this IServiceCollection services, IConfiguration config)
        {
            services.AddHttpContextAccessor();

            services.AddHttpClient();

            services.AddScoped(typeof(IBaseService<CouponDto>), typeof(BaseService<CouponDto>));

            services.AddScoped(typeof(IGenericService<>), typeof(CouponService<>));

            return services;
        }
    }
}
