using LfkSharedResources.Models.Repository;
using LfkSharedResources.Models.DatabaseModels;
using LfkSharedResources;
using System;
using System.Data.Linq;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LfkExceptions;

namespace LfkServer.Database
{
    class RepositoryConnector : DatabaseConnector
    {
        public void HandleCreate(ServerRepository serverRepository)
        {
            this.OpenConnection();

            DataContext dataContext = new DataContext(sqlConnection);
            Table<DBRepository> repositoresTable = dataContext.GetTable<DBRepository>();

            DBRepository dbRepository = new DBRepository()
            {
                Id = serverRepository.LocalRepository.Id,
                Title = serverRepository.LocalRepository.Title,
                UserId = serverRepository.LocalRepository.UserId
            };

            repositoresTable.InsertOnSubmit(dbRepository);

            try
            {
                dataContext.SubmitChanges();
            }
            catch (SqlException sqlException) when (sqlException.Number == 2627)
            {
                throw new DuplicateRepositoryTitleException("Репозиторий с именем " + dbRepository.Title + " уже существует.");
            }
            finally
            {
                this.CloseConnection();
            }
        }

        public void HandleUpdate(ServerRepository serverRepository)
        {
            this.OpenConnection();

            DataContext dataContext = new DataContext(sqlConnection);

            Table<Commit> commitsTable = dataContext.GetTable<Commit>();
            Table<RepoObject> repoObjectsTable = dataContext.GetTable<RepoObject>();

            foreach (var commit in serverRepository.Commits)
            {
                Commit newCommit = new Commit()
                {
                    Id = commit.Id,
                    RepositoryId = serverRepository.LocalRepository.Id,
                    IndexId = commit.Index.Id,
                    Date = commit.Date,
                    Comment = commit.Comment
                };

                commitsTable.InsertOnSubmit(newCommit);
            }

            foreach (var repoObject in serverRepository.Objects)
            {
                RepoObject newRepoObject = new RepoObject()
                {
                    Id = repoObject.Id,
                    FileId = Guid.NewGuid(),
                    IndexId = repoObject.IndexId,
                    Data = repoObject.Hash,
                    HuffmanTree = repoObject.HuffmanTree
                };

                repoObjectsTable.InsertOnSubmit(newRepoObject);
            }

            dataContext.SubmitChanges();
            this.CloseConnection();
        }
    }
}