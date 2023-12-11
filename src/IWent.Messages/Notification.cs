using System;
using System.Collections.Generic;
using IWent.Messages.Models;

namespace IWent.Messages
{
    public class Notification : INotification
    {
        public Guid Id { get; set; }

        public Operation Operation { get; set; }

        public DateTime Timestamp { get; set; }

        public IReadOnlyDictionary<string, string> Parameters { get; set; }

        public object Content { get; set; }
    }
}
