using System.Web;
using System.Xml.Linq;

namespace IWent.Notifications.Email.Builders.HTML;

internal class HtmlEmailBodyElement : IEmailBodyElement
{
    private readonly XElement _element;

    public HtmlEmailBodyElement(string name)
    {
        _element = new XElement(name);
    }

    public HtmlEmailBodyElement(string name, object content)
        : this(name, content.ToString())
    {
    }

    public HtmlEmailBodyElement(string name, string content)
    {
        _element = new XElement(name, content);
    }

    public IEmailBodyElement AddNested(IEmailBodyElement embeddedElement)
    {
        _element.Add(embeddedElement);
        return this;
    }

    public override string ToString()
    {
        return HttpUtility.HtmlDecode(_element.ToString());
    }
}
