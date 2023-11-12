using AutoMapper;
using Catalog.Domain.Entities;
using Messaging.RabbitMq.Client.Messages;

namespace Catalog.Domain;

public class DomainMappingProfile : Profile
{
    public DomainMappingProfile()
    {
        CreateMap<Product, ProductUpdatedMessage>().ConstructUsing(x => new ProductUpdatedMessage()
        {
            Id = x.Id,
            Name = x.Name,
            ImageUrl = x.ImageUrl,
            Price = x.Price,
            Amount = x.Amount
        });
    }
}