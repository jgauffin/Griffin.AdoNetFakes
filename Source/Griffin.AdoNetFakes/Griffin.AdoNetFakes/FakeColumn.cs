using System;

namespace Griffin.AdoNetFakes;

/// <summary>
///     Column in a <see cref="FakeDataRecord" />
/// </summary>
public class FakeColumn
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="FakeColumn" /> class.
    /// </summary>
    /// <param name="name">column name.</param>
    /// <param name="value">Data value.</param>
    public FakeColumn(string name, object value)
    {
        ColumnName = name;
        Value = value;
    }

    /// <summary>
    ///     Gets column name
    /// </summary>
    public string ColumnName { get; set; }

    /// <summary>
    ///     Gets value
    /// </summary>
    public object Value { get; set; }

    /// <summary>
    ///     Gets or sets column type, must be specified if value is null.
    /// </summary>
    public Type ColumnType { get; set; }
}
