using System.Threading;
using System.Threading.Tasks;

namespace IWent.Notifications.Email.Builders.Templates;

internal interface IEmailTemplatesStorage
{
    ValueTask<string> GetTemplateAsync(string appDirectoryPath, string templateName, CancellationToken cancellationToken);
}
