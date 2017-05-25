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
using LfkSharedResources.Models;

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
            Table<DBCommit> commitsTable = dataContext.GetTable<DBCommit>();
            Table<DBRepoObject> repoObjectTable = dataContext.GetTable<DBRepoObject>();
            Table<DBFile> filesTable = dataContext.GetTable<DBFile>();

            List<Commit> commits = new List<Commit>();
            List<RepoObject> repoObjects = new List<RepoObject>();
            List<File> files = new List<File>();

            LocalRepository localRepository = (LocalRepository)repositoresTable.FirstOrDefault(r => r.Id == guid);

            //ЗАПРЕТИТЬ БЕЗ КОММИТА ДЕЛАТЬ UPDATE!!!!!!!!!!!!!!!!
            foreach (DBCommit commit in commitsTable.Where(c => c.RepositoryId == guid))
            {
                Commit newCommit =
                    new Commit()
                    {
                        Id = commit.Id,
                        Date = commit.Date,
                        Comment = commit.Comment,
                        Index = new Index()
                        {
                            Id = commit.IndexId,
                            RepoObjectIdAndFileName = new Dictionary<Guid, string>(
                            repoObjectTable.Where(repo => repo.IndexId == commit.IndexId)
                            .Join(filesTable,
                            r => r.FileId,
                            f => f.Id,
                            (r, f) => new { Key = r.Id, Value = f.Filename }
                            )
                            .ToDictionary(m => m.Key, m => m.Value))
                        }
                    };
                commits.Add(newCommit);
            }

            foreach (Commit commit in commits)
            {
                repoObjects.AddRange(repoObjectTable
                    .Where(ro => ro.IndexId == commit.Index.Id)
                    .Select(m =>
                    new RepoObject()
                    {
                        Id = m.Id,
                        FileId = m.FileId,
                        Hash = m.Data,
                        HuffmanTree = m.HuffmanTree,
                        IndexId = m.IndexId
                    }
                    ));
            }
            //проблема очень надо решить
            foreach (RepoObject repoObject in repoObjects)
            {
                files.AddRange(filesTable.Where(f => f.Id == repoObject.FileId).Select(m =>
                  new File()
                  {
                      Id = m.Id,
                      Filename = m.Filename
                  }).Distinct());
            }
            serverRepository = new ServerRepository()
            {
                Commits = commits,
                LocalRepository = localRepository,
                Files = files,
                Objects = repoObjects
            };

            this.CloseConnection();
            return serverRepository;
        }

        public void HandleUpdate(ServerRepository serverRepository)
        {
            this.OpenConnection();

            DataContext dataContext = new DataContext(sqlConnection);

            Table<DBCommit> commitsTable = dataContext.GetTable<DBCommit>();
            Table<DBRepoObject> repoObjectsTable = dataContext.GetTable<DBRepoObject>();
            Table<DBFile> filesTable = dataContext.GetTable<DBFile>();

            var newCommits = serverRepository.Commits.Select(commit => new DBCommit()
            {
                Id = commit.Id,
                RepositoryId = serverRepository.LocalRepository.Id,
                IndexId = commit.Index.Id,
                Date = commit.Date,
                Comment = commit.Comment
            })
            .Where(newc =>
            !commitsTable.Any(c => c.Id == newc.Id)
            );
            commitsTable.InsertAllOnSubmit(newCommits);

            var newRepoObjects = serverRepository.Objects.Select(repoObject => new DBRepoObject()
            {
                Id = repoObject.Id,
                FileId = repoObject.FileId,
                IndexId = repoObject.IndexId,
                Data = repoObject.Hash,
                HuffmanTree = repoObject.HuffmanTree
            }).Where(m => !repoObjectsTable.Any(r => r.Id == m.Id));
            repoObjectsTable.InsertAllOnSubmit(newRepoObjects);

            var newFiles = serverRepository.Files.Select(f => new DBFile()
            {
                Id = f.Id,
                Filename = f.Filename
            }).Where(m => !filesTable.Any(df => df.Id == m.Id));

            filesTable.InsertAllOnSubmit(newFiles);
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

        public void HandleDelete(string repositoryId)
        {
            this.OpenConnection();
            Guid guid = Guid.Parse(repositoryId);
            DataContext dataContext = new DataContext(sqlConnection);

            Table<DBRepository> repositoryTable = dataContext.GetTable<DBRepository>();
            Table<DBCommit> commitsTable = dataContext.GetTable<DBCommit>();
            Table<DBRepoObject> repoObjectsTable = dataContext.GetTable<DBRepoObject>();
            Table<DBFile> filesTable = dataContext.GetTable<DBFile>();

            List<DBCommit> commits = commitsTable.Where(commit => commit.RepositoryId == guid).ToList();
            List<DBRepoObject> repoObjects = new List<DBRepoObject>();
            List<DBFile> files = new List<DBFile>();

            foreach (DBCommit commit in commits)
            {
                repoObjects.AddRange(repoObjectsTable.Where(r => r.IndexId == commit.IndexId));
            }

            foreach (DBRepoObject repoObject in repoObjects)
            {
                files.AddRange(filesTable.Where(f => f.Id == repoObject.FileId));
            }

            filesTable.DeleteAllOnSubmit(files);
            repoObjectsTable.DeleteAllOnSubmit(repoObjects);
            commitsTable.DeleteAllOnSubmit(commits);
            repositoryTable.DeleteOnSubmit(repositoryTable.FirstOrDefault(m => m.Id == guid));

            dataContext.SubmitChanges();

            this.CloseConnection();
        }
    }
}