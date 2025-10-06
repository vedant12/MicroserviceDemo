using AutoMapper;
using Mango.Services.ShoppingCartAPI.Models;
using Mango.Services.ShoppingCartAPI.Models.Dtos;

namespace Mango.Services.ShoppingCartAPI.Automapper
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<CartDetail, CartDetailsDto>()
                .ReverseMap();

            CreateMap<CartHeader, CartHeaderDto>()
               .ReverseMap();
        }
    }
}
