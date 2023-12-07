namespace IWent.Notifications.Email;

public interface IEmailClientConfiguration
{
    string Host { get; init; }

    int Port { get; init; }

    string Username { get; init; }

    string Password { get; init; }
}
