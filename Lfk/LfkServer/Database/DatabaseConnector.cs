using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace LfkServer.Database
{
    abstract class DatabaseConnector
    {
        protected SqlConnection sqlConnection;

        protected void OpenConnection()
        {
            sqlConnection = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\courseWorkCS\Lfk\LfkServer\Database.mdf;Integrated Security=True");
            sqlConnection.Open();
        }

        protected void CloseConnection()
        {
            if (sqlConnection != null)
            {
                sqlConnection.Close();
            }
        }
    }
}