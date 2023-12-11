using System.Threading;
using System.Threading.Tasks;
using MimeKit;

namespace IWent.Notifications.Email;

internal interface IEmailClient
{
    Task SendEmailAsync(MimeMessage message, CancellationToken cancellationToken);
}
