using System;
using System.Runtime.Serialization;

namespace IWent.Services.Exceptions;

/// <summary>
/// Base exception for all errors that can be returned in a response.
/// </summary>
[Serializable]
public class ApiException : Exception
{
    public ApiException()
    {
    }

    public ApiException(string? message)
        : base(message)
    {
    }

    public ApiException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }

    protected ApiException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}
