using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace IWent.Persistence.Repositories.Exceptions;

[Serializable]
public class EnityDoesNotExistException : Exception
{
    public EnityDoesNotExistException()
    {
    }

    public EnityDoesNotExistException(string? message)
        : base(message)
    {
    }

    public EnityDoesNotExistException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }

    protected EnityDoesNotExistException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}
