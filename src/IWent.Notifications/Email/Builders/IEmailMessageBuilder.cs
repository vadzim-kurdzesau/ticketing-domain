using MimeKit;

namespace IWent.Notifications.Email.Builders;

internal interface IEmailMessageBuilder
{
    IEmailMessageBuilder AddSender(string mail, string name);

    IEmailMessageBuilder AddReceiver(string mail, string name);

    IEmailMessageBuilder SetSubject(string subject);

    IEmailMessageBuilder SetBody(string body);

    MimeMessage Create();
}
