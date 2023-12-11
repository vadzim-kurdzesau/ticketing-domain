using System.Threading;
using System.Threading.Tasks;

namespace IWent.Notifications.Email.Builders.Templates;

internal interface IEmailTemplatesStorage
{
    ValueTask<string> GetTemplateAsync(string templateName, CancellationToken cancellationToken);
}
