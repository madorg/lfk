using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;

namespace LfkGUI.ViewModels.Converters
{
    public class TreeViewConverter : MarkupExtension,IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            TreeView treeView = new TreeView();
            ObservableCollection<string> paths = value as ObservableCollection<string>;
            LfkGUI.Utility.TreeViewConverter.BuildFilesTreeViewItem(treeView, paths.ToArray());
            return treeView;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            throw new NotImplementedException();
        }
    }
}
