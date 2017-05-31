using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LfkGUI.ViewModels.RepositoryViewModels;
using LfkGUI.Services;

namespace LfkGUI.Views.RepositoryViews
{
    /// <summary>
    /// Логика взаимодействия для AddCommandPage.xaml
    /// </summary>
    public partial class AddCommandPage : Page
    {
        Point startDragPoint;
        bool isDrag = false;

        public AddCommandPage()
        {
            InitializeComponent();
            this.DataContext = new RepositoryAddCommandViewModel(new WindowsService(App.Current.MainWindow));
        }

        private void FilesTreeView_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            startDragPoint = e.GetPosition(null);
        }

        private void StartDrag(object sender, MouseEventArgs e)
        {
            TreeView treeView = sender as TreeView;
            treeView.AllowDrop = false;
            isDrag = true;
            if (treeView != null && e.LeftButton == MouseButtonState.Pressed)
            {
                try
                {
                    DataObject data = new DataObject(treeView.SelectedItem);
                    DragDrop.DoDragDrop(treeView, data, DragDropEffects.Copy);
                }
                catch (Exception)
                {

                }
            }
            isDrag = false;
            treeView.AllowDrop = true;
        }

        private void FilesTreeView_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed ||
              e.RightButton == MouseButtonState.Pressed && !isDrag)
            {
                Point position = e.GetPosition(null);
                if (Math.Abs(position.X - startDragPoint.X) >
                        SystemParameters.MinimumHorizontalDragDistance ||
                    Math.Abs(position.Y - startDragPoint.Y) >
                        SystemParameters.MinimumVerticalDragDistance)
                {
                    StartDrag(sender, e);
                }
            }
        }
    }
}
