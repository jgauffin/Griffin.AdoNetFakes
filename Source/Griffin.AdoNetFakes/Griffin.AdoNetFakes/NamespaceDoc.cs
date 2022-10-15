using System.Runtime.CompilerServices;

namespace Griffin.AdoNetFakes;

/// <summary>
///     Welcome to the ADO.NET fakes.
/// </summary>
/// <remarks>
///     <para>
///         The <c>Factory</c> is used from within all classes of the framework. You can subclass it (and assign your
///         implementation using <c>Factory.Assign()</c>)
///         to get a fine grained control over the framework. But for most if you it's enough to use the <c>NextXxxx</c>
///         methods of each class.
///     </para>
///     Simply create a connection object and then configure it. The <c>FakeConnection.NextResult</c> contains
///     the table which will be retuned as the next query result.
/// </remarks>
/// <example>
///     How to validate an executed command:
///     <code>
/// 
/// </code>
/// </example>
[CompilerGenerated]
internal class NamespaceDoc
{
}
