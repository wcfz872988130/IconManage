using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Security.AccessControl;
using System.Windows.Media;

namespace FileManage
{
    class VirtualTextPath
    {
        public TextBox virtualtextboxpath;
        public List<DesignerItem> designerlist=new List<DesignerItem>();
        public VirtualTextPath(TextBox textbox)
        {
            DesignerItem.doubleclickEvent += TraverseStack;
            virtualtextboxpath = textbox;
        }
        public void AddDirectoryContent(string name)
        {
            name=name+"/";
            virtualtextboxpath.Text += name;
        }
        public void ReduceDirectoryContent()
        {
            string[] names = virtualtextboxpath.Text.Split('/');
            string temp = null;
            for (int i = 0; i < names.Length - 2; ++i)
            {
                temp += names[i] + "/";
            }
            virtualtextboxpath.Text = temp;
        }

        public void TraverseStack()
        {
            virtualtextboxpath.Text = "MainAlbum/";
            designerlist.Clear();
            while (pathStack.virtualalbumpath.Count > 1)
            {
                designerlist.Add(pathStack.virtualalbumpath.Peek());
                pathStack.virtualalbumpath.Pop();                
            }
            for (int i = designerlist.Count-1; i >=0;--i )
            {
                HeaderedContentControl iit=designerlist[i].Content as HeaderedContentControl;
                TextBox iitextbox = iit.Content as TextBox;
                string name = iitextbox.Text+"/";
                virtualtextboxpath.Text += name;
            }
            for (int i = designerlist.Count - 1; i >= 0; --i)
            {
                pathStack.virtualalbumpath.Push(designerlist[i]);
            }
        }
    }
}
