using System;
using System.Threading;
using System.Threading.Tasks;
using IWent.Notifications.Email.Configuration;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using MimeKit;
using Polly;

namespace IWent.Notifications.Email;

internal sealed class EmailClient : IEmailClient, IDisposable
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
        var retryPolicy = Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(retryCount: 3, retryAttempt =>
            {
                _logger.LogWarning("An error occured during the email sending. Trying to reconnect.");
                return TimeSpan.FromSeconds(Math.Pow(2, retryAttempt));
            });

        return retryPolicy.ExecuteAsync(async () =>
        {
            if (!_smtpClient.IsConnected)
            {
                _logger.LogInformation("Trying to connect to the mail server.");
                await _smtpClient.ConnectAsync(_clientConfiguration.Host, _clientConfiguration.Port, useSsl: true, cancellationToken);
                _logger.LogInformation("Successfully connected to the mail server.");
            }

            if (!_smtpClient.IsAuthenticated)
            {
                _logger.LogInformation("Trying to authenticate the mail server connection.");
                await _smtpClient.AuthenticateAsync(_clientConfiguration.Username, _clientConfiguration.Password, cancellationToken);
                _logger.LogInformation("Successfully authenticated the mail server connection.");
            }

            await _smtpClient.SendAsync(message, cancellationToken);
        });
    }

    public void Dispose()
    {
        _smtpClient.Dispose();
    }
}
