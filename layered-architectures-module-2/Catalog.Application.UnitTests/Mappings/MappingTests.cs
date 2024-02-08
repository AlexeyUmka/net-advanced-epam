using System.Runtime.Serialization;
using AutoMapper;
using Catalog.Application.Categories.Queries.GetCategories;
using Catalog.Application.Common.Mappings;
using Catalog.Application.Products.Queries.GetProducts;
using Catalog.Domain.Entities;
using NUnit.Framework;

namespace Catalog.Application.UnitTests.Mappings;

public class MappingTests
{
    private readonly IConfigurationProvider _configuration;
    private readonly IMapper _mapper;

    public MappingTests()
    {
        _configuration = new MapperConfiguration(config => 
            config.AddProfile<MappingProfile>());

        _mapper = _configuration.CreateMapper();
    }

    [Test]
    public void ShouldHaveValidConfiguration()
    {
        _configuration.AssertConfigurationIsValid();
    }

    [Test]
    [TestCase(typeof(Product), typeof(ProductDto))]
    [TestCase(typeof(Category), typeof(CategoryDto))]
    public void ShouldSupportMappingFromSourceToDestination(Type source, Type destination)
    {
        var instance = GetInstanceOf(source);

        Assert.NotNull(_mapper.Map(instance, source, destination));
    }

    private object GetInstanceOf(Type type)
    {
        if (type.GetConstructor(Type.EmptyTypes) != null)
            return Activator.CreateInstance(type)!;

        // Type without parameterless constructor
        return FormatterServices.GetUninitializedObject(type);
    }
}
