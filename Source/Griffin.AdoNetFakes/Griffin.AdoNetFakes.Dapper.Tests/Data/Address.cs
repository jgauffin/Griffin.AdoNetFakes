namespace Griffin.AdoNetFakes.Dapper.Tests.Data;

public enum AddressType
{
    Home,
    Business
}

public class Address
{
    public int Id { get; set; }

    public AddressType Type { get; set; }

    public string Line1 { get; set; }
    public string Line2 { get; set; }
    public string Line3 { get; set; }
}
