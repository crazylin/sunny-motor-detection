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
using Gemini.Framework;
using Gemini.Modules.PropertyToolBox.ViewModels;

namespace Gemini.Modules.PropertyToolBox.Views
{
    /// <summary>
    /// PropertyToolboxView.xaml 的交互逻辑
    /// </summary>
    public partial class PropertyToolboxView : UserControl
    {
        //private bool _draggingItem;
        //private Point _mouseStartPosition;
        public PropertyToolboxView()
        {
            InitializeComponent();
        }
        //private void OnListBoxPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //    var listBoxItem = VisualTreeUtility.FindParent<ListBoxItem>(
        //        (DependencyObject)e.OriginalSource);
        //    _draggingItem = listBoxItem != null;

        //    _mouseStartPosition = e.GetPosition(ListBox);
        //}

        //private void OnListBoxMouseMove(object sender, MouseEventArgs e)
        //{
        //    if (!_draggingItem)
        //        return;

        //    // Get the current mouse position
        //    Point mousePosition = e.GetPosition(null);
        //    Vector diff = _mouseStartPosition - mousePosition;

        //    if (e.LeftButton == MouseButtonState.Pressed &&
        //        (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
        //         Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance))
        //    {
        //        var listBoxItem = VisualTreeUtility.FindParent<ListBoxItem>(
        //            (DependencyObject)e.OriginalSource);

        //        if (listBoxItem == null)
        //            return;

        //        var itemViewModel = (PropertyToolboxItemViewModel)ListBox.ItemContainerGenerator.
        //            ItemFromContainer(listBoxItem);

        //        var dragData = new DataObject(PropertyToolboxDragDrop.DataFormat, itemViewModel.Model);
        //        DragDrop.DoDragDrop(listBoxItem, dragData, DragDropEffects.Move);
        //    }
        //}
    }
}
