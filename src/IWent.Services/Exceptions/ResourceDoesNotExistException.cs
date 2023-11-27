using System;
using System.Runtime.Serialization;

namespace IWent.Services.Exceptions;

[Serializable]
public class ResourceDoesNotExistException : ApiException
{
    public ResourceDoesNotExistException()
    {
    }

    public ResourceDoesNotExistException(string? message)
        : base(message)
    {
    }

    public ResourceDoesNotExistException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }

    protected ResourceDoesNotExistException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}
