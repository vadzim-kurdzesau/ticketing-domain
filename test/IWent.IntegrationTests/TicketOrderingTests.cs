using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using IWent.Services.DTO.Orders;
using IWent.Services.DTO.Payments;
using Xunit;

namespace IWent.IntegrationTests;

[Collection("TicketOrdering")] // Prevent tests running in parallel
public class TicketOrderingTests : IClassFixture<IntegrationTestsApplicationFactory>
{
    private readonly IntegrationTestsApplicationFactory _applicationFactory;

    public TicketOrderingTests(IntegrationTestsApplicationFactory applicationFactory)
    {
        _applicationFactory = applicationFactory;
    }

    [Fact]
    public async Task PlaceOrder_SuccessfulPayment()
    {
        // Assert
        var client = new IntegrationTestsClient(_applicationFactory.CreateClient());
        var cartId = Guid.NewGuid().ToString();

        var itemsToOrder = new OrderItem[]
        {
            new OrderItem
            {
                EventId = 1,
                SeatId = 1,
                PriceId = 1
            },
            new OrderItem
            {
                EventId = 1,
                SeatId = 2,
                PriceId = 2
            },
        };

        // Act
        await client.AddItemToCartAsync(cartId, itemsToOrder[0]);
        // Assert
        var itemsInCart = (await client.GetItemsInCartAsync(cartId)).ToArray();
        Assert.True(itemsInCart.Length == 1);
        itemsInCart[0].Should().BeEquivalentTo(itemsToOrder[0]);

        //Act
        await client.AddItemToCartAsync(cartId, itemsToOrder[1]);
        // Assert
        itemsInCart = (await client.GetItemsInCartAsync(cartId)).ToArray();
        Assert.True(itemsInCart.Length == 2);
        itemsInCart.Should().BeEquivalentTo(itemsToOrder);

        // Act
        var paymentInfo = await client.BookItemsInCartAsync(cartId);
        // Assert
        await Assert.ThrowsAnyAsync<Exception>(async () => (await client.GetItemsInCartAsync(cartId)).ToArray());

        // TODO: check items state

        // Act
        await client.CompleteOrderPaymentAsync(paymentInfo.PaymentId);
        var finalPaymentInfo = await client.GetPaymentInfoAsync(paymentInfo.PaymentId);
        Assert.Equal(PaymentStatus.Completed, finalPaymentInfo.Status);
    }

    [Fact]
    public async Task PlaceOrder_FailedPayment()
    {
        // Assert
        var client = new IntegrationTestsClient(_applicationFactory.CreateClient());
        var cartId = Guid.NewGuid().ToString();

        var itemsToOrder = new OrderItem[]
        {
            new OrderItem
            {
                EventId = 1,
                SeatId = 1,
                PriceId = 1
            },
            new OrderItem
            {
                EventId = 1,
                SeatId = 2,
                PriceId = 2
            },
        };

        // Act
        await client.AddItemToCartAsync(cartId, itemsToOrder[0]);
        // Assert
        var itemsInCart = (await client.GetItemsInCartAsync(cartId)).ToArray();
        Assert.True(itemsInCart.Length == 1);
        itemsInCart[0].Should().BeEquivalentTo(itemsToOrder[0]);

        //Act
        await client.AddItemToCartAsync(cartId, itemsToOrder[1]);
        // Assert
        itemsInCart = (await client.GetItemsInCartAsync(cartId)).ToArray();
        Assert.True(itemsInCart.Length == 2);
        itemsInCart.Should().BeEquivalentTo(itemsToOrder);

        // Act
        var paymentInfo = await client.BookItemsInCartAsync(cartId);
        // Assert
        await Assert.ThrowsAnyAsync<Exception>(async () => (await client.GetItemsInCartAsync(cartId)).ToArray());

        // TODO: check items state

        // Act
        await client.FailOrderPaymentAsync(paymentInfo.PaymentId);
        var finalPaymentInfo = await client.GetPaymentInfoAsync(paymentInfo.PaymentId);
        Assert.Equal(PaymentStatus.Failed, finalPaymentInfo.Status);
    }
}