using System;
using System.Collections.Generic;
using IWent.Persistence.Entities;

namespace IWent.Api.Tests.Setup;

internal static class TestData
{
    public static IEnumerable<Event> Events => new Event[]
    {
        new Event
        {
            Id = 1,
            Name = "International Football Championship",
            Date = new DateTime(2023, 6, 15, 18, 0, 0, DateTimeKind.Utc),
            VenueId = 1,
            Venue = new Venue
            {
                Id = 1,
                Name = "Olympic Stadium",
                Country = "Country A",
                Region = "Region A",
                City = "City A",
                Street = "123 Sports Ave",
            },
        },
        new Event
        {
            Id = 2,
            Name = "World Music Concert",
            Date = new DateTime(2023, 7, 20, 20, 0, 0, DateTimeKind.Utc),
            VenueId = 2,
            Venue = new Venue
            {
                Id = 2,
                Name = "Grand Arena",
                Country = "Country B",
                Region = "Region B",
                City = "City B",
                Street = "456 Concert Blvd",
            },
        },
        new Event
        {
            Id = 3,
            Name = "Annual Tech Expo",
            Date = new DateTime(2023, 8, 10, 9, 0, 0, DateTimeKind.Utc),
            VenueId = 3,
            Venue = new Venue
            {
                Id = 3,
                Name = "Convention Center",
                Country = "Country C",
                Region = "Region C",
                City = "City C",
                Street = "789 Innovation Rd",
            },
        },
        new Event
        {
            Id = 4,
            Name = "Marathon City Run",
            Date = new DateTime(2023, 9, 5, 7, 0, 0, DateTimeKind.Utc),
            VenueId = 4,
            Venue = new Venue
            {
                Id = 4,
                Name = "City Park",
                Country = "Country D",
                Region = "Region D",
                City = "City D",
                Street = "1010 Park Way",
            },
        },
        new Event
        {
            Id = 5,
            Name = "International Chess Tournament",
            Date = new DateTime(2023, 10, 15, 10, 0, 0, DateTimeKind.Utc),
            VenueId = 5,
            Venue = new Venue
            {
                Id = 5,
                Name = "Chess Hall",
                Country = "Country E",
                Region = "Region E",
                City = "City E",
                Street = "1111 Strategy Ave",
            },
        }
    };

    public static IEnumerable<EventSeat> EventSeats => new EventSeat[]
    {
        new EventSeat
        {
            EventId = 1,
            SeatId = 1,
            Seat = new Seat
            {
                Id = 1,
                Number = 1,
                RowId = 1,
                Row = new Row
                {
                    Id = 1,
                    Number = 1,
                    SectionId = 1,
                }
            },
            StateId = SeatStatus.Available,
            PriceOptions = new Price[]
            {
                new Price { Id = 1, Name = "Adult" }
            },
            State = new SeatState
            {
                Id = SeatStatus.Available,
                Name = "Available"
            },
        },
        new EventSeat
        {
            EventId = 1,
            SeatId = 2,
            Seat = new Seat
            {
                Id = 2,
                Number = 2,
                RowId = 1,
                Row = new Row
                {
                    Id = 1,
                    Number = 1,
                    SectionId = 1,
                }
            },
            StateId = SeatStatus.Available,
            PriceOptions = new Price[]
            {
                new Price { Id = 1, Name = "Adult" }
            },
            State = new SeatState
            {
                Id = SeatStatus.Available,
                Name = "Available"
            },
        },

        new EventSeat
        {
            EventId = 2,
            SeatId = 1,
            Seat = new Seat
            {
                Id = 1,
                Number = 1,
                RowId = 1,
                Row = new Row
                {
                    Id = 1,
                    Number = 1,
                    SectionId = 1,
                }
            },
            StateId = SeatStatus.Available,
            PriceOptions = new Price[]
            {
                new Price { Id = 1, Name = "Adult" }
            },
            State = new SeatState
            {
                Id = SeatStatus.Available,
                Name = "Available"
            },
        },

        new EventSeat
        {
            EventId = 2,
            SeatId = 2,
            Seat = new Seat
            {
                Id = 2,
                Number = 2,
                RowId = 2,
                Row = new Row
                {
                    Id = 2,
                    Number = 2,
                    SectionId = 1,
                }
            },
            StateId = SeatStatus.Available,
            PriceOptions = new Price[]
            {
                new Price { Id = 1, Name = "Adult" }
            },
            State = new SeatState
            {
                Id = SeatStatus.Available,
                Name = "Available"
            },
        },
    };
}
