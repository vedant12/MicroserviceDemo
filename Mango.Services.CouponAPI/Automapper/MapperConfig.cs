using AutoMapper;
using Mango.Services.CouponAPI.Dtos;
using Mango.Services.CouponAPI.Models;

namespace Mango.Services.CouponAPI.Automapper
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<Coupon, CouponDto>()
                .ReverseMap();
        }
    }
}
