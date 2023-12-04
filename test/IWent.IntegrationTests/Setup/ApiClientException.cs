using System;
using System.Runtime.Serialization;

namespace IWent.IntegrationTests.Setup;

[Serializable]
internal sealed class ApiClientException : Exception
{
    public ApiClientException()
    {
    }

    public ApiClientException(string? message) : base(message)
    {
    }

    public ApiClientException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public ApiClientException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}
