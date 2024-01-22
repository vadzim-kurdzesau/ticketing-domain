using System;

namespace IWent.BookingTimer.Messages
{
    public class BookingTimerMessage
    {
        /// <summary>
        /// 
        /// </summary>
        public string BookingNumber { get; set; }

        /// <summary>
        /// Defines the action need to be with the timer for this booking.
        /// </summary>
        public TimerAction Action { get; set; }
    }
}
