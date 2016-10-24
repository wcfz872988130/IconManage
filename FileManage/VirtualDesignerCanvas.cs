using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Xml;
using System.Windows.Data;
using System.Diagnostics;
using System.ComponentModel;

namespace FileManage
{
    public partial class VirtualDesignerCanvas : DesignerCanvas,INotifyPropertyChanged
    {
        public List<DesignerItem> virtualalbum = new List<DesignerItem>();
        public List<string> IconPath=new List<string>();
        public DesignerItem rootDesignerItem = new DesignerItem();
        public ShowTreeView virtualCanvasTreeView;
        public event PropertyChangedEventHandler PropertyChanged;
        public string albumname;
        public string AlbumName
        {
            get { return albumname; }
            set 
            {
                albumname = value;
                if(this.PropertyChanged!=null)
                {
                    this.PropertyChanged.Invoke(this,new PropertyChangedEventArgs("AlbumName"));
                }
            }
        }
        protected override void OnDrop(DragEventArgs e)
        {
            base.OnDrop(e);
            Debug.Write("11111111111\n");
            IconPath.Clear();
            DesignerCanvas dragObject = e.Data.GetData(typeof(DesignerCanvas)) as DesignerCanvas;
            Point pp = e.GetPosition(this);
            if (dragObject != null)
            {
                var designerItems = dragObject.SelectionService.CurrentSelection.OfType<DesignerItem>();
                if (GetAlbumPosition(pp) != null)
                {
                    DesignerItem target = GetAlbumPosition(pp);
                    foreach (DesignerItem item in designerItems)
                    {
                        item.Has_Selected = true;
                        Border border=item.Content as Border;
                        border.BorderBrush = Brushes.LightGray;
                        string path = item.realityAddress;
                        IconPath.Add(path);
                    }
                    AddAlbumIcon(target);
                }
                else
                {
                    foreach (DesignerItem item in designerItems)
                    {
                        string path = item.realityAddress;
                        Border border = item.Content as Border;
                        border.BorderBrush = Brushes.LightGray;
                        item.Has_Selected = true;
                        IconPath.Add(path);
                    }
                    AddAlbumIcon(pathStack.virtualalbumpath.Peek());
                    SetVirtualAlbumView(pathStack.virtualalbumpath.Peek());
                }
                dragObject.SelectionService.ClearSelection();
                dragObject.Focus();
            }
            virtualCanvasTreeView.CreateDynIconTreeView();
            e.Handled = true;
        }
        public DesignerItem AddAlbum()
        {
            string Albumname =  albumname;
            HeaderedContentControl headcontent = AddImage.AddAlbum(Albumname);
            DesignerItem newItem = new DesignerItem();
            newItem.canvas_real_album = false;
            newItem.Content = headcontent;
            newItem.Is_Virtual_Folder = true;
            return newItem;
        }

        public void SetVirtualAlbumView(DesignerItem album)
        {
            leftdistance = 20;
            topdistance = 20;
            this.virtualalbum.Clear();
            this.Children.Clear();
            foreach(DesignerItem item in album.my_album)
            {
                Point position = new Point(leftdistance, topdistance);
                DesignerCanvas.SetLeft(item, leftdistance);
                DesignerCanvas.SetTop(item, topdistance);
                virtualalbum.Add(item);
                this.Children.Add(item);                
                leftdistance += 90;
                if (leftdistance >= 630)
                {
                    leftdistance = 20;
                    topdistance += 110;
                }
            }
        }
        public void AddVirtualAlbumView(DesignerItem album)
        {
            Point position = new Point(leftdistance, topdistance);
            DesignerCanvas.SetLeft(album, leftdistance);
            DesignerCanvas.SetTop(album, topdistance);
            this.Children.Add(album);
            leftdistance += 90;
            if (leftdistance >= 630)
            {
                leftdistance = 20;
                topdistance += 110;
            }
        }
        private DesignerItem GetDesignerItem(string pathString)
        {
            if (string.IsNullOrEmpty(pathString))
            {
                return null;
            }
            //加载文件
            string[] sp = pathString.Split('.');
            string name = sp[sp.Length - 1];
            DesignerItem newItem = new DesignerItem();
            newItem.canvas_real_album = false;

            //string[] filesname = sp[sp.Length - 2].Split('\\');
            //string filename = filesname[filesname.Length - 1];
            //AddImage addImg = new AddImage(filename);
            HeaderedContentControl grid = AddImage.AddImageandText(pathString,true);       
            newItem.Content = grid;

            return newItem;
        }
        //private void AddShowIcon()
        //{
        //    leftdistance = 0;
        //    topdistance = 0;
        //    this.Children.Clear();
        //    foreach(string path in IconPath)
        //    {
        //        DesignerItem iitem = GetDesignerItem(path);
        //        virtualalbum.Add(iitem);
        //        rootDesignerItem.my_album.Add(iitem);
        //    }
        //}
        public void CheckOutRepetition(string name,DesignerItem self)
        {
            foreach(DesignerItem item in virtualalbum)
            {
                if (item.Is_Virtual_Folder)
                {
                    HeaderedContentControl it = item.Content as HeaderedContentControl;
                    MyTextBox iit = it.Content as MyTextBox;
                    string itemName = iit.Text;
                    if (itemName == name && item != self)
                    {
                        MessageBox.Show("相册名重复");
                        break;
                    }
                }
            }
        }
        private void AddAlbumIcon(DesignerItem target)
        {
            foreach (string path in IconPath)
            {
                DesignerItem iitem = GetDesignerItem(path);
                string[] str2= path.Split('\\');
                string str1 = str2[str2.Length - 1];
                iitem.realityAddress = str1;
                target.my_album.Add(iitem);
            }
        }
        public DesignerItem GetAlbumPosition(Point position)
        {
            foreach(var item in virtualalbum)
            {
                double top = Canvas.GetTop(item);
                double left = Canvas.GetLeft(item);
                if (position.X > left && position.X < (left + 100)&&position.Y>top&&position.Y<top+100&&item.Is_Virtual_Folder)
                {
                    return item;
                }
            }
            return null;
        }
    }
}
