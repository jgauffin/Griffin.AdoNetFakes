using System;
using System.Collections.Generic;
using System.Data;

namespace Griffin.AdoNetFakes
{
    /// <summary>
    /// Simple collection
    /// </summary>
    public class ParameterCollection : List<FakeParameter>, IDataParameterCollection
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ParameterCollection"/> class.
        /// </summary>
        /// <param name="command">The commandResult.</param>
        public ParameterCollection(FakeCommand command)
        {
            Command = command;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParameterCollection"/> class.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        public ParameterCollection(ParameterCollection parameters)
        {
            AddRange(parameters);
            Command = parameters.Command;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ParameterCollection"/> class.
        /// </summary>
        /// <param name="parameters">Add a range of paramets to this collection</param>
        public ParameterCollection(params FakeParameter[] parameters)
        {
            if (parameters == null) throw new ArgumentNullException("parameters");
            foreach (var parameter in parameters)
            {
                Add(parameter);
            }
        }


        /// <summary>
        /// Gets commandResult that the parameters is for (if specified in the constructor)
        /// </summary>
        public FakeCommand Command { get; set; }

        #region IDataParameterCollection Members

        /// <summary>
        /// Gets a value indicating whether a parameter in the collection has the specified name.
        /// </summary>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <returns>
        /// true if the collection contains the parameter; otherwise, false.
        /// </returns>
        public bool Contains(string parameterName)
        {
            return IndexOf(parameterName) != -1;
        }

        /// <summary>
        /// Gets the location of the <see cref="T:System.Data.IDataParameter"/> within the collection.
        /// </summary>
        /// <param name="parameterName">The name of the parameter.</param>
        /// <returns>
        /// The zero-based location of the <see cref="T:System.Data.IDataParameter"/> within the collection.
        /// </returns>
        public int IndexOf(string parameterName)
        {
            var index = 0;
            foreach (var p in this)
            {
                if (p.ParameterName.Equals(parameterName, StringComparison.OrdinalIgnoreCase))
                    return index;

                index++;
            }

            return -1;
        }

        /// <summary>
        /// Removes the <see cref="T:System.Data.IDataParameter"/> from the collection.
        /// </summary>
        /// <param name="parameterName">The name of the parameter.</param>
        public void RemoveAt(string parameterName)
        {
            var index = IndexOf(parameterName);
            if (index != -1)
                RemoveAt(index);
        }

        /// <summary>
        /// Gets or sets the <see cref="System.Object"/> with the specified parameter name.
        /// </summary>
        public FakeParameter this[string parameterName]
        {
            get
            {
                var index = IndexOf(parameterName);
                return index == -1 ? null : this[index];
            }
            set
            {
                var index = IndexOf(parameterName);
                if (index == -1)
                    throw new ArgumentOutOfRangeException("parameterName", parameterName);

                this[index] = value;
            }
        }

        object IDataParameterCollection.this[string parameterName]
        {
            get
            {
                var index = IndexOf(parameterName);
                return index == -1 ? null : this[index];
            }
            set
            {
                var index = IndexOf(parameterName);
                if (index == -1)
                    throw new ArgumentOutOfRangeException("parameterName", parameterName);

                this[index].Value = value;
            }
        }

        #endregion
    }
}