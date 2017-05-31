using LfkGUI.Services;
using LfkClient.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LfkGUI.Services.TreeService;

namespace LfkGUI.ViewModels.RepositoryViewModels
{
    public class RepositoryAddCommandViewModel : BasicViewModel
    {
        private WindowsService windowsService;
        private LfkClient.Repository.Repository Repository = LfkClient.Repository.Repository.GetInstance();

        #region Свойства

        public TreeNode ChangedFiles { get; set; }
        public TreeNode PreparedToCommitFiles { get; set; }

        #endregion

        public RepositoryAddCommandViewModel(WindowsService windowsService)
        {
            this.windowsService = windowsService;
            UpdateTreeNodes();
        }

        private async void UpdateTreeNodes()
        {
            TreeBuilder treeBuilder = new TreeBuilder();
            ChangedFiles = treeBuilder.BuildTreeFromFilenames((await Repository.GetChangedFiles()).ToArray());
            PreparedToCommitFiles = treeBuilder.BuildTreeFromFilenames((await Repository.GetChangedFilesAfterParentCommit()).ToArray());
        }

        private RelayCommand addCommand;
        public RelayCommand AddCommand
        {
            get
            {
                return addCommand ?? (addCommand = new RelayCommand((node) =>
                {
                    Command(node, ChangedFiles, PreparedToCommitFiles, Repository.Add);
                }, obj =>
                {
                    return obj != null && obj is TreeNode;
                })
                );
            }
        }


        private RelayCommand resetCommand;
        public RelayCommand ResetCommand
        {
            get
            {
                return resetCommand ?? (resetCommand = new RelayCommand((node) =>
                {
                    Command(node, PreparedToCommitFiles, ChangedFiles, Repository.Reset);
                }, obj =>
                {
                    return obj != null && obj is TreeNode;
                })
                 );
            }
        }

        private void Command(object obj, TreeNode source, TreeNode destination, Action<List<string>> action)
        {
            TreeNode node = obj as TreeNode;
            List<string> filepaths = new List<string>();
            TreeBuilder builder = new TreeBuilder();
            builder.ConvertTreeToFilenames(node, ref filepaths);
            action(filepaths);

            builder.AttachNodeToTree(destination, node);
            builder.RemoveNodeFromTree(source, node);
        }
    }
}
