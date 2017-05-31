using LfkGUI.Services;
using LfkGUI.Services.TreeService;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LfkGUI.ViewModels.RepositoryViewModels
{
    public class RepositoryIncludeCommandViewModel : BasicViewModel
    {
        private WindowsService windowsService;
        private LfkClient.Repository.Repository Repository = LfkClient.Repository.Repository.GetInstance();

        #region Свойства

        public TreeNode UntrackedFilesRoot { get; set; }
        public TreeNode TrackedFilesRoot { get; set; }

        #endregion

        public RepositoryIncludeCommandViewModel(WindowsService windowsService)
        {
            this.windowsService = windowsService;
            UpdateTreeNodes();
        }

        private void UpdateTreeNodes()
        {
            TreeBuilder treeBuilder = new TreeBuilder();
            UntrackedFilesRoot = treeBuilder.BuildTreeFromFilenames(Repository.GetUnincludedFiles().ToArray());
            TrackedFilesRoot = treeBuilder.BuildTreeFromFilenames(Repository.GetIncludedFiles().ToArray());
        }

        private RelayCommand includeCommand;
        public RelayCommand IncludeCommand
        {
            get
            {
                return includeCommand ?? (includeCommand = new RelayCommand((node) =>
                {
                    Command(node,UntrackedFilesRoot, TrackedFilesRoot, Repository.Include);
                }, obj =>
                {
                    return obj != null && obj is TreeNode;
                })
                );
            }
        }


        private RelayCommand unincludeCommand;
        public RelayCommand UnincludeCommand
        {
            get
            {
                return unincludeCommand ?? (unincludeCommand = new RelayCommand((node) =>
                {
                    Command(node,TrackedFilesRoot, UntrackedFilesRoot, Repository.Uninclude);
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
