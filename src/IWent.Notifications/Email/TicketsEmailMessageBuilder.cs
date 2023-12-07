using System.Net.Mail;
using System.Text;
using IWent.Messages.Models;

namespace IWent.Notifications.Email;

public class TicketsEmailMessageBuilder
{
    private readonly MailMessage _mailMessage;
    private readonly StringBuilder _body;

    public TicketsEmailMessageBuilder()
    {
        _mailMessage = new MailMessage("", "ticketstestformeonly@mailinator.com");
        _body = new StringBuilder();
    }

    public void AddTicket(Ticket ticket)
    {
        _body.AppendLine("====================================================");
        _body.AppendLine($"\"{ticket.EventName}\"");
        _body.AppendLine($"When: {ticket.Date:G}");
        _body.AppendLine($"Where: {ticket.Address.Street}\n{ticket.Address.City} {ticket.Address.Region}, {ticket.Address.Country}");
        _body.AppendLine($"Section: {ticket.SectionName}\nRow: {ticket.RowNumber}\tSeat: {ticket.Number}");
    }

    public MailMessage Create()
    {
        _mailMessage.Subject = "Tickets";
        _mailMessage.Body = _body.ToString();
        return _mailMessage;
    }
}
