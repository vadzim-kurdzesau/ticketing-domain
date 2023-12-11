using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace IWent.Notifications.Email.Builders.Templates;

internal class EmailTemplatesStorage : IEmailTemplatesStorage
{
    private readonly IDictionary<string, string> _templates;

    public EmailTemplatesStorage()
    {
        _templates = new Dictionary<string, string>();
    }

    public async ValueTask<string> GetTemplateAsync(string templateName, CancellationToken cancellationToken)
    {
        if (_templates.TryGetValue(templateName, out var template))
        {
            return template;
        }

        var path = Path.Combine(Constants.ResourceFolderPath, templateName);
        template = await File.ReadAllTextAsync(path, cancellationToken);
        _templates[templateName] = template;

        return template;
    }
}
