using System.Data;

namespace IWent.Persistence.Repositories.ConnectionProviders;

public interface IDbConnectionProvider
{
    IDbConnection InitializeConnection();
}
