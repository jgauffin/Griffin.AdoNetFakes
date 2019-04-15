using System.Data;
using System.Data.Common;

namespace Griffin.AdoNetFakes
{
    /// <summary>
    /// Fake data parameter
    /// </summary>
    public class FakeParameter : DbParameter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FakeParameter"/> class.
        /// </summary>
        public FakeParameter()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FakeParameter"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        public FakeParameter(string name, object value)
        {
            ParameterName = name;
            Value = value;
        }
        /// <summary>
        /// Gets or sets the <see cref="T:System.Data.DbType"/> of the parameter.
        /// </summary>
        /// <returns>One of the <see cref="T:System.Data.DbType"/> values. The default is <see cref="F:System.Data.DbType.String"/>.</returns>
        ///
        /// <exception cref="T:System.ArgumentException">The property is not set to a valid <see cref="T:System.Data.DbType"/>.</exception>
        public override DbType DbType { get; set; }

        /// <summary>
        /// Gets or sets a value that indicates whether the parameter is input-only, output-only, bidirectional, or a stored procedure return value parameter.
        /// </summary>
        /// <returns>One of the <see cref="T:System.Data.ParameterDirection"/> values. The default is Input.</returns>
        ///
        /// <exception cref="T:System.ArgumentException">The property is not set to one of the valid <see cref="T:System.Data.ParameterDirection"/> values.</exception>
        public override ParameterDirection Direction { get; set; }

        /// <summary>
        /// Gets or sets a value that indicates whether the parameter accepts null values.
        /// </summary>
        /// <returns>true if null values are accepted; otherwise false. The default is false.</returns>
        public override bool IsNullable { get; set; }

        /// <summary>
        /// Gets or sets the name of the <see cref="T:System.Data.Common.DbParameter"/>.
        /// </summary>
        /// <returns>The name of the <see cref="T:System.Data.Common.DbParameter"/>. The default is an empty string ("").</returns>
        public override string ParameterName { get; set; }

        /// <summary>
        /// Gets or sets the name of the source column mapped to the <see cref="T:System.Data.DataSet"/> and used for loading or returning the <see cref="P:System.Data.Common.DbParameter.Value"/>.
        /// </summary>
        /// <returns>The name of the source column mapped to the <see cref="T:System.Data.DataSet"/>. The default is an empty string.</returns>
        public override string SourceColumn { get; set; }

        /// <summary>
        /// Sets or gets a value which indicates whether the source column is nullable. This allows <see cref="T:System.Data.Common.DbCommandBuilder"/> to correctly generate Update statements for nullable columns.
        /// </summary>
        /// <returns>true if the source column is nullable; false if it is not.</returns>
        public override bool SourceColumnNullMapping { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="T:System.Data.DataRowVersion"/> to use when you load <see cref="P:System.Data.Common.DbParameter.Value"/>.
        /// </summary>
        /// <returns>One of the <see cref="T:System.Data.DataRowVersion"/> values. The default is Current.</returns>
        ///
        /// <exception cref="T:System.ArgumentException">The property is not set to one of the <see cref="T:System.Data.DataRowVersion"/> values.</exception>
        public override DataRowVersion SourceVersion { get; set; }

        /// <summary>
        /// Gets or sets the value of the parameter.
        /// </summary>
        /// <returns>An <see cref="T:System.Object"/> that is the value of the parameter. The default value is null.</returns>
        public override object Value { get; set; }

        /// <summary>
        /// Indicates the precision of numeric parameters.
        /// </summary>
        /// <returns>The maximum number of digits used to represent the Value property of a data provider Parameter object. The default value is 0, which indicates that a data provider sets the precision for Value.</returns>
        public override byte Precision { get; set; }

        /// <summary>
        /// Indicates the scale of numeric parameters.
        /// </summary>
        /// <returns>The number of decimal places to which <see cref="T:System.Data.OleDb.OleDbParameter.Value"/> is resolved. The default is 0.</returns>
        public override byte Scale { get; set; }

        /// <summary>
        /// Gets or sets the maximum size, in bytes, of the data within the column.
        /// </summary>
        /// <returns>The maximum size, in bytes, of the data within the column. The default value is inferred from the parameter value.</returns>
        public override int Size { get; set; }

        /// <summary>
        /// Resets the DbType property to its original settings.
        /// </summary>
        public override void ResetDbType()
        {
        }
    }
}