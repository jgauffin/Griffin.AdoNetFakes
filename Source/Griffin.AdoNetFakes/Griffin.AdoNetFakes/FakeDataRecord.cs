using System;
using System.Collections.Generic;
using System.Data;

namespace Griffin.AdoNetFakes
{
    /// <summary>
    /// A fake data record.
    /// </summary>
    public class FakeDataRecord : IDataRecord
    {
        private readonly int _columnCount;

        private readonly Func<int, FakeColumn> _func;
        private readonly IList<FakeColumn> _row;

        /// <summary>
        /// Initializes a new instance of the <see cref="FakeDataRecord" /> class.
        /// </summary>
        /// <param name="row">The row that the record fronts.</param>
        public FakeDataRecord(IList<FakeColumn> row)
        {
            _row = row;
            _columnCount = row.Count;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FakeDataRecord" /> class.
        /// </summary>
        /// <param name="columnAccessor">The column accessor.</param>
        /// <param name="columnCount">The column count.</param>
        /// <remarks>Let's you use a func to return the columns.</remarks>
        public FakeDataRecord(Func<int, FakeColumn> columnAccessor, int columnCount)
        {
            _func = columnAccessor;
            _columnCount = columnCount;
        }

        #region IDataRecord Members

        /// <summary>
        /// Gets the name for the field to find.
        /// </summary>
        /// <returns>
        /// The name of the field or the empty string (""), if there is no value to return.
        /// </returns>
        /// <param name="i">The index of the field to find. </param><exception cref="T:System.IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="P:System.Data.IDataRecord.FieldCount"/>. </exception><filterpriority>2</filterpriority>
        public string GetName(int i)
        {
            if (i < 0 || i >= _columnCount)
                throw new ArgumentOutOfRangeException("i");

            return GetColumn(i).ColumnName;
        }

        /// <summary>
        /// Gets the data type information for the specified field.
        /// </summary>
        /// <returns>
        /// The data type information for the specified field.
        /// </returns>
        /// <param name="i">The index of the field to find. </param><exception cref="T:System.IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="P:System.Data.IDataRecord.FieldCount"/>. </exception><filterpriority>2</filterpriority>
        public string GetDataTypeName(int i)
        {
            if (i < 0 || i >= _columnCount)
                throw new ArgumentOutOfRangeException("i");

            var col = GetColumn(i);
            if (col.Value == DBNull.Value || col.Value == null)
                return col.ColumnType.Name;

            return col.Value.GetType().Name;
        }

        /// <summary>
        /// Gets the <see cref="T:System.Type"/> information corresponding to the type of <see cref="T:System.Object"/> that would be returned from <see cref="M:System.Data.IDataRecord.GetValue(System.Int32)"/>.
        /// </summary>
        /// <returns>
        /// The <see cref="T:System.Type"/> information corresponding to the type of <see cref="T:System.Object"/> that would be returned from <see cref="M:System.Data.IDataRecord.GetValue(System.Int32)"/>.
        /// </returns>
        /// <param name="i">The index of the field to find. </param><exception cref="T:System.IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="P:System.Data.IDataRecord.FieldCount"/>. </exception><filterpriority>2</filterpriority>
        public Type GetFieldType(int i)
        {
            if (i < 0 || i >= _columnCount)
                throw new ArgumentOutOfRangeException("i");

            var col = GetColumn(i);
            if (col.Value == DBNull.Value || col.Value == null)
                return col.ColumnType;

            return col.Value.GetType();
        }

        /// <summary>
        /// Return the value of the specified field.
        /// </summary>
        /// <returns>
        /// The <see cref="T:System.Object"/> which will contain the field value upon return.
        /// </returns>
        /// <param name="i">The index of the field to find. </param><exception cref="T:System.IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="P:System.Data.IDataRecord.FieldCount"/>. </exception><filterpriority>2</filterpriority>
        public object GetValue(int i)
        {
            if (i < 0 || i >= _columnCount)
                throw new ArgumentOutOfRangeException("i");

            var col = GetColumn(i);
            return col.Value;
        }

        /// <summary>
        /// Populates an array of objects with the column values of the current record.
        /// </summary>
        /// <returns>
        /// The number of instances of <see cref="T:System.Object"/> in the array.
        /// </returns>
        /// <param name="values">An array of <see cref="T:System.Object"/> to copy the attribute fields into. </param><filterpriority>2</filterpriority>
        public int GetValues(object[] values)
        {
            for (var i = 0; i < _columnCount; i++)
            {
                values[i] = GetColumn(i).Value;
            }

            return _columnCount;
        }

        /// <summary>
        /// Return the index of the named field.
        /// </summary>
        /// <returns>
        /// The index of the named field.
        /// </returns>
        /// <param name="name">The name of the field to find. </param><filterpriority>2</filterpriority>
        public int GetOrdinal(string name)
        {
            for (var i = 0; i < _columnCount; i++)
            {
                if (name == GetColumn(i).ColumnName)
                    return i;
            }

            return -1;
        }

        /// <summary>
        /// Gets the value of the specified column as a Boolean.
        /// </summary>
        /// <returns>
        /// The value of the column.
        /// </returns>
        /// <param name="i">The zero-based column ordinal. </param><exception cref="T:System.IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="P:System.Data.IDataRecord.FieldCount"/>. </exception><filterpriority>2</filterpriority>
        public bool GetBoolean(int i)
        {
            if (i < 0 || i >= _columnCount)
                throw new ArgumentOutOfRangeException("i");

            var col = GetColumn(i);
            return (bool) col.Value;
        }

        /// <summary>
        /// Gets the 8-bit unsigned integer value of the specified column.
        /// </summary>
        /// <returns>
        /// The 8-bit unsigned integer value of the specified column.
        /// </returns>
        /// <param name="i">The zero-based column ordinal. </param><exception cref="T:System.IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="P:System.Data.IDataRecord.FieldCount"/>. </exception><filterpriority>2</filterpriority>
        public byte GetByte(int i)
        {
            if (i < 0 || i >= _columnCount)
                throw new ArgumentOutOfRangeException("i");

            var col = GetColumn(i);
            return (byte) col.Value;
        }

        /// <summary>
        /// Reads a stream of bytes from the specified column offset into the buffer as an array, starting at the given buffer offset.
        /// </summary>
        /// <returns>
        /// The actual number of bytes read.
        /// </returns>
        /// <param name="i">The zero-based column ordinal. </param><param name="fieldOffset">The index within the field from which to start the read operation. </param><param name="buffer">The buffer into which to read the stream of bytes. </param><param name="bufferoffset">The index for <paramref name="buffer"/> to start the read operation. </param><param name="length">The number of bytes to read. </param><exception cref="T:System.IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="P:System.Data.IDataRecord.FieldCount"/>. </exception><filterpriority>2</filterpriority>
        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the character value of the specified column.
        /// </summary>
        /// <returns>
        /// The character value of the specified column.
        /// </returns>
        /// <param name="i">The zero-based column ordinal. </param><exception cref="T:System.IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="P:System.Data.IDataRecord.FieldCount"/>. </exception><filterpriority>2</filterpriority>
        public char GetChar(int i)
        {
            if (i < 0 || i >= _columnCount)
                throw new ArgumentOutOfRangeException("i");

            var col = GetColumn(i);
            return (char) col.Value;
        }

        /// <summary>
        /// Reads a stream of characters from the specified column offset into the buffer as an array, starting at the given buffer offset.
        /// </summary>
        /// <returns>
        /// The actual number of characters read.
        /// </returns>
        /// <param name="i">The zero-based column ordinal. </param><param name="fieldoffset">The index within the row from which to start the read operation. </param><param name="buffer">The buffer into which to read the stream of bytes. </param><param name="bufferoffset">The index for <paramref name="buffer"/> to start the read operation. </param><param name="length">The number of bytes to read. </param><exception cref="T:System.IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="P:System.Data.IDataRecord.FieldCount"/>. </exception><filterpriority>2</filterpriority>
        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Returns the GUID value of the specified field.
        /// </summary>
        /// <returns>
        /// The GUID value of the specified field.
        /// </returns>
        /// <param name="i">The index of the field to find. </param><exception cref="T:System.IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="P:System.Data.IDataRecord.FieldCount"/>. </exception><filterpriority>2</filterpriority>
        public Guid GetGuid(int i)
        {
            if (i < 0 || i >= _columnCount)
                throw new ArgumentOutOfRangeException("i");

            var col = GetColumn(i);
            return (Guid) col.Value;
        }

        /// <summary>
        /// Gets the 16-bit signed integer value of the specified field.
        /// </summary>
        /// <returns>
        /// The 16-bit signed integer value of the specified field.
        /// </returns>
        /// <param name="i">The index of the field to find. </param><exception cref="T:System.IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="P:System.Data.IDataRecord.FieldCount"/>. </exception><filterpriority>2</filterpriority>
        public short GetInt16(int i)
        {
            if (i < 0 || i >= _columnCount)
                throw new ArgumentOutOfRangeException("i");

            var col = GetColumn(i);
            return (Int16) col.Value;
        }

        /// <summary>
        /// Gets the 32-bit signed integer value of the specified field.
        /// </summary>
        /// <returns>
        /// The 32-bit signed integer value of the specified field.
        /// </returns>
        /// <param name="i">The index of the field to find. </param><exception cref="T:System.IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="P:System.Data.IDataRecord.FieldCount"/>. </exception><filterpriority>2</filterpriority>
        public int GetInt32(int i)
        {
            if (i < 0 || i >= _columnCount)
                throw new ArgumentOutOfRangeException("i");

            var col = GetColumn(i);
            return (Int32) col.Value;
        }

        /// <summary>
        /// Gets the 64-bit signed integer value of the specified field.
        /// </summary>
        /// <returns>
        /// The 64-bit signed integer value of the specified field.
        /// </returns>
        /// <param name="i">The index of the field to find. </param><exception cref="T:System.IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="P:System.Data.IDataRecord.FieldCount"/>. </exception><filterpriority>2</filterpriority>
        public long GetInt64(int i)
        {
            if (i < 0 || i >= _columnCount)
                throw new ArgumentOutOfRangeException("i");

            var col = GetColumn(i);
            return (Int64) col.Value;
        }

        /// <summary>
        /// Gets the single-precision floating point number of the specified field.
        /// </summary>
        /// <returns>
        /// The single-precision floating point number of the specified field.
        /// </returns>
        /// <param name="i">The index of the field to find. </param><exception cref="T:System.IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="P:System.Data.IDataRecord.FieldCount"/>. </exception><filterpriority>2</filterpriority>
        public float GetFloat(int i)
        {
            if (i < 0 || i >= _columnCount)
                throw new ArgumentOutOfRangeException("i");

            var col = GetColumn(i);
            return (float) col.Value;
        }

        /// <summary>
        /// Gets the double-precision floating point number of the specified field.
        /// </summary>
        /// <returns>
        /// The double-precision floating point number of the specified field.
        /// </returns>
        /// <param name="i">The index of the field to find. </param><exception cref="T:System.IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="P:System.Data.IDataRecord.FieldCount"/>. </exception><filterpriority>2</filterpriority>
        public double GetDouble(int i)
        {
            if (i < 0 || i >= _columnCount)
                throw new ArgumentOutOfRangeException("i");

            var col = GetColumn(i);
            return (double) col.Value;
        }

        /// <summary>
        /// Gets the string value of the specified field.
        /// </summary>
        /// <returns>
        /// The string value of the specified field.
        /// </returns>
        /// <param name="i">The index of the field to find. </param><exception cref="T:System.IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="P:System.Data.IDataRecord.FieldCount"/>. </exception><filterpriority>2</filterpriority>
        public string GetString(int i)
        {
            if (i < 0 || i >= _columnCount)
                throw new ArgumentOutOfRangeException("i");

            var col = GetColumn(i);
            return (string) col.Value;
        }

        /// <summary>
        /// Gets the fixed-position numeric value of the specified field.
        /// </summary>
        /// <returns>
        /// The fixed-position numeric value of the specified field.
        /// </returns>
        /// <param name="i">The index of the field to find. </param><exception cref="T:System.IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="P:System.Data.IDataRecord.FieldCount"/>. </exception><filterpriority>2</filterpriority>
        public decimal GetDecimal(int i)
        {
            if (i < 0 || i >= _columnCount)
                throw new ArgumentOutOfRangeException("i");

            var col = GetColumn(i);
            return (decimal) col.Value;
        }

        /// <summary>
        /// Gets the date and time data value of the specified field.
        /// </summary>
        /// <returns>
        /// The date and time data value of the specified field.
        /// </returns>
        /// <param name="i">The index of the field to find. </param><exception cref="T:System.IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="P:System.Data.IDataRecord.FieldCount"/>. </exception><filterpriority>2</filterpriority>
        public DateTime GetDateTime(int i)
        {
            if (i < 0 || i >= _columnCount)
                throw new ArgumentOutOfRangeException("i");

            var col = GetColumn(i);
            return (DateTime) col.Value;
        }

        /// <summary>
        /// Returns an <see cref="T:System.Data.IDataReader"/> for the specified column ordinal.
        /// </summary>
        /// <returns>
        /// The <see cref="T:System.Data.IDataReader"/> for the specified column ordinal.
        /// </returns>
        /// <param name="i">The index of the field to find. </param><exception cref="T:System.IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="P:System.Data.IDataRecord.FieldCount"/>. </exception><filterpriority>2</filterpriority>
        public IDataReader GetData(int i)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Return whether the specified field is set to null.
        /// </summary>
        /// <returns>
        /// true if the specified field is set to null; otherwise, false.
        /// </returns>
        /// <param name="i">The index of the field to find. </param><exception cref="T:System.IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="P:System.Data.IDataRecord.FieldCount"/>. </exception><filterpriority>2</filterpriority>
        public bool IsDBNull(int i)
        {
            if (i < 0 || i >= _columnCount)
                throw new ArgumentOutOfRangeException("i");

            var col = GetColumn(i);
            return col.Value == DBNull.Value;
        }

        /// <summary>
        /// Gets the number of columns in the current row.
        /// </summary>
        /// <returns>
        /// When not positioned in a valid recordset, 0; otherwise, the number of columns in the current record. The default is -1.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public int FieldCount
        {
            get { return _columnCount; }
        }

        /// <summary>
        /// Gets the column located at the specified index.
        /// </summary>
        /// <returns>
        /// The column located at the specified index as an <see cref="T:System.Object"/>.
        /// </returns>
        /// <param name="i">The zero-based index of the column to get. </param><exception cref="T:System.IndexOutOfRangeException">The index passed was outside the range of 0 through <see cref="P:System.Data.IDataRecord.FieldCount"/>. </exception><filterpriority>2</filterpriority>
        object IDataRecord.this[int i]
        {
            get
            {
                if (i < 0 || i >= _columnCount)
                    throw new ArgumentOutOfRangeException("i");

                var col = GetColumn(i);
                return col.Value;
            }
        }

        /// <summary>
        /// Gets the column with the specified name.
        /// </summary>
        /// <returns>
        /// The column with the specified name as an <see cref="T:System.Object"/>.
        /// </returns>
        /// <param name="name">The name of the column to find. </param><exception cref="T:System.IndexOutOfRangeException">No column with the specified name was found. </exception><filterpriority>2</filterpriority>
        object IDataRecord.this[string name]
        {
            get
            {
                var index = GetOrdinal(name);
                return GetColumn(index);
            }
        }

        #endregion

        private FakeColumn GetColumn(int i)
        {
            return _row != null ? _row[i] : _func(i);
        }
    }
}