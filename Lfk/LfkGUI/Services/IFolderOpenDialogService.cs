using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LfkGUI.Services
{
    public interface IFolderOpenDialogService
    {
        string  FilePath { get; set; }
        bool    OpenFolderDialog(); 
    }
}
