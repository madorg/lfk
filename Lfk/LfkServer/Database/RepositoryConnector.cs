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

        public ServerRepository HandleRead(string repositoryId)
        {
            ServerRepository serverRepository;

            this.OpenConnection();

            Guid guid = Guid.Parse(repositoryId);
            DataContext dataContext = new DataContext(sqlConnection);

            Table<DBRepository> repositoresTable = dataContext.GetTable<DBRepository>();
            Table<Commit> commitsTable = dataContext.GetTable<Commit>();
            Table<RepoObject> repoObjectTable = dataContext.GetTable<RepoObject>();
            Table<File> filesTable = dataContext.GetTable<File>();


            //var repoObjectsAndFilenamesJoin = repoObjectTable.Join(
            //                filesTable,
            //                r => r.FileId,
            //                f => f.Id,
            //                (r, f) => new KeyValuePair<Guid, string>(r.FileId, f.Filename));

            List<LfkSharedResources.Models.Commit> commits = commitsTable
                .Where(c => c.RepositoryId == guid)
                .Select(c => new LfkSharedResources.Models.Commit()
                {
                    Id = c.Id,
                    Index = new LfkSharedResources.Models.Index()
                    {
                        Id = c.IndexId,
                        RepoObjectIdAndFileName =
                        repoObjectTable.Where(rep=>rep.IndexId == c.IndexId).Join(
                            filesTable,
                            r => r.FileId,
                            f => f.Id,
                            (r, f) => new KeyValuePair<Guid, string>(r.FileId, f.Filename))
                            .AsEnumerable()
                            .ToDictionary(m => m.Key, m => m.Value)
                        //repoObjectsAndFilenamesJoin.ToDictionary(m => m.Key, m => m.Value).Where()
                    },
                    Comment = c.Comment,
                    Date = c.Date
                }
                ).ToList();
            //List<RepoObject> objects = new List<RepoObject>();
            //foreach (Commit commit in commits)
            //{
            //    objects.AddRange(repoObjectTable.Where(o => o.IndexId == commit.IndexId));
            //}

            //repositoresTable.FirstOrDefault(r => r.Id == guid);
            //serverRepository = new ServerRepository()
            //{
            //    Objects = objects,
            //    Commits = commits
            //};



            this.CloseConnection();
            return null;
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

        public List<LocalRepository> HandleView(string userId)
        {
            this.OpenConnection();

            Guid guid = Guid.Parse(userId);
            DataContext dataContext = new DataContext(sqlConnection);
            Table<DBRepository> repositoriesTable = dataContext.GetTable<DBRepository>();

            List<LocalRepository> managedRepositories = repositoriesTable
                .Where(r => r.UserId == guid)
                .Select(m => new LocalRepository()
                {
                    Id = m.Id,
                    Title = m.Title,
                    UserId = m.UserId
                }).ToList();

            this.CloseConnection();

            return managedRepositories;
        }
    }
}