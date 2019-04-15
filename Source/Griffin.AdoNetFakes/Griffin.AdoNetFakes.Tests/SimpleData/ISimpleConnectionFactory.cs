using System.Data;

namespace Griffin.AdoNetFakes.Tests.SimpleData
{
    public interface ISimpleConnectionFactory
    {
        IDbConnection GetConnection();
    }
}
