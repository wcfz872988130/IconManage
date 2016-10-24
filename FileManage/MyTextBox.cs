using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Security.AccessControl;
using System.Windows.Media;
using System.Windows.Input;

namespace FileManage
{
    class MyTextBox:TextBox
    {
        public delegate void DoOneTextBoxChange();
        public static event DoOneTextBoxChange DoTextboxChange;
        public delegate void DoOneTextBoxCheck(string name,DesignerItem item);
        public static event DoOneTextBoxCheck DoTextboxCheck;
        public MyTextBox()
        {           
            //this.KeyDown += new KeyEventHandler(tb_KeyDown);
        }
        protected override void OnLostFocus(RoutedEventArgs e)
        {
            base.OnLostFocus(e);
            TextBox tb = e.Source as TextBox;
            //this.IsHitTestVisible = false;
            tb.PreviewMouseDown += new MouseButtonEventHandler(OnPreviewMouseDown);
            HeaderedContentControl it = this.Parent as HeaderedContentControl;
            if (it.Parent is DesignerItem)
            {
                DesignerItem iit = it.Parent as DesignerItem;
                if (DoTextboxCheck != null)
                {
                    DoTextboxCheck(this.Text, iit);
                }
            }           
            if (DoTextboxChange != null)
            {
                DoTextboxChange();
            }
        }
        protected override void OnGotFocus(RoutedEventArgs e)
        {
            base.OnGotFocus(e);
            TextBox tb = e.Source as TextBox;
            tb.SelectAll();
            tb.PreviewMouseDown -= new MouseButtonEventHandler(OnPreviewMouseDown);
        }
        void OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            TextBox tb = e.Source as TextBox;
            tb.Focus();
            e.Handled = true;
        }
    }
}
