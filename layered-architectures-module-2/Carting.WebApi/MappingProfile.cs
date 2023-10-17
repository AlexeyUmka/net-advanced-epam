using AutoMapper;

namespace Carting.WebApi;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<BLL.Models.Cart, WebApi.Models.Cart>().ReverseMap();
        CreateMap<BLL.Models.CartItem, WebApi.Models.CartItem>().ReverseMap();
        CreateMap<BLL.Models.Image, WebApi.Models.Image>().ReverseMap();
    }
}