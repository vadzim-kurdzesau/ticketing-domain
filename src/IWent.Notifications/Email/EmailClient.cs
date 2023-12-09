using System.Threading;
using System.Threading.Tasks;
using MimeKit;

namespace IWent.Notifications.Email;

internal sealed class EmailClient : IEmailClient
{
    private readonly IEmailClientFactory _emailClientFactory;

    public EmailClient(IEmailClientFactory emailClientFactory)
    {
        _emailClientFactory = emailClientFactory;
    }

    public async Task SendEmailAsync(MimeMessage message, CancellationToken cancellationToken)
    {
        var client = await _emailClientFactory.InitializeAsync(cancellationToken);
        await client.SendAsync(message, cancellationToken);
    }
}
