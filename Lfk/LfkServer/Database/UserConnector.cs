using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LfkSharedResources.Models.User;
using System.Data.SqlClient;

namespace LfkServer.Database
{
    class UserConnector : DatabaseConnector
    {
        public Guid Create(SignupUser user)
        {
            Guid guid = Guid.NewGuid();

            this.OpenConnection();

            SqlCommand sqlCommand = new SqlCommand(
                $"INSERT INTO users VALUES ('{ guid }', '{ user.Name }', '{ user.Email }', '{ user.Password }');", this.sqlConnection);

            try
            {
                if (sqlCommand.ExecuteNonQuery() == 0)
                {
                    guid = Guid.Empty;
                }
            }
            catch (SqlException sqlex)
            {
                Console.WriteLine("Исключение: " + sqlex.Message);
                guid = Guid.Empty;
                throw;
            }

            this.CloseConnection();

            return guid;
        }
    }
}