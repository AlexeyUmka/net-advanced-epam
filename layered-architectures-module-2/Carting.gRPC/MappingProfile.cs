using AutoMapper;

namespace Carting.gRPC;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<BLL.Models.Cart, CartResponse>();
        CreateMap<BLL.Models.CartItem, CartItemResponse>();
        CreateMap<BLL.Models.Image, ImageResponse>();
    }
}