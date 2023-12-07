using System.Net.Mail;

namespace IWent.Notifications.Email;

public interface IEmailClient
{
    Task SendEmailAsync(MailMessage message, CancellationToken cancellationToken);
}
