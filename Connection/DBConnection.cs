using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace TestCrud.Connection
{
    public class DBConnection
    {
        private SqlConnection _connection;
        private string _connectionString = "Data Source=(localdb)\\TestCrud;Initial Catalog=Test-Crud;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        public  SqlConnection Connection
        {
            get
            {

                    _connection = new SqlConnection(_connectionString);
                    _connection.Open();
                return _connection;

            }
        }

    }
}
