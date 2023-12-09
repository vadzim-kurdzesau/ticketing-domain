using System;

namespace IWent.Messages
{
    public interface IMessage
    {
        /// <summary>
        /// The unique identifier of this message which can be used for its tracking.
        /// </summary>
        Guid Id { get; set; }

        /// <summary>
        /// The date and time when this message was sent.
        /// </summary>
        DateTime Timestamp { get; set; }
    }
}
