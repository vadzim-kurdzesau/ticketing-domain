namespace IWent.Notifications.Email.Configuration;

public class EmailClientConfiguration : IEmailClientConfiguration
{
    public string Host { get; init; } = null!;

    public int Port { get; init; }

    public string Username { get; init; } = null!;

    public string Password { get; init; } = null!;

    public string SenderName { get; init; } = null!;
}
