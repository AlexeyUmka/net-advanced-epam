using AutoMapper;
using Carting.BLL.Models;
using Messaging.RabbitMq.Client.Messages;

namespace Carting.BLL;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<BLL.Models.Cart, DAL.Models.Cart>().ReverseMap();
        CreateMap<BLL.Models.CartItem, DAL.Models.CartItem>().ReverseMap();
        CreateMap<BLL.Models.Image, DAL.Models.Image>().ReverseMap();
        CreateMap<ProductUpdatedMessage, BLL.Models.CartItem>().ConstructUsing(message => new CartItem()
        {
            Name = message.Name,
            ExternalId = message.Id,
            Image = new Image() { Url = message.ImageUrl },
            Price = message.Price,
            Quantity = message.Amount
        });
    }
}