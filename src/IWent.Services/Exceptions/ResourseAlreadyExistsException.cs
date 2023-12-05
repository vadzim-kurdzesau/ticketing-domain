using System;
using System.Runtime.Serialization;

namespace IWent.Services.Exceptions;

[Serializable]
public class ResourseAlreadyExistsException : ApiException
{
    public ResourseAlreadyExistsException()
    {
    }

    public ResourseAlreadyExistsException(string? message)
        : base(message)
    {
    }

    public ResourseAlreadyExistsException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }

    protected ResourseAlreadyExistsException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}
