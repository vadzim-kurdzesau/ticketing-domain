namespace IWent.Notifications.Email.Builders;

internal interface IEmailBodyElement
{
    IEmailBodyElement AddNested(IEmailBodyElement embeddedElement);
}
