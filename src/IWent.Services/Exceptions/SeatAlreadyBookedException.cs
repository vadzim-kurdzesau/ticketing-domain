using System;
using System.Runtime.Serialization;

namespace IWent.Services.Exceptions;

[Serializable]
public class SeatAlreadyBookedException : ApiException
{
    public SeatAlreadyBookedException()
    {
    }

    public SeatAlreadyBookedException(string? message)
        : base(message)
    {
    }

    public SeatAlreadyBookedException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }

    protected SeatAlreadyBookedException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}
