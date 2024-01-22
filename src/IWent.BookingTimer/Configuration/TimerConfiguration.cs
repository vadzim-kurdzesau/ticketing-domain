using System;

namespace IWent.BookingTimer.Configuration;

public class TimerConfiguration : ITimerConfiguration
{
    public TimeSpan Expiration { get; init; }
}
