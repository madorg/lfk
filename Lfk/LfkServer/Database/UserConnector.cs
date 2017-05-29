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
using NLog;

namespace LfkServer.Database
{
    class UserConnector : DatabaseConnector
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public Guid Create(SignupUser user)
        {
            logger.Debug("Запуск обработки операции CREATE");

            Guid guid = Guid.NewGuid();

            this.OpenConnection();
            logger.Debug("CREATE: Соединение с БД открыто");

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
                logger.Debug("CREATE: Данные успешно добавлены в БД");
            }
            catch (SqlException sqlException) when (sqlException.Number == 2627)
            {
                logger.Error("CREATE: Ошибка SQL-сервера №2627, генерация DuplicateEmailException; подробности см. в LfkLog.txt");
                logger.Trace(sqlException.StackTrace);
                throw new DuplicateEmailException("Пользователь с e-mail " + dbUser.Email + " уже существует.");
            }
            finally
            {
                this.CloseConnection();
                logger.Debug("CREATE: Соединение с БД закрыто");
            }

            logger.Debug("Завершение обработки операции CREATE, ответ = " + guid.ToString());
            return guid;
        }

        public Guid Read(LoginUser user)
        {
            logger.Debug("Запуск обработки операции READ");

            Guid guid = Guid.Empty;

            this.OpenConnection();
            logger.Debug("READ: Соединение с БД открыто");

            DataContext dataContext = new DataContext(this.sqlConnection);
            Table<DBUser> usersTable = dataContext.GetTable<DBUser>();

            DBUser dbUser = usersTable.SingleOrDefault(u => u.Email == user.Email);
            if (dbUser == default(DBUser))
            {
                logger.Warn("Генерация WrongUserCredentialsException: неверный e-mail");
                throw new WrongUserCredentialsException("Неверный e-mail");
            }
            else
            {
                if (dbUser.Password != user.Password)
                {
                    logger.Warn("READ: Генерация WrongUserCredentialsException: неверный пароль");
                    throw new WrongUserCredentialsException("Неверный пароль");
                }
                else
                {
                    guid = dbUser.Id;
                }
            }

            this.CloseConnection();
            logger.Debug("READ: Соединение с БД закрыто");

            logger.Debug("Завершение обработки операции READ, ответ = " + guid.ToString());
            return guid;
        }
    }
}