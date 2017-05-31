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

    }
}