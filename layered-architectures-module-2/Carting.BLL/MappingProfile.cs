using AutoMapper;

namespace Carting.BLL;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<BLL.Models.Cart, DAL.Models.Cart>().ReverseMap();
        CreateMap<BLL.Models.CartItem, DAL.Models.CartItem>().ReverseMap();
        CreateMap<BLL.Models.Image, DAL.Models.Image>().ReverseMap();
    }
}