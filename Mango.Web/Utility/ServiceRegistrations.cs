using Mango.Web.Models;
using Mango.Web.Service;
using Mango.Web.Service.IService;

namespace Mango.Web.Utility
{
    public static class ServiceRegistrations
    {
        public static IServiceCollection AddServiceRegistrations(this IServiceCollection services, IConfiguration configuration)
        {
            SD.CouponAPIBase = configuration["ServiceUrls:CouponAPI"]!;
            SD.AuthAPIBase = configuration["ServiceUrls:AuthAPI"]!;

            services.AddHttpContextAccessor();

            services.AddHttpClient();
            services.AddHttpClient<ICouponService, CouponService>();
            services.AddHttpClient<IAuthService, AuthService>();

            services.AddScoped<IBaseService, BaseService>();
            services.AddScoped<ICouponService, CouponService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ITokenProvider, TokenProvider>();

            return services;
        }
    }
}
