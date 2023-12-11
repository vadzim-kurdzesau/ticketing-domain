using System.Collections.Generic;
using IWent.Messages.Models;

namespace IWent.Messages
{
    public interface INotification : IMessage
    {
        /// <summary>
        /// The name of the operation this message contains.
        /// </summary>
        Operation Operation { get; set; }

        /// <summary>
        /// The parameters of this notifications, such as email or a customer name.
        /// </summary>
        IReadOnlyDictionary<string, string> Parameters { get; set; }

        /// <summary>
        /// The content of the notification.
        /// </summary>
        object Content { get; set; } // TODO: consider a better solution than a System.Object type
    }
}
