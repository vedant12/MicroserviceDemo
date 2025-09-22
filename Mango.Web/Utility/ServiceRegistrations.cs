using Mango.Web.Models;
using Mango.Web.Service;
using Mango.Web.Service.IService;

namespace Mango.Web.Utility
{
    public static class ServiceRegistrations
    {
        public static IServiceCollection AddServiceRegistrations(this IServiceCollection services, IConfiguration configuration)
        {
            SD.CouponAPIBase = configuration["ServiceUrls:CouponAPI"];

            services.AddHttpContextAccessor();
            services.AddHttpClient();
            services.AddHttpClient<ICouponService, CouponService>();

            services.AddScoped<IBaseService, BaseService>();
            services.AddScoped<ICouponService, CouponService>();

            return services;
        }
    }
}
