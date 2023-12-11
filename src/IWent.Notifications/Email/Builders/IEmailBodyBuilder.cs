namespace IWent.Notifications.Email.Builders;

internal interface IEmailBodyBuilder
{
    IEmailBodyBuilder AddElement(IEmailBodyElement element);

    string Build();
}
