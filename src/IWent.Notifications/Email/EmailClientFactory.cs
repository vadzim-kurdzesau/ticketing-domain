using System.Threading;
using System.Threading.Tasks;
using IWent.Notifications.Email.Configuration;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;

namespace IWent.Notifications.Email;

internal sealed class EmailClientFactory : IEmailClientFactory
{
    private readonly IEmailClientConfiguration _clientConfiguration;
    private readonly ILogger<EmailClientFactory> _logger;
    private readonly ISmtpClient _smtpClient;

    public EmailClientFactory(IEmailClientConfiguration clientConfiguration, ILogger<EmailClientFactory> logger)
    {
        _clientConfiguration = clientConfiguration;
        _logger = logger;
        _smtpClient = new SmtpClient();
    }

    public async ValueTask<ISmtpClient> InitializeAsync(CancellationToken cancellationToken)
    {
        if (_smtpClient.IsAuthenticated)
        {
            _logger.LogDebug("Requested an existing {Client}.", nameof(EmailClientFactory));
            return _smtpClient;
        }

        _logger.LogInformation("Trying to connect to the mail server.");
        await _smtpClient.ConnectAsync(_clientConfiguration.Host, _clientConfiguration.Port, useSsl: true, cancellationToken);
        _logger.LogInformation("Successfully connected to the mail server.");

        _logger.LogInformation("Trying to authenticate the mail server connection.");
        await _smtpClient.AuthenticateAsync(_clientConfiguration.Username, _clientConfiguration.Password, cancellationToken);
        _logger.LogInformation("Successfully authenticated the mail server connection.");

        return _smtpClient;
    }

    public void Dispose()
    {
        _smtpClient.Dispose();
    }
}
