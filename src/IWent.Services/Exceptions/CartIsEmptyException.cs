using System;
using System.Runtime.Serialization;

namespace IWent.Services.Exceptions;

[Serializable]
public class CartIsEmptyException : ApiException
{
    public CartIsEmptyException()
    {
    }

    public CartIsEmptyException(string? message)
        : base(message)
    {
    }

    public CartIsEmptyException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }

    protected CartIsEmptyException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}
