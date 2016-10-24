using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Security.AccessControl;
using System.Windows.Media.Imaging;
using System.IO;
using System.ComponentModel;

namespace FileManage
{
    public class ShowTreeView
    {
        public ShowTreeView()
        {
            MyTextBox.DoTextboxChange += CreateDynIconTreeView;
        }
        public List<TVItems> listItems = new List<TVItems>();
        public TreeView showTreeView;
        public DesignerItem showTreeRoot;
        public void CreateDynIconTreeView()
        {
            showTreeView.Items.Clear();
            listItems.Clear();
            TreeViewItem Nodes = new TreeViewItem() { Header = CreateTreeViewItem1("MainAlbum") };
            Nodes.Expanded += Node_Expanded;
            Nodes.Collapsed += Node_Collapsed;
            TVItems items = new TVItems()
            {
                ViewItem = Nodes,
                path = showTreeRoot
            };
            listItems.Add(items);
            getDirectories(showTreeRoot, Nodes);
            //getfiles(path, Nodes);
            Nodes.IsExpanded = true;
            showTreeView.Items.Add(Nodes);
        }

        private void getDirectories(DesignerItem albumitem, TreeViewItem aNode)
        {
            foreach (DesignerItem item in albumitem.my_album)
            {                
                if (item.Is_Virtual_Folder)
                {
                    HeaderedContentControl iii=item.Content as HeaderedContentControl;
                    TextBox textbox = iii.Content as TextBox;
                    string name = textbox.Text;
                    TreeViewItem Node = new TreeViewItem()
                    {
                        Header = CreateTreeViewItem1(name)
                    };
                    TVItems items = new TVItems()
                    {
                        ViewItem = Node,
                        path=item
                    };

                    listItems.Add(items);
                    aNode.Items.Add(Node);
                    aNode.Expanded += Node_Expanded;
                    aNode.Collapsed += Node_Collapsed;
                    //getDirectories(item, Node);
                    aNode.IsExpanded = true;
                    getDirectories(item, Node);
                }
                else
                {
                    //TreeViewItem Node = new TreeViewItem() { Header = CreateTreeViewItem2(".png") };
                    HeaderedContentControl iii = item.Content as HeaderedContentControl;
                    HeaderedContentControl headiii = iii.Header as HeaderedContentControl;
                    TextBox commentTextBox = headiii.Content as TextBox;
                    Label textbox = iii.Content as Label;
                    //commentTextBox.IsEnabled = false;
                    string comment = commentTextBox.Text;
                    string name = textbox.Content.ToString();
                    name += ".dds";
                    string labelName = null;
                    if (comment.Equals("添加备注"))
                    {
                        labelName = name;
                    }
                    else
                        labelName = name + "  " + comment;
                    TreeViewItem Node = new TreeViewItem() { Header = CreateTreeViewItem2(name, labelName) };
                    aNode.Items.Add(Node);
                }
            }
        }
        private void getfiles(DesignerItem albums, TreeViewItem aNode)
        {
            foreach (DesignerItem photo in albums.my_album)
            {
                if(!photo.Is_Virtual_Folder)
                {
                    HeaderedContentControl iii = photo.Content as HeaderedContentControl;
                    HeaderedContentControl headiii = iii.Header as HeaderedContentControl;
                    TextBox commentTextBox = headiii.Content as TextBox;
                    Label textbox = iii.Content as Label;
                    textbox.IsEnabled = false;
                    string comment = commentTextBox.Text;
                    string name = textbox.Content.ToString();
                    name += ".dds";
                    string labelName=null;
                    if(comment.Equals("添加备注"))
                    {
                        labelName = name;
                    }
                    else
                        labelName = name + "  " + comment;
                    TreeViewItem Node = new TreeViewItem() { Header = CreateTreeViewItem2(name,labelName) };
                    aNode.Items.Add(Node);
                }
            }
        }

        void Node_Collapsed(object sender, RoutedEventArgs e)
        {
            TreeViewItem item = (TreeViewItem)sender;
            var stackPanel = item.Header as StackPanel;
            var image = stackPanel.Children[0] as Image;
            image.Source = new BitmapImage(new Uri("pack://application:,,,/Icons/FolderOff.png"));
            e.Handled = true;
        }

        void Node_Expanded(object sender, RoutedEventArgs e)
        {
            TreeViewItem item = (TreeViewItem)sender;
            var stackPanel = item.Header as StackPanel;
            var image = stackPanel.Children[0] as Image;
            image.Source = new BitmapImage(new Uri("pack://application:,,,/Icons/FolderOpen.png"));
            e.Handled = true;
        }
        //创建文件夹类型ITEM
        private object CreateTreeViewItem1(string AValue)
        {
            StackPanel stkPanl = new StackPanel()
            {
                Height = 24,
                Orientation = Orientation.Horizontal,
                VerticalAlignment = VerticalAlignment.Center
            };
            Image img = new Image()
            {
                Width = 18,
                Height = 18,
                Source = new BitmapImage(new Uri("pack://application:,,,/Icons/FolderOff.png"))
            };

            stkPanl.Children.Add(img);
            Label lblHeader = new Label()
            {
                Content = AValue,
                VerticalAlignment = VerticalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center
            };
            stkPanl.Children.Add(lblHeader);
            return stkPanl;
        }
        //创建文件类型ITEM
        private object CreateTreeViewItem2(string AValue, string labelName)
        {
            StackPanel stkPanl = new StackPanel()
            {
                Height = 24,
                Orientation = Orientation.Horizontal,
                VerticalAlignment = VerticalAlignment.Center
            };
            Image img = new Image()
            {
                Width = 16,
                Height = 16,
                Source = new BitmapImage(new Uri(getPath(AValue)))
            };

            stkPanl.Children.Add(img);
            Label lblHeader = new Label()
            {
                Content = labelName,
                VerticalAlignment = VerticalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center
            };
            stkPanl.Children.Add(lblHeader);
            return stkPanl;
        }

        string getPath(string file)
        {
            string format = Path.GetExtension(file);//获取文件格式
            switch (format)
            {
                case ".txt":
                    return "pack://application:,,,/Icons/txt.png";
                case ".exe":
                    return "pack://application:,,,/Icons/exe.png";
                case ".pdf":
                    return "pack://application:,,,/Icons/pdf.png";
                case ".mp3":
                case ".wav":
                case ".au":
                case ".midi":
                case ".wma":
                case ".aac":
                case ".ape":
                case ".cda":
                case ".aiff":
                    return "pack://application:,,,/Icons/music.png";
                case ".avi":
                case ".nvai":
                case ".mp4":
                case ".rmvb":
                case ".mpg":
                    return "pack://application:,,,/Icons/video.png";
                case ".zip":
                case ".iso":
                case ".rar":
                    return "pack://application:,,,/Icons/zip.png";
                case ".docx":
                case ".doc":
                    return "pack://application:,,,/Icons/docx.png";
                case ".xslx":
                case ".xsl":
                    return "pack://application:,,,/Icons/excel.png";
                case ".PNG":
                case ".JPG":
                case ".png":
                case ".jpg":
                case ".ico":
                case ".dds":
                    return "pack://application:,,,/Icons/image.png";
                case ".cs":
                case ".sln":
                case ".xaml":
                    return "pack://application:,,,/Icons/code_file.ico";
                case ".dll":
                    return "pack://application:,,,/Icons/dll.png";
                case ".folder":
                    return "pack://application:,,,/Icons/FolderOpen.png";
                default:
                    return "pack://application:,,,/Icons/unknown.png";
            }
        }
    }
}
