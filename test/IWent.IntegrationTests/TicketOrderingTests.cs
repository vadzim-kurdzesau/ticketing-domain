using System;
using System.Linq;
using System.Threading.Tasks;
using IWent.IntegrationTests.Setup;
using IWent.Services.DTO.Events;
using IWent.Services.DTO.Orders;
using IWent.Services.DTO.Payments;
using IWent.Tests.Shared;
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
        var client = new ApiClient(_applicationFactory.CreateClient());

        // Get seats to book
        var eventToAttend = (await client.GetEventsAsync()).First();
        var eventId = eventToAttend.Id;

        var availableSections = await client.GetSectionsAsync(eventToAttend.VenueId);
        var sectionId = availableSections.First().Id;

        var seatsToBook = (await client.GetSectionSeatsAsync(eventId, sectionId))
            .Take(2).ToArray();

        // Book the seats
        var cartId = Guid.NewGuid().ToString();
        var itemsToOrder = new OrderItem[]
        {
            new OrderItem
            {
                EventId = eventId,
                SeatId = seatsToBook[0].SeatId,
                PriceId = seatsToBook[0].PriceOptions.First().Id,
            },
            new OrderItem
            {
                EventId = eventId,
                SeatId = seatsToBook[1].SeatId,
                PriceId = seatsToBook[1].PriceOptions.First().Id,
            },
        };

        await client.AddItemToCartAsync(cartId, itemsToOrder[0]);

        await client.AddItemToCartAsync(cartId, itemsToOrder[1]);

        var paymentInfo = await client.BookItemsInCartAsync(cartId);

        await client.CompleteOrderPaymentAsync(paymentInfo.PaymentId);

        // Assert
        var resultPaymentInfo = await client.GetPaymentInfoAsync(paymentInfo.PaymentId);
        Assert.Equal(PaymentStatus.Completed, resultPaymentInfo.Status);

        var bookedSeats = (await client.GetSectionSeatsAsync(eventId, sectionId))
            .Where(s => seatsToBook.Select(s => s.SeatId).Contains(s.SeatId))
            .ToArray();

        Assert.True(Array.TrueForAll(bookedSeats, s => s.StateId == SeatState.Sold));
    }

    [Fact]
    public async Task PlaceOrder_FailedPayment_BookedSeatsReturnToAvailable()
    {
        var client = new ApiClient(_applicationFactory.CreateClient());

        // Get seats to book
        var eventToAttend = (await client.GetEventsAsync()).First();
        var eventId = eventToAttend.Id;

        var availableSections = await client.GetSectionsAsync(eventToAttend.VenueId);
        var sectionId = availableSections.First().Id;

        var seatsToBook = (await client.GetSectionSeatsAsync(eventId, sectionId))
            .Skip(2).Take(2).ToArray();

        // Book the seats
        var cartId = Guid.NewGuid().ToString();
        var itemsToOrder = new OrderItem[]
        {
            new OrderItem
            {
                EventId = eventId,
                SeatId = seatsToBook[0].SeatId,
                PriceId = seatsToBook[0].PriceOptions.First().Id,
            },
            new OrderItem
            {
                EventId = eventId,
                SeatId = seatsToBook[1].SeatId,
                PriceId = seatsToBook[1].PriceOptions.First().Id,
            },
        };

        await client.AddItemToCartAsync(cartId, itemsToOrder[0]);

        await client.AddItemToCartAsync(cartId, itemsToOrder[1]);

        var paymentInfo = await client.BookItemsInCartAsync(cartId);

        await client.FailOrderPaymentAsync(paymentInfo.PaymentId);

        // Assert
        var resultPaymentInfo = await client.GetPaymentInfoAsync(paymentInfo.PaymentId);
        Assert.Equal(PaymentStatus.Failed, resultPaymentInfo.Status);

        var bookedSeats = (await client.GetSectionSeatsAsync(eventId, sectionId))
            .Where(s => seatsToBook.Select(s => s.SeatId).Contains(s.SeatId))
            .ToArray();

        Assert.True(Array.TrueForAll(bookedSeats, s => s.StateId == SeatState.Available));
    }
}