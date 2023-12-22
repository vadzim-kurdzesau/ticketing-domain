using System;
using System.Runtime.Serialization;

namespace IWent.Services.Exceptions;

[Serializable]
public class CartAlreadyExistsException : ApiException
{
    public CartAlreadyExistsException()
    {
    }

    public CartAlreadyExistsException(string? message)
        : base(message)
    {
    }

    public CartAlreadyExistsException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }

    protected CartAlreadyExistsException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}
