namespace IWent.BookingTimer.Handling.Timers;

public interface ITimerFactory
{
    BookingTimer Create(string bookingId);
}