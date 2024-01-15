using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace IWent.Notifications.Email.Builders.Templates;

internal class EmailTemplatesStorage : IEmailTemplatesStorage
{
    private readonly ConcurrentDictionary<string, string> _templates;

    public EmailTemplatesStorage()
    {
        _templates = new ConcurrentDictionary<string, string>();
    }

    public async ValueTask<string> GetTemplateAsync(string appDirectoryPath, string templateName, CancellationToken cancellationToken)
    {
        if (_templates.TryGetValue(templateName, out var template))
        {
            return template;
        }

        var path = Path.Combine(appDirectoryPath, "Resources", templateName);
        template = await File.ReadAllTextAsync(path, cancellationToken);
        _templates.TryAdd(templateName, template);

        return template;
    }
}
