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
        public IDbConnection DbConnection { 
            get
            {
                if (con == null || con.State != ConnectionState.Open)
                {
                    
                    
                    const string conectString = "Server=mysql.bateaquihost.com.br;Database=isangue_ewertondev;uid=isangue_ewertondev;pwd=ewertondev123!;";
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
