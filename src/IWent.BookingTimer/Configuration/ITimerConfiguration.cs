using System;

namespace IWent.BookingTimer.Configuration;

public interface ITimerConfiguration
{
    TimeSpan Expiration { get; init; }
}