using AutoMapper;
using Carting.BLL.Services.Implementations;
using Carting.DAL.Repositories.Interfaces;
using FluentValidation;
using Moq;

namespace Carting.UnitTests;

public class CartServiceTests
{
    [Fact]
    public async Task GetCartByExternalIdAsync_ReturnsCart()
    {
        // Arrange
        var cartRepositoryMock = new Mock<ICartRepository>();
        var mapperMock = new Mock<IMapper>();
        var cartItemValidatorMock = new Mock<IValidator<Carting.BLL.Models.CartItem>>();
        var cartService = new CartService(cartRepositoryMock.Object, mapperMock.Object, cartItemValidatorMock.Object);

        // Define test data and expected results
        var cartExternalId = 1;
        var dalCart = new Carting.DAL.Models.Cart();
        var bllCart = new Carting.BLL.Models.Cart();

        cartRepositoryMock.Setup(repo => repo.GetCartByExternalIdAsync(cartExternalId))
            .ReturnsAsync(dalCart);
        mapperMock.Setup(mapper => mapper.Map<Carting.BLL.Models.Cart>(It.IsAny<Carting.DAL.Models.Cart>()))
            .Returns(bllCart);

        // Act
        var result = await cartService.GetCartByExternalIdAsync(cartExternalId);

        // Assert
        Assert.Equal(bllCart, result);
    }
    
    [Fact]
    public async Task AddCartItemAsync_ValidInput_CreatesNewCart()
    {
        // Arrange
        var cartRepositoryMock = new Mock<ICartRepository>();
        var mapperMock = new Mock<IMapper>();
        var cartItemValidatorMock = new Mock<IValidator<Carting.BLL.Models.CartItem>>();
        var cartService = new CartService(cartRepositoryMock.Object, mapperMock.Object, cartItemValidatorMock.Object);

        var cartExternalId = 1;
        var bllCartItem = new Carting.BLL.Models.CartItem();

        cartRepositoryMock.Setup(repo => repo.GetCartByExternalIdAsync(cartExternalId))
            .ReturnsAsync((Carting.DAL.Models.Cart)null);
        mapperMock.Setup(mapper => mapper.Map<Carting.DAL.Models.CartItem>(bllCartItem))
            .Returns(new Carting.DAL.Models.CartItem());

        // Act
        await cartService.AddCartItemAsync(cartExternalId, bllCartItem);

        // Assert
        cartRepositoryMock.Verify(repo => repo.CreateAsync(It.IsAny<Carting.DAL.Models.Cart>()), Times.Once);
    }

    [Fact]
    public async Task AddCartItemAsync_ValidInput_UpdatesExistingCart()
    {
        // Arrange
        var cartRepositoryMock = new Mock<ICartRepository>();
        var mapperMock = new Mock<IMapper>();
        var cartItemValidatorMock = new Mock<IValidator<Carting.BLL.Models.CartItem>>();
        var cartService = new CartService(cartRepositoryMock.Object, mapperMock.Object, cartItemValidatorMock.Object);

        var cartExternalId = 1;
        var bllCartItem = new Carting.BLL.Models.CartItem();
        var existingCart = new Carting.DAL.Models.Cart { ExternalId = cartExternalId, Items = new List<Carting.DAL.Models.CartItem>() };

        cartRepositoryMock.Setup(repo => repo.GetCartByExternalIdAsync(cartExternalId))
            .ReturnsAsync(existingCart);
        mapperMock.Setup(mapper => mapper.Map<Carting.DAL.Models.CartItem>(bllCartItem))
            .Returns(new Carting.DAL.Models.CartItem());

        // Act
        await cartService.AddCartItemAsync(cartExternalId, bllCartItem);

        // Assert
        cartRepositoryMock.Verify(repo => repo.AddCartItemAsync(cartExternalId, It.IsAny<Carting.DAL.Models.CartItem>()), Times.Once);
    }


    [Fact]
    public async Task RemoveCartItemAsync_ValidInput_CallsRemoveCartItemAsync()
    {
        // Arrange
        var cartRepositoryMock = new Mock<ICartRepository>();
        var mapperMock = new Mock<IMapper>();
        var cartItemValidatorMock = new Mock<IValidator<Carting.BLL.Models.CartItem>>();
        var cartService = new CartService(cartRepositoryMock.Object, mapperMock.Object, cartItemValidatorMock.Object);

        var cartExternalId = 1;
        var cartItemExternalId = 2;

        // Act
        await cartService.RemoveCartItemAsync(cartExternalId, cartItemExternalId);

        // Assert
        cartRepositoryMock.Verify(repo => repo.RemoveCartItemAsync(cartExternalId, cartItemExternalId), Times.Once);
    }
}