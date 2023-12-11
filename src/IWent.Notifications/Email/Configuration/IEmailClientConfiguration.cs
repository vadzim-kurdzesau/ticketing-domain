namespace IWent.Notifications.Email.Configuration;

public interface IEmailClientConfiguration
{
    string Host { get; init; }

    int Port { get; init; }

    string Username { get; init; }

    string Password { get; init; }

    string SenderName { get; init; }
}
