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
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
		string path = AppDomain.CurrentDomain.BaseDirectory + "\\path.txt";
        //string folder_texture_id_path = AppDomain.CurrentDomain.BaseDirectory + "\\folder_texture_id.txt";
        //string workspacePath=AppDomain.CurrentDomain.BaseDirectory+"MyDir";
        //bool Isload = false;
        ShowTreeView showtreeview=new ShowTreeView();
        DirectoryInfo di = null;
        VirtualTextPath virtualTextPath;
        public List<TVItems> listItems = new List<TVItems>();    
        public List<LVItem> listLVItem = new List<LVItem>();
        string sformat = string.Empty;
        int folder_texture_id = 0;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
			foreach(var item in Enum.GetValues(typeof(FileFormat))) {
				string strName = Enum.GetName(typeof(FileFormat), item);
			}
            virtualTextPath = new VirtualTextPath(txtVirtualPath);
            MyVirtualDesignerCanvas.virtualCanvasTreeView = showtreeview;
            showtreeview .showTreeView= tvMain;
            showtreeview.showTreeRoot = MyVirtualDesignerCanvas.rootDesignerItem;
            MyVirtualDesignerCanvas.rootDesignerItem.Is_Virtual_Folder = true;
            pathStack.virtualalbumpath.Push(MyVirtualDesignerCanvas.rootDesignerItem);
            string ApplicationPath = Directory.GetCurrentDirectory();
            //pathStack.albumpath.Push(workspacePath);
			try {
				string filepath = string.Empty;//文件下的路径
				if(File.Exists(path)) {
					//如果文件不为空
					StreamReader sr = new StreamReader(path, Encoding.GetEncoding("utf-8")); //读取数据
					filepath = sr.ReadToEnd().ToString().Trim();
					sr.Close();//关闭文件流
					txtPath.Text = filepath;
                    //CreateDynIconTreeView(filepath);
                    //MyDesignerCanvas.SetRealityCanvasView(filepath);
                    //Isload = true;
				}
			}
			catch {
				MessageBox.Show("读取文件中的数据失败");
				//Environment.Exit(0);//关闭程序；
			}
        }
        private void button_path_Click(object sender, RoutedEventArgs e)
        {
			try {
                System.Windows.Forms.FolderBrowserDialog dialog = new System.Windows.Forms.FolderBrowserDialog();												  
				if(dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
					txtPath.Text = dialog.SelectedPath;
                    //如果路径不为空的话，就将路径指定到文件中
                    if (!string.IsNullOrEmpty(txtPath.Text))
                    {
                        if (!Directory.Exists(txtPath.Text))
                        {
                            MessageBox.Show("路径不存在");
                            return;
                        }
                        if (File.Exists(path))
                        {
                            //如果文件存在，则删除该文件
                            File.Delete(path);
                        }
                        if(!txtPath.Text.Contains("icon"))
                        {
                            MessageBox.Show("选择路径不包含icon");
                            return;
                        }
                        FileStream fs = new FileStream(path, FileMode.Create);//创建文件
                        StreamWriter sw = new StreamWriter(fs);
                        sw.WriteLine(txtPath.Text.Trim());
                        sw.Close();
                        fs.Close();
                        MyDesignerCanvas.SetRealityCanvasView(txtPath.Text);
                        Virtualbutton_import_Click(txtPath.Text);
				}
                }
			}
			catch(Exception ex) {
                MessageBox.Show("失败失败：" + ex.Message);
			}
        }

        private void Virtualbutton_import_Click(string path)
        {
            MyVirtualDesignerCanvas.rootDesignerItem.my_album.Clear();
            MyVirtualDesignerCanvas.virtualalbum.Clear();
            pathStack.virtualalbumpath.Clear();
            MyVirtualDesignerCanvas.Children.Clear();
            tvMain.Items.Clear();
            showtreeview.listItems.Clear();
            XMLParse xmlparse = new XMLParse(path);
            xmlparse.xmlFileName = path;
            xmlparse.xmlPath = AppDomain.CurrentDomain.BaseDirectory + "Xml";
          
            if (xmlparse.JudgeParse())
            {
                MyVirtualDesignerCanvas.rootDesignerItem = xmlparse.parseXml();
                pathStack.virtualalbumpath.Clear();
                pathStack.virtualalbumpath.Push(MyVirtualDesignerCanvas.rootDesignerItem);
                MyVirtualDesignerCanvas.SetVirtualAlbumView(MyVirtualDesignerCanvas.rootDesignerItem);
                showtreeview.showTreeRoot = MyVirtualDesignerCanvas.rootDesignerItem;
                showtreeview.CreateDynIconTreeView();
            }
            else
            {
                pathStack.virtualalbumpath.Push(MyVirtualDesignerCanvas.rootDesignerItem);
            }
        }

        void ImportIcon(string pathString, string name)
        {
            name += ".dds";
            DirectoryInfo n = new DirectoryInfo(pathString);
            System.IO.File.SetAttributes(pathString, System.IO.FileAttributes.Normal);
            File.Copy(pathString, Path.Combine(txtPath.Text, name), true);                
        }

        long GetDicrectoryLenght(string path)
        {
            if(!Directory.Exists(path)) {
                return 0;
            }
            DirectoryInfo info = new DirectoryInfo(path);
            long lenght = 0;
            foreach(var fi in info.GetFiles()) {
                lenght += fi.Length;
            }
            DirectoryInfo[] dis = info.GetDirectories();
            if(dis.Length > 0) {
                for(int i = 0; i < dis.Length; i++) {
                    lenght += GetDicrectoryLenght(dis[i].FullName);
                }
            }
            return lenght;
        }

        private void tvMain_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            TreeViewItem item = (TreeViewItem)e.NewValue;
            TVItems tvItems = showtreeview.listItems.Find(delegate (TVItems tv) {
                return (tv.ViewItem.Equals(item));
            });
            if(tvItems == null) {
                return;
            }
            pathStack.virtualalbumpath.Clear();
            pathStack.virtualalbumpath.Push(MyVirtualDesignerCanvas.rootDesignerItem);
            GetParentPath(tvItems.path);
            //pathStack.virtualalbumpath.Push(tvItems.path);
            MyVirtualDesignerCanvas.virtualalbum.Clear();
            MyVirtualDesignerCanvas.Children.Clear();
            MyVirtualDesignerCanvas.IconDictionary.Clear();
            MyVirtualDesignerCanvas.id = 0;
            MyVirtualDesignerCanvas.SetVirtualAlbumView(tvItems.path);
            virtualTextPath.TraverseStack();
            //ImportIcon(tvItems.path);
            //MyDesignerCanvas.SetRealityCanvasView(tvItems.path);
            //SetListView(sformat);            
        }
        private void GetParentPath(DesignerItem item)
        {
            List<DesignerItem> tempItem = new List<DesignerItem>();
            while (item != MyVirtualDesignerCanvas.rootDesignerItem)
            {
                tempItem.Add(item);
                item = item.myParentDesignerItem;
            }
            for (int i = tempItem.Count - 1; i >= 0; --i)
            {
                pathStack.virtualalbumpath.Push(tempItem[i]);
            }
        }
        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            InputWindows popUp = new InputWindows(MyVirtualDesignerCanvas);
            popUp.Owner = Application.Current.MainWindow;
            popUp.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            popUp.ShowDialog();
            
            MyVirtualDesignerCanvas.Children.Clear();
            DesignerItem item = MyVirtualDesignerCanvas.AddAlbum();
            pathStack.virtualalbumpath.Peek().MyAlbumID++;
            MyVirtualDesignerCanvas.virtualalbum.Add(item);
            pathStack.virtualalbumpath.Peek().my_album.Add(item);
            item.myParentDesignerItem = pathStack.virtualalbumpath.Peek();
            MyVirtualDesignerCanvas.CheckOutRepetition(MyVirtualDesignerCanvas.albumname,item);
            MyVirtualDesignerCanvas.SetVirtualAlbumView(pathStack.virtualalbumpath.Peek());
            showtreeview.CreateDynIconTreeView();
        }
        private void Format_Click(object sender, RoutedEventArgs e)
        {
            if (pathStack.albumpath.Count <= 1)
                return;
            pathStack.albumpath.Pop();
            MyDesignerCanvas.Children.Clear();
            MyDesignerCanvas.IconDictionary.Clear();
            MyDesignerCanvas.leftdistance = 0;
            MyDesignerCanvas.topdistance = 0;
            MyDesignerCanvas.id = 0;
            MyDesignerCanvas.SetRealityCanvasView(pathStack.albumpath.Peek());
        }

        private void VirtualFormat_Click(object sender, RoutedEventArgs e)
        {
            if (pathStack.virtualalbumpath.Count > 1)
                pathStack.virtualalbumpath.Pop();
            virtualTextPath.TraverseStack();
            MyVirtualDesignerCanvas.Children.Clear();
            MyVirtualDesignerCanvas.virtualalbum.Clear();
            MyVirtualDesignerCanvas.id = 0;
            MyVirtualDesignerCanvas.SetVirtualAlbumView(pathStack.virtualalbumpath.Peek());
        }

        private void button_save_Click(object sender, RoutedEventArgs e)
        {
            XMLParse xmlparse = new XMLParse();
            xmlparse.xmlFileName = txtPath.Text;
            xmlparse.xmlPath = AppDomain.CurrentDomain.BaseDirectory + "Xml";
            xmlparse.writeXml(MyVirtualDesignerCanvas.rootDesignerItem);
        }

        private void button_import_picture_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Windows.Forms.OpenFileDialog openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
                openFileDialog1.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
                openFileDialog1.Filter = "ddsfiles (*.dds)|*.dds";
                openFileDialog1.Multiselect = true;
                openFileDialog1.Title = "My Image Browser";
                openFileDialog1.FilterIndex = 2;
                openFileDialog1.RestoreDirectory = true;
                if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    foreach (string ddsPath in openFileDialog1.FileNames)
                    {
                        string name = MyDesignerCanvas.AddRealityCanvasView(ddsPath);
                        HeaderedContentControl tempPic = AddImage.AddImageandText(ddsPath, name, true);
                        DesignerItem newItem = new DesignerItem();
                        newItem.canvas_real_album = false;
                        newItem.Content = tempPic;
                        string str1 = txtPath.Text;
                        str1 = str1.Replace("tw2", "*");
                        string[] str2 = str1.Split('*');
                        ImportIcon(ddsPath,name);
                        str1 = str2[str2.Length - 1];
                        newItem.realityAddress = name + ".dds";
                        MyVirtualDesignerCanvas.virtualalbum.Add(newItem);
                        pathStack.virtualalbumpath.Peek().my_album.Add(newItem);
                        MyVirtualDesignerCanvas.AddVirtualAlbumView(newItem);
                    }
                    //showtreeview.CreateDynIconTreeView();
                }
            }
            catch (Exception ef)
            {
                MessageBox.Show("导入图片失败：" + ef.Message);
            }  
        }
    }
}
