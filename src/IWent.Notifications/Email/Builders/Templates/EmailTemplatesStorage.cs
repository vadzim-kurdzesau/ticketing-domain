using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace IWent.Notifications.Email.Builders.Templates;

internal class EmailTemplatesStorage : IEmailTemplatesStorage
{
    private readonly ConcurrentDictionary<string, string> _templates;
    private readonly string _resourceDirectoryPath;

    public EmailTemplatesStorage(string appDirectoryPath)
    {
        if (string.IsNullOrWhiteSpace(appDirectoryPath))
        {
            throw new ArgumentNullException(nameof(appDirectoryPath));
        }

        _resourceDirectoryPath = Path.Combine(appDirectoryPath, "Resources");
        _templates = new ConcurrentDictionary<string, string>();
    }

    public async ValueTask<string> GetTemplateAsync(string templateName, CancellationToken cancellationToken)
    {
        if (_templates.TryGetValue(templateName, out var template))
        {
            return template;
        }

        var path = Path.Combine(_resourceDirectoryPath, templateName);
        template = await File.ReadAllTextAsync(path, cancellationToken);
        _templates.TryAdd(templateName, template);

        return template;
    }
}
