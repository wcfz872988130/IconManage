using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows;
using System.Windows.Media;
using System.Text;
using System.Xml;
using System.IO;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Data;

namespace FileManage
{
    public class csvParse
    {
        public static string xmlFilePath;
        public static string xmlFileName;
        public static string xmlPath;
        private static string assembleName;
        public static Dictionary<string,DesignerItem> pair_name_designerItem = new Dictionary<string,DesignerItem>();
        public static void SaveCSV(DesignerItem root)
        {
            try
            {
                xmlFileName = xmlFileName.Replace("icon", "*");
                string[] tempName = xmlFileName.Split('*');
                string[] xmlName = xmlFileName.Split('\\');
                assembleName = xmlName[xmlName.Length - 2];
                xmlFileName = xmlPath + "\\" + assembleName + ".csv";
                if (File.Exists(xmlFileName))
                {
                    File.Delete(xmlFileName);
                }
                System.IO.FileStream fs = new System.IO.FileStream(xmlFileName, System.IO.FileMode.Create,
        System.IO.FileAccess.Write);
                System.IO.StreamWriter sw = new System.IO.StreamWriter(fs, System.Text.Encoding.UTF8);
                SaveAlbum(root, sw);
                sw.Close();
                fs.Close();
                MessageBox.Show("保存成功");
            }
            catch(Exception e) 
            {
                MessageBox.Show("保存失败:" + e.Message);
            }
        }
        public static void SaveAlbum(DesignerItem root, System.IO.StreamWriter sw)
        {
            for (int i = 0; i < root.my_album.Count;++i )
            {               
                HeaderedContentControl iit = root.my_album[i].Content as HeaderedContentControl;
                if (root.my_album[i].Is_Virtual_Folder)
                {
                    //pair_name_designerItem[name] = root.my_album[i];
                    if(root.my_album[i].my_album.Count>0)
                    {
                        SaveAlbum(root.my_album[i],sw);
                    }
                }
                else
                {
                    string data = "";
                    string uup = root.my_album[i].realityAddress;
                    HeaderedContentControl iiit = iit.Header as HeaderedContentControl;
                    MyTextBox textbox = iiit.Content as MyTextBox;
                    string comment = textbox.Text;
                    data += uup+","+comment+",";
                    Stack<string> AlbumName = new Stack<string>();
                    DesignerItem tempRoot = root;
                    while (tempRoot.myParentDesignerItem != null)
                    {
                        HeaderedContentControl newalbumiit = tempRoot.Content as HeaderedContentControl;
                        MyTextBox newiitextbox = newalbumiit.Content as MyTextBox;
                        string name = newiitextbox.Text;
                        AlbumName.Push(name);
                        tempRoot = tempRoot.myParentDesignerItem;
                    }
                    while(AlbumName.Count!=0)
                    {
                        data += AlbumName.Peek() + ",";
                        AlbumName.Pop();
                    }
                    sw.WriteLine(data);
                }
            }
            return;
        }
        public static DesignerItem OpenCSV()
        {
            DesignerItem MainAlbum = new DesignerItem();
            string tName = xmlFileName.Replace("icon", "*");
            string[] tempName = tName.Split('*');
            string[] xmlName = xmlFileName.Split('\\');
            assembleName = xmlName[xmlName.Length - 2];
            xmlFileName = xmlPath + "\\" + assembleName + ".csv";

            System.Text.Encoding encoding = GetType(xmlFileName); //Encoding.ASCII;//
            //DataTable dt = new DataTable();
            System.IO.FileStream fs = new System.IO.FileStream(xmlFileName, System.IO.FileMode.Open, 
            System.IO.FileAccess.Read);

            System.IO.StreamReader sr = new System.IO.StreamReader(fs, encoding);

            //记录每次读取的一行记录
            string strLine = "";
            //记录每行记录中的各字段内容
            string[] aryLine = null;
            //string[] tableHead = null;
            //标示列数
            int columnCount = 0;
            //标示是否是读取的第一行
            //bool IsFirst = true;
            //逐行读取CSV中的数据
            //int csvLine = sr.ReadToEnd().Split('\n').Length;

            while ((strLine = sr.ReadLine()) != null)
            {
                aryLine = strLine.Split(',');
                
                columnCount = aryLine.Length;

                DesignerItem newItem = null;
                for (int j = 2; j < columnCount; ++j)
                {
                    string albumName = aryLine[j];
                    if(aryLine[2]=="")
                    {
                        newItem = MainAlbum;
                        break;
                    }
                    if(albumName=="")
                    {
                        break;
                    }
                    if (pair_name_designerItem.ContainsKey(albumName))
                    {
                        newItem = pair_name_designerItem[albumName];
                        continue;
                    }
                    else
                    {
                        HeaderedContentControl headcontent = AddImage.AddAlbum(albumName);
                        DesignerItem item = new DesignerItem();
                        item.Content = headcontent;
                        item.Is_Virtual_Folder = true;
                        item.canvas_real_album = false;
                        pair_name_designerItem[albumName] = item;
                        if (newItem != null)
                        {
                            newItem.my_album.Add(item);
                            item.myParentDesignerItem = newItem;
                        }
                        newItem = item;
                    }
                }
                string comment = aryLine[1];
                string IconName = aryLine[0];
                string finalName = xmlFilePath + "\\" + IconName;
                if (File.Exists(finalName))
                {
                    HeaderedContentControl ddscontent = AddImage.AddImageandText(finalName, true, comment);
                    DesignerItem item = new DesignerItem();
                    item.realityAddress = IconName;
                    item.Content = ddscontent;
                    item.canvas_real_album = false;
                    newItem.my_album.Add(item);
                }
            }
            sr.Close();
            fs.Close();
            
            foreach (KeyValuePair<string, DesignerItem> pair in pair_name_designerItem)
            {
                if (pair.Value.myParentDesignerItem == null)
                {
                    MainAlbum.my_album.Add(pair.Value);
                    pair.Value.myParentDesignerItem = MainAlbum;
                }
            }
            return MainAlbum;
        }
        public static bool JudgeParse()
        {
            string tName = xmlFileName.Replace("icon", "*");
            string[] tempName = tName.Split('*');
            string[] xmlName = xmlFileName.Split('\\');
            assembleName = xmlName[xmlName.Length - 2];
            string xml_Path = xmlPath + "\\" + assembleName + ".csv";
            if (File.Exists(xml_Path))
            {
                return true;
            }
            return false;
        }

        public static System.Text.Encoding GetType(string FILE_NAME)
        {
            System.IO.FileStream fs = new System.IO.FileStream(FILE_NAME, System.IO.FileMode.Open,
                System.IO.FileAccess.Read);
            System.Text.Encoding r = GetType(fs);
            fs.Close();
            return r;
        }

        /// 通过给定的文件流，判断文件的编码类型
        /// <param name="fs">文件流</param>
        /// <returns>文件的编码类型</returns>
        public static System.Text.Encoding GetType(System.IO.FileStream fs)
        {
            byte[] Unicode = new byte[] { 0xFF, 0xFE, 0x41 };
            byte[] UnicodeBIG = new byte[] { 0xFE, 0xFF, 0x00 };
            byte[] UTF8 = new byte[] { 0xEF, 0xBB, 0xBF }; //带BOM
            System.Text.Encoding reVal = System.Text.Encoding.Default;

            System.IO.BinaryReader r = new System.IO.BinaryReader(fs, System.Text.Encoding.Default);
            int i;
            int.TryParse(fs.Length.ToString(), out i);
            byte[] ss = r.ReadBytes(i);
            if (IsUTF8Bytes(ss) || (ss[0] == 0xEF && ss[1] == 0xBB && ss[2] == 0xBF))
            {
                reVal = System.Text.Encoding.UTF8;
            }
            else if (ss[0] == 0xFE && ss[1] == 0xFF && ss[2] == 0x00)
            {
                reVal = System.Text.Encoding.BigEndianUnicode;
            }
            else if (ss[0] == 0xFF && ss[1] == 0xFE && ss[2] == 0x41)
            {
                reVal = System.Text.Encoding.Unicode;
            }
            r.Close();
            return reVal;
        }

        /// 判断是否是不带 BOM 的 UTF8 格式
        /// <param name="data"></param>
        /// <returns></returns>
        private static bool IsUTF8Bytes(byte[] data)
        {
            int charByteCounter = 1;　 //计算当前正分析的字符应还有的字节数
            byte curByte; //当前分析的字节.
            for (int i = 0; i < data.Length; i++)
            {
                curByte = data[i];
                if (charByteCounter == 1)
                {
                    if (curByte >= 0x80)
                    {
                        //判断当前
                        while (((curByte <<= 1) & 0x80) != 0)
                        {
                            charByteCounter++;
                        }
                        //标记位首位若为非0 则至少以2个1开始 如:110XXXXX...........1111110X　
                        if (charByteCounter == 1 || charByteCounter > 6)
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    //若是UTF-8 此时第一位必须为1
                    if ((curByte & 0xC0) != 0x80)
                    {
                        return false;
                    }
                    charByteCounter--;
                }
            }
            if (charByteCounter > 1)
            {
                throw new Exception("非预期的byte格式");
            }
            return true;
        }
    }
}

