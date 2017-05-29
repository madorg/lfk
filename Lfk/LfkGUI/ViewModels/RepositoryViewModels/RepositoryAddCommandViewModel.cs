using LfkGUI.Services;
using LfkClient.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LfkGUI.Utility;

namespace LfkGUI.ViewModels.RepositoryViewModels
{
    public class RepositoryAddCommandViewModel : BasicViewModel
    {
        private WindowsService windowsService;
        private DialogService dialogService;
        private LfkClient.Repository.Repository Repository = LfkClient.Repository.Repository.GetInstance();

        #region Свойства

        ObservableCollection<string> ChangedFiles { get; set; }
        ObservableCollection<string> FilesToBeCommitted { get; set; }

        #endregion

        public RepositoryAddCommandViewModel(WindowsService windowsService)
        {
            ChangedFiles = new ObservableCollection<string>();
            FilesToBeCommitted = new ObservableCollection<string>();
            dialogService = new DialogService();
            this.windowsService = windowsService;
            UpdateCollections();
        }

        private async void UpdateCollections()
        {
            foreach (var item in await Repository.GetChangedFiles())
            {
                if (!ChangedFiles.Contains(item))
                    ChangedFiles.Add(item);
            }

            foreach (var item in await Repository.GetChangedFilesAfterParentCommit())
            {
                if (!FilesToBeCommitted.Contains(item))
                    FilesToBeCommitted.Add(item);
            }
        }

        private RelayCommand addCommand;
        public RelayCommand AddCommand
        {
            get
            {
                return addCommand ?? (addCommand = new RelayCommand(Add
                //,obj => {
                //return !string.IsNullOrWhiteSpace(LocalRepository.Title) && System.IO.Directory.Exists(Path);})
                ));
            }
        }

        private RelayCommand resetCommand;
        public RelayCommand ResetCommand
        {
            get
            {
                return resetCommand ?? (resetCommand = new RelayCommand(Reset
                //,obj => {
                //return !string.IsNullOrWhiteSpace(LocalRepository.Title) && System.IO.Directory.Exists(Path);})
                ));
            }
        }

        private void Reset(object obj)
        {
            throw new NotImplementedException();
        }

        private async void Add(object obj)
        {
            //List<string> files = TreeViewConverter.ParseTreeViewItemToFullFilenames(item);
            //await TreeViewConverter.BuildFilesTreeViewItem(AddedFilesTreeView, files.ToArray());

            //LfkClient.Repository.Repository.GetInstance().Add(files);
            //TreeViewConverter.RemoveTreeViewItem(item);
        }

    }
}
