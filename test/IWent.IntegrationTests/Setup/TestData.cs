using System;
using System.Collections.Generic;
using IWent.Persistence.Entities;

namespace IWent.IntegrationTests.Setup;

internal static class TestData
{
    public static Event Event =>
        new Event
        {
            Name = "Test Marafon",
            Date = new DateTime(2024, 12, 12, 0, 0, 0, DateTimeKind.Utc),
            Venue = new Venue
            {
                Name = "Test Stadium",
                Country = "Test Country",
                Region = "Test Region",
                City = "Test City",
                Street = "Test Street",
                Sections = new Section[]
                {
                    new Section
                    {
                        Name = "Section A",
                        SeatType = SeatType.Designated,
                        Rows = new Row[]
                        {
                            new Row
                            {
                                Number = 1,
                                Seats = new Seat[]
                                {
                                    new Seat { Number = 1 },
                                    new Seat { Number = 2 },
                                    new Seat { Number = 3 },
                                    new Seat { Number = 4 },
                                    new Seat { Number = 5 },
                                },
                            },
                            new Row
                            {
                                Number = 2,
                                Seats = new Seat[]
                                {
                                    new Seat { Number = 6 },
                                    new Seat { Number = 7 },
                                    new Seat { Number = 8 },
                                    new Seat { Number = 9 },
                                    new Seat { Number = 10 },
                                },
                            },
                        }
                    }
                }
            }
        };

    public static IEnumerable<Price> PriceOptions => new Price[]
    {
        new Price { Name = "Adult", Amount = 100.0M },
        new Price { Name = "Child", Amount = 75.0M },
        new Price { Name = "VIP", Amount = 150.0M },
    };
}
