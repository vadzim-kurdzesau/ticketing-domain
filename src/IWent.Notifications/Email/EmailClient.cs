using System.Threading;
using System.Threading.Tasks;
using IWent.Notifications.Email.Configuration;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MimeKit;

namespace IWent.Notifications.Email;

internal sealed class EmailClient : IHostedService, IEmailClient
{
    private readonly ISmtpClient _smtpClient;
    private readonly IEmailClientConfiguration _clientConfiguration;
    private readonly ILogger<EmailClient> _logger;

    public EmailClient(IEmailClientConfiguration clientConfiguration, ILogger<EmailClient> logger)
    {
        _smtpClient = new SmtpClient();
        _clientConfiguration = clientConfiguration;
        _logger = logger;
    }

    public Task SendEmailAsync(MimeMessage message, CancellationToken cancellationToken)
    {
        return _smtpClient.SendAsync(message, cancellationToken);
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Trying to connect to the mail server.");
        await _smtpClient.ConnectAsync(_clientConfiguration.Host, _clientConfiguration.Port, useSsl: true, cancellationToken);
        _logger.LogInformation("Successfully connected to the mail server.");

        _logger.LogInformation("Trying to authenticate the mail server connection.");
        await _smtpClient.AuthenticateAsync(_clientConfiguration.Username, _clientConfiguration.Password, cancellationToken);
        _logger.LogInformation("Successfully authenticated the mail server connection.");
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return _smtpClient.DisconnectAsync(quit: true, cancellationToken);
    }
}
