using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LfkGUI.Services
{
    class FolderOpenDialogService
    {
        public string FilePath { get; set; }

        public bool OpenFolderDialog()
        {
            bool rc = false;
            System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();
            fbd.RootFolder = Environment.SpecialFolder.MyComputer;
            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                FilePath = fbd.SelectedPath;
                rc = true;
            }
            return rc;
        }
    }
}
