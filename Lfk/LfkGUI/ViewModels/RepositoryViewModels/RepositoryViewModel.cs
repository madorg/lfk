using LfkGUI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LfkGUI.ViewModels.RepositoryViewModels
{
    public class RepositoryViewModel : BasicViewModel
    {
        private WindowsService windowsService;
        private DialogService dialogService;

        public RepositoryViewModel(WindowsService windowsService)
        {
            dialogService = new DialogService();
            this.windowsService = windowsService;
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

        private void Add(object obj)
        {
            throw new NotImplementedException();
        }

    }
}