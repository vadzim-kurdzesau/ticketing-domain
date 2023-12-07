using System.Net;
using System.Net.Mail;

namespace IWent.Notifications.Email;

public sealed class EmailClient : IEmailClient, IDisposable
{
    private readonly SmtpClient _smtpClient;

    public EmailClient(IEmailClientConfiguration clientConfiguration)
    {
        _smtpClient = new SmtpClient(clientConfiguration.Host, clientConfiguration.Port)
        {
            Credentials = new NetworkCredential(clientConfiguration.Username, clientConfiguration.Password),
            EnableSsl = true,
        };
    }

    public Task SendEmailAsync(MailMessage message, CancellationToken cancellationToken)
    {
        return _smtpClient.SendMailAsync(message, cancellationToken);
    }

    public void Dispose()
    {
        _smtpClient.Dispose();
    }
}
