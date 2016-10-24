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

namespace FileManage
{
    class XMLParse:INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public string xmlFilePath;
        public string xmlFileName;
        public string xmlPath;
        private string assembleName;
        public XMLParse(string Path)
        {
            xmlFilePath=Path;
        }
        public XMLParse()
        { }
        public string XMLFileName
        {
            get { return xmlFileName; }
            set
            {
                xmlFileName = value;
                if(this.PropertyChanged!=null)
                {
                    this.PropertyChanged.Invoke(this,new PropertyChangedEventArgs("XMLFileName"));
                }
            }
        }
        private void parsenode(XmlNode node,DesignerItem parentItem)
        {
            XmlNodeList nodeList = node.SelectNodes("Album");
            string str1 = xmlFilePath.Replace("tw2","*");
            string[] str2 = str1.Split('*');
            string prename = str2[0];
            if (nodeList.Count > 0)
            { 
                foreach(XmlNode childNode in nodeList)
                {
                    string albumName = childNode.Attributes["album"].Value.ToString();
                    HeaderedContentControl headcontent = AddImage.AddAlbum(albumName);
                    DesignerItem item = new DesignerItem();
                    item.Content = headcontent;
                    item.Is_Virtual_Folder = true;
                    item.canvas_real_album = false;
                    parentItem.my_album.Add(item);
                    item.myParentDesignerItem = parentItem;
                    parsenode(childNode,item);
                }
            }
            XmlNodeList ddsList = node.SelectNodes("DDS");
            
            if (ddsList.Count > 0)
            {
                foreach(XmlNode ddschildnode in ddsList)
                {
                    string comment = ddschildnode.Attributes["comment"].Value.ToString();
                    string IconName = ddschildnode.Attributes["filename"].Value.ToString();
                    string finalName = xmlFilePath +"\\"+ IconName;
                    if(File.Exists(finalName))
                    {
                        HeaderedContentControl ddscontent = AddImage.AddImageandText(finalName, true, comment);
                        DesignerItem item = new DesignerItem();
                        item.realityAddress = IconName;
                        item.Content = ddscontent;
                        item.canvas_real_album = false;
                        parentItem.my_album.Add(item);
                    }
                }
            }
            return;
        }
        public DesignerItem parseXml()
        {
            XmlDocument doc = new XmlDocument();
            string tName = xmlFileName.Replace("icon", "*");
            string[] tempName = tName.Split('*');
            string[] xmlName = xmlFileName.Split('\\');
            assembleName = xmlName[xmlName.Length - 2];
            xmlFileName = xmlPath + "\\" + assembleName + ".xml";
            doc.Load(xmlFileName);
            XmlNode MainNode = doc.SelectSingleNode("MainAlbum");
            DesignerItem MainAlbum = new DesignerItem();
            parsenode(MainNode, MainAlbum);
            return MainAlbum;
        }
        public bool JudgeParse()
        {
            string tName = xmlFileName.Replace("icon", "*");
            string[] tempName = tName.Split('*');
            string[] xmlName = xmlFileName.Split('\\');
            assembleName = xmlName[xmlName.Length - 2];
            string xml_Path  = xmlPath + "\\" + assembleName + ".xml";
            if (File.Exists(xml_Path))
            {
                return true;
            }
            return false;
        }
        public void writeXml(DesignerItem root)
        {
            try
            {
                xmlFileName = xmlFileName.Replace("icon", "*");
                string[] tempName = xmlFileName.Split('*');
                string[] xmlName = xmlFileName.Split('\\');
                assembleName = xmlName[xmlName.Length - 2];
                xmlFileName = xmlPath + "\\" + assembleName + ".xml";
                if (File.Exists(xmlFileName))
                {
                    File.Delete(xmlFileName);
                }
                using (FileStream fs = new FileStream(xmlFileName, FileMode.Create))
                {
                    XmlTextWriter writer = new XmlTextWriter(fs, null);
                    writer.Formatting = Formatting.Indented;
                    writer.WriteStartElement("MainAlbum");
                    readDesignerItem(root, writer);
                    writer.WriteFullEndElement();
                    writer.Close();
                    fs.Close();
                }
                MessageBox.Show("保存成功");
            }
            catch(Exception e)
            {
                MessageBox.Show("保存失败:"+e.Message);
            }
        }
        private void readDesignerItem(DesignerItem root,XmlTextWriter writer)
        {
            foreach (DesignerItem item in root.my_album)
            {
                HeaderedContentControl iit = item.Content as HeaderedContentControl;
                if (item.Is_Virtual_Folder)
                {                    
                    MyTextBox iitextbox = iit.Content as MyTextBox;
                    string name = iitextbox.Text;
                    writer.WriteStartElement("Album");
                    writer.WriteAttributeString("album", name);
                    readDesignerItem(item,writer);
                    writer.WriteEndElement();
                }
                else
                {
                    string uup = item.realityAddress;
                    HeaderedContentControl iiit=iit.Header as HeaderedContentControl;
                    MyTextBox textbox=iiit.Content as MyTextBox;
                    string comment=textbox.Text;
                    writer.WriteStartElement("DDS");
                    writer.WriteAttributeString("filename",uup);
                    writer.WriteAttributeString("comment",comment);
                    writer.WriteEndElement();
                }
            }
        }
    }
}