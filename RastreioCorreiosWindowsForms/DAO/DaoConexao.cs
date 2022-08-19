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

        private IDbConnection con { get; set; }
        public IDbConnection DbConnection
        {
            get
            {
                if (con == null || con.State != ConnectionState.Open)
                {
                    const string conectString = "Server=us-cdbr-east-06.cleardb.net;Database=heroku_0aa954f85b2a5b0;uid=bf1e68a41157c2;pwd=f69196af;";
                    MySqlConnection dbCon = new MySqlConnection();
                    dbCon.ConnectionString = conectString;
                    dbCon.Open();
                    con = dbCon;
                }
                return con;
            }
            set
            {
                con = value;
            }
        }


        public void Dispose()
        {
            GC.Collect();
        }
    }
}
