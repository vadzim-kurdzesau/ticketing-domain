using System;
using System.Threading;
using System.Threading.Tasks;
using MailKit.Net.Smtp;

namespace IWent.Notifications.Email;

internal interface IEmailClientFactory : IDisposable
{
    ValueTask<ISmtpClient> InitializeAsync(CancellationToken cancellationToken);
}
