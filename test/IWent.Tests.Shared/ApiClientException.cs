using System;
using System.Runtime.Serialization;

namespace IWent.Tests.Shared;

[Serializable]
public class ApiClientException : Exception
{
    public ApiClientException()
    {
    }

    public ApiClientException(string? message)
        : base(message)
    {
    }

    public ApiClientException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }

    protected ApiClientException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }
}
