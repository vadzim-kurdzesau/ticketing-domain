using Microsoft.Azure.WebJobs;

namespace IWent.Notifications.Email.Builders.Templates;

internal interface IEmailTemplatesStorageFactory
{
    IEmailTemplatesStorage Create(ExecutionContext executionContext);
}
