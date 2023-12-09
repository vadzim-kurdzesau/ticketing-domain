using MimeKit;

namespace IWent.Notifications.Email.Builders.HTML;

internal class HtmlEmailMessageBuilder : IEmailMessageBuilder
{
    private readonly MimeMessage _message;
    private readonly BodyBuilder _bodyBuilder;

    public HtmlEmailMessageBuilder()
    {
        _message = new MimeMessage();
        _bodyBuilder = new BodyBuilder();
    }

    public IEmailMessageBuilder AddReceiver(string mail, string name)
    {
        _message.To.Add(new MailboxAddress(name, mail));
        return this;
    }

    public IEmailMessageBuilder AddSender(string mail, string name)
    {
        _message.From.Add(new MailboxAddress(name, mail));
        return this;
    }

    public IEmailMessageBuilder SetSubject(string subject)
    {
        _message.Subject = subject;
        return this;
    }

    public IEmailMessageBuilder SetBody(string body)
    {
        _bodyBuilder.HtmlBody = body;
        return this;
    }

    public MimeMessage Create()
    {
        _message.Body = _bodyBuilder.ToMessageBody();
        return _message;
    }
}
