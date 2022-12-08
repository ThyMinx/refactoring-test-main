using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestAppLibrary
{
    public class ServiceDataAccess
    {
        private readonly string _connectionString;

        public ServiceDataAccess(string connectionString)
        {
            _connectionString = connectionString;
        }

        public Service GetService(string serviceName) {
            Service service = null;
            using (var conn = new SqlConnection(_connectionString))
            {
                using (var cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "Services_GetByName";
                    cmd.Parameters.AddWithValue("@name", serviceName);

                    conn.Open();
                    var dr = cmd.ExecuteReader();

                    if (dr.Read())
                    {
                        service = new Service { 
                            Id = Guid.Parse(dr["Id"].ToString()),
                            Name = dr["Name"].ToString()
                        };
                    }
                }
            }

            return service;
        }
    }
}
