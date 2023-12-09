using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Xsl;

namespace IWent.Notifications.Email.Builders.HTML;

internal class HtmlEmailBodyBuilder : IEmailBodyBuilder
{
    private readonly IList<IEmailBodyElement> _bodyElements;
    private readonly XslCompiledTransform _xslt;

    public HtmlEmailBodyBuilder(string template)
    {
        _bodyElements = new List<IEmailBodyElement>();
        _xslt = new XslCompiledTransform();

        using var xmlReader = GetXmlReader(template);
        _xslt.Load(xmlReader);
    }

    public IEmailBodyBuilder AddElement(IEmailBodyElement element)
    {
        _bodyElements.Add(element);
        return this;
    }

    public string Build()
    {
        var writer = new StringWriter();
        using (var xmlWriter = XmlWriter.Create(writer, _xslt.OutputSettings))
        {
            foreach (var element in _bodyElements)
            {
                using var elementReader = GetXmlReader(element.ToString());
                _xslt.Transform(elementReader, xmlWriter);
            }
        }

        return writer.ToString();
    }

    private static XmlReader GetXmlReader(string value)
    {
        var stringReader = new StringReader(value);
        return XmlReader.Create(stringReader);
    }
}
