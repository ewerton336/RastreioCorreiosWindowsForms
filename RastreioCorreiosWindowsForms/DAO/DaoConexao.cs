using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RastreioCorreiosWindowsForms.DAO
{
    public class DaoConexao : IDisposable
    {
        public IDbConnection DbConnection { get; private set; }
        public DaoConexao(MySqlConnection dbConnection)
        {

            if (dbConnection.State != ConnectionState.Open)
            {
                dbConnection.ConnectionString = "Server=mysql.bateaquihost.com.br;Database=isangue_ewertondev;uid=isangue_ewertondev;pwd=ewertondev123!";
                dbConnection.Open();
            }

            DbConnection = dbConnection;
        }

        public void Dispose()
        {
            GC.Collect();
        }
    }
}
