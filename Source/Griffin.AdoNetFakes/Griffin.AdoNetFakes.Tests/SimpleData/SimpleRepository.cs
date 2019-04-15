using System;
using System.Collections.Generic;
using System.Data;

namespace Griffin.AdoNetFakes.Tests.SimpleData
{
    public class SimpleRepository
    {
        private ISimpleConnectionFactory _connectionFactory;

        public SimpleRepository(ISimpleConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public SimpleObject Get()
        {
            using (var connection = _connectionFactory.GetConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT TOP 1 * from [SimpleObjects]";

                    using (var reader = command.ExecuteReader())
                    {
                        if (!reader.Read())
                            return null;

                        return Parse(reader);
                    }
                }
            }
        }

        public IList<SimpleObject> GetAll()
        {
            using (var connection = _connectionFactory.GetConnection())
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT * from [SimpleObjects]";

                    using (var reader = command.ExecuteReader())
                    {
                        var list = new List<SimpleObject>();

                        while (reader.Read())
                        {
                            list.Add(Parse(reader));
                        }

                        return list;
                    }
                }
            }
        }

        private static SimpleObject Parse(IDataRecord reader)
        {
            var instance = new SimpleObject()
            {
                Id = Convert.ToInt32(reader[nameof(SimpleObject.Id)]),
                Name = Convert.ToString(reader[nameof(SimpleObject.Name)]),
                DateOfBirth = Convert.ToDateTime(reader[nameof(SimpleObject.DateOfBirth)])
            };

            return instance;
        }
    }
}
