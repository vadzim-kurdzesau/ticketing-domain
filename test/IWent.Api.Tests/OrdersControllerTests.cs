using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using IWent.Api.Tests.Setup;
using IWent.Services.Cart;
using IWent.Services.DTO.Orders;
using IWent.Tests.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Moq.EntityFrameworkCore;
using Xunit;

namespace IWent.Api.Tests;

public class OrdersControllerTests : IClassFixture<EventsWebApplicationFactory>
{
    private readonly EventsWebApplicationFactory _applicationFactory;
    private readonly ApiClient _client;

    public OrdersControllerTests(EventsWebApplicationFactory applicationFactory)
    {
        _applicationFactory = applicationFactory;
        _client = new ApiClient(_applicationFactory.CreateClient());
    }

    [Fact]
    public async Task AddToCart_CreatesNewCartAndAdds()
    {
        // Arrange
        var cartId = Guid.NewGuid().ToString();
        var orderItem = new OrderItem()
        {
            EventId = 1,
            SeatId = 1,
            PriceId = 1,
        };

        // Act
        await _client.AddItemToCartAsync(cartId, orderItem);

        // Assert
        var itemsInTheCart = (await _client.GetItemsInCartAsync(cartId)).ToArray();
        Assert.True(itemsInTheCart.Length == 1);
        itemsInTheCart[0].Should().BeEquivalentTo(orderItem);
    }

    [Fact]
    public async Task AddToCart_CartExists_AddsToExistingOne()
    {
        // Arrange
        var cartId = Guid.NewGuid().ToString();
        var itemsToAdd = new OrderItem[]
        {
            new OrderItem
            {
                EventId = 1,
                SeatId = 1,
                PriceId = 1,
            },
            new OrderItem
            {
                EventId = 1,
                SeatId = 2,
                PriceId = 1,
            }
        };

        await _client.AddItemToCartAsync(cartId, itemsToAdd[0]);

        // Act
        await _client.AddItemToCartAsync(cartId, itemsToAdd[1]);

        // Assert
        var itemsInTheCart = (await _client.GetItemsInCartAsync(cartId)).ToArray();
        Assert.True(itemsInTheCart.Length == 2);
        itemsInTheCart.Should().BeEquivalentTo(itemsToAdd);
    }

    [Fact]
    public async Task BookSeats_CartDoesNotExist_ReturnsError()
    {
        // Arrange
        var cartId = Guid.NewGuid().ToString();

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ApiClientException>(async () => await _client.BookItemsInCartAsync(cartId));
        Assert.Contains(StatusCodes.Status404NotFound.ToString(), exception.Message);
    }

    [Fact]
    public async Task BookSeats_ChangesSeatsStatusToBooked()
    {
        // Arrange
        const int seatsToBook = 2;

        var bookedSeats = TestData.EventSeats.Take(seatsToBook);
        _applicationFactory.ContextMock.Setup(c => c.EventSeats)
            .ReturnsDbSet(bookedSeats);

        var dbMock = new Mock<DatabaseFacade>(_applicationFactory.ContextMock.Object);
        dbMock.Setup(d => d.BeginTransaction())
            .Returns(Mock.Of<IDbContextTransaction>());

        _applicationFactory.ContextMock.Setup(c => c.Database)
            .Returns(dbMock.Object);

        var paymentsDbSetMock = new Mock<DbSet<Persistence.Entities.Payment>>();
        _applicationFactory.ContextMock.Setup(c => c.Payments)
            .Returns(paymentsDbSetMock.Object);

        var cartId = Guid.NewGuid().ToString();

        var cartStorage = _applicationFactory.Services.GetRequiredService<ICartStorage>();
        var cart = cartStorage.GetOrCreate(cartId);
        foreach (var seat in bookedSeats)
        {
            cart.TryAddItem(new CartItem(seat.EventId, seat.SeatId, seat.PriceOptions.First().Id, DateTime.Now));
        }

        // Act
        var paymentInfo = await _client.BookItemsInCartAsync(cartId);

        // Assert
        Assert.NotNull(paymentInfo.PaymentId);

        paymentsDbSetMock.Verify(p => p.Add(It.Is<Persistence.Entities.Payment>(p
            => p.OrderItems.Select(s => s.SeatId).Intersect(bookedSeats.Select(s => s.SeatId)).Count() == seatsToBook)));

        Assert.True(bookedSeats.All(s => s.StateId == Persistence.Entities.SeatStatus.Booked));
    }
}
