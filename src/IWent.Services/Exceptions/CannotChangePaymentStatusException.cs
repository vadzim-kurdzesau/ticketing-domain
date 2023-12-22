using System;
using System.Runtime.Serialization;

namespace IWent.Services.Exceptions;

[Serializable]
public class CannotChangePaymentStatusException : ApiException
{
    public CannotChangePaymentStatusException()
    {
    }

    public CannotChangePaymentStatusException(string? message)
        : base(message)
    {
    }

    public CannotChangePaymentStatusException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }

    protected CannotChangePaymentStatusException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}
