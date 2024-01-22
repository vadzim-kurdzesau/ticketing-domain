using System;

namespace IWent.BookingTimer.Handling;

public class TimerExpiredEventArgs : EventArgs
{
    public string BookingId { get; init; } = null!;
}
