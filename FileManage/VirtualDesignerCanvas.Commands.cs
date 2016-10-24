using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Xml;
using System.Xml.Linq;
using Microsoft.Win32;


namespace FileManage
{
    public partial class VirtualDesignerCanvas:DesignerCanvas
    {
        public VirtualDesignerCanvas()
        {
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Delete, Delete_Executed));
            MyTextBox.DoTextboxCheck += CheckOutRepetition;
        }
        #region Delete Command

        private void Delete_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            DeleteCurrentSelection();
        }

        //private void Delete_Enabled(object sender, CanExecuteRoutedEventArgs e)
        //{
        //    e.CanExecute = this.SelectionService.CurrentSelection.Count() > 0;
        //}

        #endregion

        private void DeleteCurrentSelection()
        {
            foreach (DesignerItem item in SelectionService.CurrentSelection.OfType<DesignerItem>())
            {
                pathStack.virtualalbumpath.Peek().my_album.Remove(item);
                this.Children.Clear();
                virtualalbum.Clear();
                SetVirtualAlbumView(pathStack.virtualalbumpath.Peek());
                virtualCanvasTreeView.CreateDynIconTreeView();
            }
            SelectionService.ClearSelection();
            UpdateZIndex();
        }
        private void UpdateZIndex()
        {
            List<UIElement> ordered = (from UIElement item in this.Children
                                       orderby Canvas.GetZIndex(item as UIElement)
                                       select item as UIElement).ToList();
            for (int i = 0; i < ordered.Count; i++)
            {
                Canvas.SetZIndex(ordered[i], i);
            }
        }
    }
}
