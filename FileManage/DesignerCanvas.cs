using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Xml;
using System.Windows.Data;
using System.Diagnostics;


namespace FileManage
{
    public class DesignerCanvas : Canvas
    {
        // start point of the rubberband drag operation
        private Point? rubberbandSelectionStartPoint = null;
        public Dictionary<DesignerItem, string> IconDictionary = new Dictionary<DesignerItem, string>();
        public Dictionary<DesignerItem, string> albumDictionary = new Dictionary<DesignerItem, string>();
        private string name = null;
        private string workspacePath = null;
        public int id = 0;
        public int folder_texture_id = 0;
        public int folder_sequence = 0;
        public int leftdistance = 20;
        public int topdistance = 20;

        /// <summary>
        //private Border border;
        //private Expander expander;
        /// </summary>
        /// <param name="Name"></param>
        private void GetNodeName(string Name)
        {
            name = Name;
        }

        private SelectionService selectionService;
        internal SelectionService SelectionService
        {
            get
            {
                if (selectionService == null)
                    selectionService = new SelectionService(this);
                return selectionService;
            }
        }
        //public DesignerCanvas()
        //{
        //    //ToolboxItem.transname += GetNodeName;
        //    this.AllowDrop = true;
        //}

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            base.OnMouseDown(e);
            if (e.Source == this)
            {
                // in case that this click is the start for a 
                // drag operation we cache the start point
                this.rubberbandSelectionStartPoint = new Point?(e.GetPosition(this));
                // if you click directly on the canvas all 
                // selected items are 'de-selected'
                SelectionService.ClearSelection();
                Focus();
                e.Handled = true;
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (e.LeftButton != MouseButtonState.Pressed)
                this.rubberbandSelectionStartPoint = null;

            if (this.rubberbandSelectionStartPoint.HasValue)
            {

                AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(this);
                if (adornerLayer != null)
                {
                    RubberbandAdorner adorner = new RubberbandAdorner(this, rubberbandSelectionStartPoint);
                    if (adorner != null)
                    {
                        adornerLayer.Add(adorner);
                    }
                }
            }
            e.Handled = true;
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            base.OnMouseUp(e);
        }

        //protected override void OnDrop(DragEventArgs e)
        //{
        //}

        protected override Size MeasureOverride(Size constraint)
        {
            Size size = new Size();
            foreach (UIElement element in base.Children)
            {
                double left = Canvas.GetLeft(element);
                double top = Canvas.GetTop(element);
                left = double.IsNaN(left) ? 0 : left;
                top = double.IsNaN(top) ? 0 : top;
                //measure desired size for each child
                element.Measure(constraint);
                Size desiredSize = element.DesiredSize;
                if (!double.IsNaN(desiredSize.Width) && !double.IsNaN(desiredSize.Height))
                {
                    size.Width = Math.Max(size.Width, left + desiredSize.Width);
                    size.Height = Math.Max(size.Height, top + desiredSize.Height);
                }
            }
            // add margin 
            size.Width += 10;
            size.Height += 10;
            return size;
        }
        public string AddRealityCanvasView(string path)
        {
            for (int i = this.Children.Count-1; i >= 0;--i )
            {
                DesignerItem it = this.Children[i] as DesignerItem;
                Border iit = it.Content as Border;
                HeaderedContentControl Headiit = iit.Child as HeaderedContentControl;
                Label iiit = Headiit.Content as Label;
                name = iiit.Content as string;
                int result=0;
                if (int.TryParse(name, out result))
                {
                    name = (result + 1).ToString();
                    break;
                }
            }

            HeaderedContentControl headcontent = AddImage.AddImageandText(path,name,false);
            DesignerItem newItem = new DesignerItem();

            Border border = new Border();
            border.BorderBrush = Brushes.LightGray;
            Thickness borderthick = new Thickness(2);
            border.BorderThickness = borderthick;
            CornerRadius bordercornerRadius = new CornerRadius(10);
            border.CornerRadius = bordercornerRadius;
            border.Background = new SolidColorBrush(Colors.Transparent);
            Thickness innerpadding = new Thickness(0, 5, 0, 0);
            border.Height = newItem.Height;
            border.Margin = innerpadding;
            border.Child = headcontent;
            newItem.Content = border;
            //newItem.Padding = new Thickness(3,3,3,3);
            //newItem.Background = new SolidColorBrush(Colors.Black);
            newItem.realityAddress = path;
            newItem.Has_Selected = true;
            Point position = new Point(leftdistance, topdistance);
            DesignerCanvas.SetLeft(newItem, leftdistance);
            DesignerCanvas.SetTop(newItem, topdistance);
            this.Children.Add(newItem);
            leftdistance += 90;
            if (leftdistance >= 810)
            {
                leftdistance = 20;
                topdistance += 110;
            }
            return name;
        }
        public void SetRealityCanvasView(string path)
        {
            workspacePath = path;
            this.Children.Clear();
            leftdistance = 20;
            topdistance = 20;
            if (string.IsNullOrEmpty(workspacePath))
            {
                return;
            }
            //MyDesignerCanvas.Children.Clear();
            //读取我选择的路径下的文件和目录
            //string[] directories = Directory.GetDirectories(workspacePath);//文件夹集合
            string[] files = new string[100000];
            
            files = Directory.GetFiles(workspacePath, "*.dds");
            Array.Sort(files,new CustomComparer());
            //加载文件夹
            //foreach (var item in directories)
            //{
            //    string[] directorysname = item.Split('\\');
            //    string directoryname = directorysname[directorysname.Length - 1];
            //    HeaderedContentControl headcontent = AddImage.AddAlbum(directoryname);
            //    DesignerItem newItem = new DesignerItem();
            //    newItem.Content = headcontent;
            //    newItem.Is_folder = true;
            //    newItem.realityAddress = item;
            //    Point position = new Point(leftdistance, topdistance);
            //    DesignerCanvas.SetLeft(newItem, leftdistance);
            //    DesignerCanvas.SetTop(newItem, topdistance);
            //    this.Children.Add(newItem);
            //    leftdistance += 90;
            //    if (leftdistance >= 1080)
            //    {
            //        leftdistance = 0;
            //        topdistance += 110;
            //    }
            //}
            //加载文件
            for (int i = 0; i < files.Length;++i )
            {
                string pathString = files[i];
                string[] sp = pathString.Split('.');
                string name = sp[sp.Length - 1];
                HeaderedContentControl grid = AddImage.AddImageandText(pathString, false);
                DesignerItem newItem = new DesignerItem();

                Border border = new Border();
                border.BorderBrush = Brushes.Red;
                Thickness borderthick = new Thickness(2);
                border.BorderThickness = borderthick;
                CornerRadius bordercornerRadius = new CornerRadius(10);
                border.CornerRadius = bordercornerRadius;
                border.Background = new SolidColorBrush(Colors.Transparent);
                Thickness innerpadding = new Thickness(0, 5, 0, 0);
                border.Height = newItem.Height;
                border.Margin = innerpadding;
                border.Child = grid;

                newItem.Content = border;
                newItem.realityAddress = pathString;
                //newItem.Padding = new Thickness(3, 3, 3, 3);
                //newItem.Background = new SolidColorBrush(Colors.Black);
                Point position = new Point(leftdistance, topdistance);
                DesignerCanvas.SetLeft(newItem, leftdistance);
                DesignerCanvas.SetTop(newItem, topdistance);
                this.Children.Add(newItem);
                leftdistance += 90;
                if (leftdistance >= 810)
                {
                    leftdistance = 20;
                    topdistance += 110;
                }
            }
        }
    }
}

