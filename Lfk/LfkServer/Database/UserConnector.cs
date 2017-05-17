using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LfkSharedResources.Models.User;
using System.Data.SqlClient;
using System.Data.Linq;
using LfkSharedResources.Models.DatabaseModels;
using LfkExceptions;

namespace LfkServer.Database
{
    class UserConnector : DatabaseConnector
    {
        public Guid Create(SignupUser user)
        {
            Guid guid = Guid.NewGuid();
            this.OpenConnection();

            DataContext dataContext = new DataContext(this.sqlConnection);
            Table<DBUser> usersTable = dataContext.GetTable<DBUser>();

            DBUser dbUser = new DBUser()
            {
                Id = guid,
                Name = user.Name,
                Email = user.Email,
                Password = user.Password
            };

            usersTable.InsertOnSubmit(dbUser);

            try
            {
                dataContext.SubmitChanges();   
            }
            catch (SqlException sqlException) when (sqlException.Number == 2627)
            {
                throw new DuplicateEmailException("Пользователь с e-mail " + dbUser.Email + " уже существует.");
            }
            finally
            {
                this.CloseConnection();
            }
            
            return guid;
        }

        public Guid Read(LoginUser user)
        {
            Guid guid = Guid.Empty;
            this.OpenConnection();

            DataContext dataContext = new DataContext(this.sqlConnection);
            Table<DBUser> usersTable = dataContext.GetTable<DBUser>();

            DBUser dbUser = usersTable.SingleOrDefault(u => u.Email == user.Email);
            if (dbUser == default(DBUser))
            {
                throw new WrongUserCredentialsException("Неверный e-mail");
            }
            else
            {
                if (dbUser.Password != user.Password)
                {
                    throw new WrongUserCredentialsException("Неверный пароль");
                }
                else
                {
                    guid = dbUser.Id;
                }
            }

            this.CloseConnection();

            return guid;
        }
    }
}