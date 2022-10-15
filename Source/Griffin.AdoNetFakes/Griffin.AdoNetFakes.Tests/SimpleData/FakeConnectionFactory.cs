using System.Data;

namespace Griffin.AdoNetFakes.Tests.SimpleData;

public class FakeConnectionFactory : ISimpleConnectionFactory
{
    public FakeConnectionFactory()
    {
        Connection = new FakeConnection();
    }

    public FakeConnection Connection { get; }

    public IDbConnection GetConnection()
    {
        return Connection;
    }
}
