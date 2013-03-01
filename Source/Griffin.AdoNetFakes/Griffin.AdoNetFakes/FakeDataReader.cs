using System;
using System.Collections;
using System.Data;

namespace Griffin.AdoNetFakes
{
    /// <summary>
    ///     Fake reader
    /// </summary>
    /// <remarks>
    ///     Uses a data table as the source.
    ///     <para>
    ///         Inherit and override <see cref="SchemaTable" /> or
    ///         override <see cref="Factory.CreateSchemaTable" /> to provide your
    ///         own schema tables. An empty one is returned by default.
    ///     </para>
    /// </remarks>
    public class FakeDataReader : IDataReader
    {
        private readonly DataTable _table;
        private int _rowNumber = -1;
        private DataTable _schemaTable;

        /// <summary>
        ///     Initializes a new instance of the <see cref="FakeDataReader" /> class.
        /// </summary>
        /// <param name="table">The table uses as result.</param>
        public FakeDataReader(DataTable table)
        {
            _table = table;
        }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is closed result.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is closed result; otherwise, <c>false</c>.
        /// </value>
        public bool IsClosedResult { get; set; }

        /// <summary>
        ///     Gets or sets the schema table.
        /// </summary>
        /// <value>
        ///     The schema table.
        /// </value>
        /// <remarks>Not specified per default</remarks>
        protected DataTable SchemaTable
        {
            get { return _schemaTable ?? Factory.Instance.CreateSchemaTable(this); }
            set { _schemaTable = value; }
        }

        /// <summary>
        ///     Gets a value indicating whether this instance has rows.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance has rows; otherwise, <c>false</c>.
        /// </value>
        /// <remarks>returns the datatable count</remarks>
        public virtual bool HasRows
        {
            get { return _table.Rows.Count > 0; }
        }

        /// <summary>
        ///     Gets if disposed has been called.
        /// </summary>
        public bool IsDisposed { get; set; }

        #region IDataReader Members

        /// <summary>
        ///     Sets <see cref="IsClosedResult" /> to true
        /// </summary>
        public void Close()
        {
            IsClosedResult = true;
        }

        /// <summary>
        ///     Returns a <see cref="T:System.Data.DataTable" /> that describes the column metadata of the
        ///     <see
        ///         cref="T:System.Data.IDataReader" />
        ///     .
        /// </summary>
        /// <returns>
        ///     A <see cref="T:System.Data.DataTable" /> that describes the column metadata.
        /// </returns>
        /// <exception cref="T:System.InvalidOperationException">
        ///     The <see cref="T:System.Data.IDataReader" /> is closed.
        /// </exception>
        public DataTable GetSchemaTable()
        {
            return SchemaTable;
        }

        /// <summary>
        ///     Advances the data reader to the next result, when reading the results of batch SQL statements.
        /// </summary>
        /// <returns>
        ///     true if there are more rows; otherwise, false.
        /// </returns>
        public bool NextResult()
        {
            return _table.Rows.Count > _rowNumber;
        }

        /// <summary>
        ///     Advances the <see cref="T:System.Data.IDataReader" /> to the next record.
        /// </summary>
        /// <returns>
        ///     true if there are more rows; otherwise, false.
        /// </returns>
        public bool Read()
        {
            ++_rowNumber;
            return _table.Rows.Count > _rowNumber;
        }

        /// <summary>
        ///     Gets a value indicating the depth of nesting for the current row.
        /// </summary>
        /// <returns>The depth of nesting for the current row.</returns>
        public virtual int Depth
        {
            get { return 0; }
        }

        /// <summary>
        ///     Gets a value indicating whether the data reader is closed.
        /// </summary>
        /// <returns>true if the data reader is closed; otherwise, false.</returns>
        public virtual bool IsClosed
        {
            get { return IsClosedResult; }
        }

        /// <summary>
        ///     Gets the number of rows changed, inserted, or deleted by execution of the SQL statement.
        /// </summary>
        /// <returns>The number of rows changed, inserted, or deleted; 0 if no rows were affected or the statement failed; and -1 for SELECT statements.</returns>
        public virtual int RecordsAffected
        {
            get { return _table.Rows.Count; }
        }

        /// <summary>
        ///     Gets the boolean.
        /// </summary>
        /// <param name="ordinal">The ordinal.</param>
        /// <returns></returns>
        public virtual bool GetBoolean(int ordinal)
        {
            if (_rowNumber == -1) throw new InvalidOperationException("Call Read() first.");
            return (bool) _table.Rows[_rowNumber][ordinal];
        }

        /// <summary>
        ///     Gets the byte.
        /// </summary>
        /// <param name="ordinal">The ordinal.</param>
        /// <returns></returns>
        public virtual byte GetByte(int ordinal)
        {
            if (_rowNumber == -1) throw new InvalidOperationException("Call Read() first.");
            return (byte) _table.Rows[_rowNumber][ordinal];
        }

        /// <summary>
        ///     Gets the bytes.
        /// </summary>
        /// <param name="ordinal">The ordinal.</param>
        /// <param name="dataOffset">The data offset.</param>
        /// <param name="buffer">The buffer.</param>
        /// <param name="bufferOffset">The buffer offset.</param>
        /// <param name="length">The length.</param>
        /// <returns></returns>
        public virtual long GetBytes(int ordinal, long dataOffset, byte[] buffer, int bufferOffset, int length)
        {
            if (_rowNumber == -1) throw new InvalidOperationException("Call Read() first.");
            return (byte) _table.Rows[_rowNumber][ordinal];
        }

        /// <summary>
        ///     Gets the char.
        /// </summary>
        /// <param name="ordinal">The ordinal.</param>
        /// <returns></returns>
        public virtual char GetChar(int ordinal)
        {
            if (_rowNumber == -1) throw new InvalidOperationException("Call Read() first.");
            return (char) _table.Rows[_rowNumber][ordinal];
        }

        /// <summary>
        ///     Gets the chars.
        /// </summary>
        /// <param name="ordinal">The ordinal.</param>
        /// <param name="dataOffset">The data offset.</param>
        /// <param name="buffer">The buffer.</param>
        /// <param name="bufferOffset">The buffer offset.</param>
        /// <param name="length">The length.</param>
        /// <returns></returns>
        public virtual long GetChars(int ordinal, long dataOffset, char[] buffer, int bufferOffset, int length)
        {
            if (_rowNumber == -1) throw new InvalidOperationException("Call Read() first.");
            return (byte) _table.Rows[_rowNumber][ordinal];
        }

        /// <summary>
        ///     Gets the GUID.
        /// </summary>
        /// <param name="ordinal">The ordinal.</param>
        /// <returns></returns>
        public virtual Guid GetGuid(int ordinal)
        {
            if (_rowNumber == -1) throw new InvalidOperationException("Call Read() first.");
            return (Guid) _table.Rows[_rowNumber][ordinal];
        }

        /// <summary>
        ///     Gets the int16.
        /// </summary>
        /// <param name="ordinal">The ordinal.</param>
        /// <returns></returns>
        public virtual short GetInt16(int ordinal)
        {
            if (_rowNumber == -1) throw new InvalidOperationException("Call Read() first.");
            return (short) _table.Rows[_rowNumber][ordinal];
        }

        /// <summary>
        ///     Gets the int32.
        /// </summary>
        /// <param name="ordinal">The ordinal.</param>
        /// <returns></returns>
        public virtual int GetInt32(int ordinal)
        {
            if (_rowNumber == -1) throw new InvalidOperationException("Call Read() first.");
            return (int) _table.Rows[_rowNumber][ordinal];
        }

        /// <summary>
        ///     Gets the int64.
        /// </summary>
        /// <param name="ordinal">The ordinal.</param>
        /// <returns></returns>
        public virtual long GetInt64(int ordinal)
        {
            if (_rowNumber == -1) throw new InvalidOperationException("Call Read() first.");
            return (long) _table.Rows[_rowNumber][ordinal];
        }

        /// <summary>
        ///     Gets the date time.
        /// </summary>
        /// <param name="ordinal">The ordinal.</param>
        /// <returns></returns>
        public virtual DateTime GetDateTime(int ordinal)
        {
            if (_rowNumber == -1) throw new InvalidOperationException("Call Read() first.");
            return (DateTime) _table.Rows[_rowNumber][ordinal];
        }

        /// <summary>
        ///     Returns an <see cref="T:System.Data.IDataReader" /> for the specified column ordinal.
        /// </summary>
        /// <param name="i">The index of the field to find.</param>
        /// <returns>
        ///     An <see cref="T:System.Data.IDataReader" />.
        /// </returns>
        /// <exception cref="T:System.IndexOutOfRangeException">
        ///     The index passed was outside the range of 0 through <see cref="P:System.Data.IDataRecord.FieldCount" />.
        /// </exception>
        public IDataReader GetData(int i)
        {
            throw new NotSupportedException("Not implemented in the fake");
        }

        /// <summary>
        ///     Gets the string.
        /// </summary>
        /// <param name="ordinal">The ordinal.</param>
        /// <returns></returns>
        public virtual string GetString(int ordinal)
        {
            if (_rowNumber == -1) throw new InvalidOperationException("Call Read() first.");
            return (string) _table.Rows[_rowNumber][ordinal];
        }

        /// <summary>
        ///     Gets the value.
        /// </summary>
        /// <param name="ordinal">The ordinal.</param>
        /// <returns></returns>
        public virtual object GetValue(int ordinal)
        {
            if (_rowNumber == -1) throw new InvalidOperationException("Call Read() first.");
            return _table.Rows[_rowNumber][ordinal];
        }

        /// <summary>
        ///     Populates an array of objects with the column values of the current row.
        /// </summary>
        /// <param name="values">
        ///     An array of <see cref="T:System.Object" /> into which to copy the attribute columns.
        /// </param>
        /// <returns>
        ///     The number of instances of <see cref="T:System.Object" /> in the array.
        /// </returns>
        public virtual int GetValues(object[] values)
        {
            if (_rowNumber == -1) throw new InvalidOperationException("Call Read() first.");
            for (var i = 0; i < _table.Columns.Count; i++)
            {
                values[i] = _table.Rows[_rowNumber][i];
            }

            return _table.Columns.Count;
        }

        /// <summary>
        ///     Determines whether [is DB null] [the specified ordinal].
        /// </summary>
        /// <param name="ordinal">The ordinal.</param>
        /// <returns>
        ///     <c>true</c> if [is DB null] [the specified ordinal]; otherwise, <c>false</c>.
        /// </returns>
        public virtual bool IsDBNull(int ordinal)
        {
            if (_rowNumber == -1) throw new InvalidOperationException("Call Read() first.");
            return _table.Rows[_rowNumber][ordinal] is DBNull || _table.Rows[_rowNumber][ordinal] == null;
        }

        /// <summary>
        ///     Gets the number of columns in the current row.
        /// </summary>
        /// <returns>When not positioned in a valid recordset, 0; otherwise, the number of columns in the current record. The default is -1.</returns>
        public virtual int FieldCount
        {
            get { return _table.Columns.Count; }
        }

        /// <summary>
        ///     Gets the column with the specified name.
        /// </summary>
        /// <returns>
        ///     The column with the specified name as an <see cref="T:System.Object" />.
        /// </returns>
        /// <exception cref="T:System.IndexOutOfRangeException">No column with the specified name was found. </exception>
        public virtual object this[int ordinal]
        {
            get
            {
                if (_rowNumber == -1) throw new InvalidOperationException("Call Read() first.");

                return _table.Rows[_rowNumber][ordinal];
            }
        }

        /// <summary>
        ///     Gets the column with the specified name.
        /// </summary>
        /// <returns>
        ///     The column with the specified name as an <see cref="T:System.Object" />.
        /// </returns>
        /// <exception cref="T:System.IndexOutOfRangeException">No column with the specified name was found. </exception>
        public virtual object this[string name]
        {
            get
            {
                if (_rowNumber == -1) throw new InvalidOperationException("Call Read() first.");
                var index = GetOrdinal(name);
                return _table.Rows[_rowNumber][index];
            }
        }

        /// <summary>
        ///     Gets the decimal.
        /// </summary>
        /// <param name="ordinal">The ordinal.</param>
        /// <returns></returns>
        public virtual decimal GetDecimal(int ordinal)
        {
            if (_rowNumber == -1) throw new InvalidOperationException("Call Read() first.");
            return (decimal) _table.Rows[_rowNumber][ordinal];
        }

        /// <summary>
        ///     Gets the double.
        /// </summary>
        /// <param name="ordinal">The ordinal.</param>
        /// <returns></returns>
        public virtual double GetDouble(int ordinal)
        {
            if (_rowNumber == -1) throw new InvalidOperationException("Call Read() first.");
            return (double) _table.Rows[_rowNumber][ordinal];
        }

        /// <summary>
        ///     Gets the float.
        /// </summary>
        /// <param name="ordinal">The ordinal.</param>
        /// <returns></returns>
        public virtual float GetFloat(int ordinal)
        {
            if (_rowNumber == -1) throw new InvalidOperationException("Call Read() first.");
            return (float) _table.Rows[_rowNumber][ordinal];
        }

        /// <summary>
        ///     Gets the name.
        /// </summary>
        /// <param name="ordinal">The ordinal.</param>
        /// <returns></returns>
        public virtual string GetName(int ordinal)
        {
            return _table.Columns[ordinal].ColumnName;
        }

        /// <summary>
        ///     Return the index of the named field.
        /// </summary>
        /// <param name="name">The name of the field to find.</param>
        /// <returns>
        ///     The index of the named field.
        /// </returns>
        public virtual int GetOrdinal(string name)
        {
            if (name == null) throw new ArgumentNullException("name");
            for (var i = 0; i < _table.Columns.Count; i++)
            {
                if (_table.Columns[i].ColumnName.Equals(name))
                    return i;
            }
            
            throw new IndexOutOfRangeException(string.Format("Column '{0}' was not found.", name));
        }

        /// <summary>
        ///     Gets the name of the data type.
        /// </summary>
        /// <param name="ordinal">The ordinal.</param>
        /// <returns></returns>
        public virtual string GetDataTypeName(int ordinal)
        {
            if (_rowNumber == -1) throw new InvalidOperationException("Call Read() first.");
            return _table.Rows[_rowNumber][ordinal].GetType().Name;
        }

        /// <summary>
        ///     Gets the type of the field.
        /// </summary>
        /// <param name="ordinal">The ordinal.</param>
        /// <returns></returns>
        public virtual Type GetFieldType(int ordinal)
        {
            if (_rowNumber == -1) throw new InvalidOperationException("Call Read() first.");
            return _table.Rows[_rowNumber][ordinal].GetType();
        }

        /// <summary>
        ///     Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            IsDisposed = true;
        }

        #endregion

        /// <summary>
        ///     Gets the enumerator.
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerator GetEnumerator()
        {
            return _table.Rows.GetEnumerator();
        }
    }
}