using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace FileManage.Controls
{
    public class DragThumb:Thumb
    {
        public DragThumb()
        {
            base.DragDelta += new DragDeltaEventHandler(DragThumb_DragDelta);
            base.DragCompleted += new DragCompletedEventHandler(DragThumb_DragCompleted);
        }

        void DragThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            //DesignerItem designerItem = this.DataContext as DesignerItem;
            //DesignerCanvas designer = VisualTreeHelper.GetParent(designerItem) as DesignerCanvas;
            //if (designerItem != null && designer != null && designerItem.IsSelected)
            //{
            //    double minLeft = double.MaxValue;
            //    double minTop = double.MaxValue;

            //    // we only move DesignerItems
            //    var designerItems = from item in designer.SelectedItems
            //                        where item is DesignerItem
            //                        select item;

            //    foreach (DesignerItem item in designerItems)
            //    {
            //        double left = Canvas.GetLeft(item);
            //        double top = Canvas.GetTop(item);

            //        minLeft = double.IsNaN(left) ? 0 : Math.Min(left, minLeft);
            //        minTop = double.IsNaN(top) ? 0 : Math.Min(top, minTop);
            //    }

            //    double deltaHorizontal = Math.Max(-minLeft, e.HorizontalChange);
            //    double deltaVertical = Math.Max(-minTop, e.VerticalChange);

            //    foreach (DesignerItem item in designerItems)
            //    {
            //        double left = Canvas.GetLeft(item);
            //        double top = Canvas.GetTop(item);

            //        if (double.IsNaN(left)) left = 0;
            //        if (double.IsNaN(top)) top = 0;

            //        int widthUnit = (int)((left + deltaHorizontal) /110.0+0.5);
            //        int heightUnit=(int)((top+deltaVertical)/110.0+0.5);
            //        Canvas.SetLeft(item, widthUnit*110);
            //        Canvas.SetTop(item, heightUnit*110);
            //    }
            //    designer.InvalidateMeasure();
            //    e.Handled = true;
            //}
        }

        void DragThumb_DragCompleted(object sender,DragCompletedEventArgs e)
        {
            //double deltaHorizontal = e.HorizontalChange;
            //double deltaVertical = e.VerticalChange;

            //DesignerItem designerItem = this.DataContext as DesignerItem;
            //double left = Canvas.GetLeft(designerItem);
            //double top = Canvas.GetTop(designerItem);
            //DesignerCanvas designer = VisualTreeHelper.GetParent(designerItem) as DesignerCanvas;
            //if (designerItem != null && designer != null && designerItem.IsSelected)
            //{
            //    // we only move DesignerItems
            //    var designerItems = designer.SelectionService.CurrentSelection.OfType<DesignerItem>();
            //    foreach (DesignerItem item in designerItems)
            //    {
            //        //double left = Canvas.GetLeft(item);
            //        //double top = Canvas.GetTop(item);

            //        //if (double.IsNaN(left)) left = 0;
            //        //if (double.IsNaN(top)) top = 0;

            //        //int widthUnit = (int)((left + deltaHorizontal) / 110.0 + 0.5);
            //        //int heightUnit = (int)((top + deltaVertical) / 110.0 + 0.5);
            //        //Canvas.SetLeft(item, widthUnit * 110);
            //        //Canvas.SetTop(item, heightUnit * 110);
            //    }

            //    designer.InvalidateMeasure();
            //    e.Handled = true;
            //}
        }
    }
}
