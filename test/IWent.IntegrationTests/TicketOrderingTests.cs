using System;
using System.Threading.Tasks;
using IWent.IntegrationTests.Setup;
using IWent.Services.DTO.Orders;
using IWent.Services.DTO.Payments;
using Xunit;

namespace IWent.IntegrationTests;

public class TicketOrderingTests : IClassFixture<IntegrationTestsApplicationFactory>
{
    private readonly IntegrationTestsApplicationFactory _applicationFactory;

    public TicketOrderingTests(IntegrationTestsApplicationFactory applicationFactory)
    {
        _applicationFactory = applicationFactory;
    }

    [Fact]
    public async Task PlaceOrder_SuccessfulPayment_BookedSeatsAreSold()
    {
        // Arrange
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

        await client.AddItemToCartAsync(cartId, itemsToOrder[1]);

        var paymentInfo = await client.BookItemsInCartAsync(cartId);

        await client.CompleteOrderPaymentAsync(paymentInfo.PaymentId);

        // Assert
        var resultPaymentInfo = await client.GetPaymentInfoAsync(paymentInfo.PaymentId);
        Assert.Equal(PaymentStatus.Completed, resultPaymentInfo.Status);

        // TODO: verify seats status
    }

    [Fact]
    public async Task PlaceOrder_FailedPayment()
    {
        // Arrange
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

        await client.AddItemToCartAsync(cartId, itemsToOrder[1]);

        var paymentInfo = await client.BookItemsInCartAsync(cartId);

        await client.FailOrderPaymentAsync(paymentInfo.PaymentId);

        // Assert
        var finalPaymentInfo = await client.GetPaymentInfoAsync(paymentInfo.PaymentId);
        Assert.Equal(PaymentStatus.Failed, finalPaymentInfo.Status);
    }
}