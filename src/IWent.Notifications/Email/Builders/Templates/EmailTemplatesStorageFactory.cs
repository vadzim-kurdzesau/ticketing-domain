using Microsoft.Azure.WebJobs;

namespace IWent.Notifications.Email.Builders.Templates;

internal class EmailTemplatesStorageFactory : IEmailTemplatesStorageFactory
{
    private readonly object _storageLock = new();
    private IEmailTemplatesStorage? _storage;

    public IEmailTemplatesStorage Create(ExecutionContext executionContext)
    {
        if (_storage == null)
        {
            lock (_storageLock)
            {
                if (_storage == null)
                {
                    _storage = new EmailTemplatesStorage(executionContext.FunctionAppDirectory);
                }
            }
        }

        return _storage;
    }
}
