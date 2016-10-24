using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlTypes;
using System.Windows.Forms;
using System.IO;

namespace FileManage
{
    class OpenImg
    {
        public void open(Image img,string Path)
        {
            //OpenFileDialog openFileDialog1 = new OpenFileDialog();
            //openFileDialog1.InitialDirectory = "";
           // openFileDialog1.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            //openFileDialog1.FilterIndex = 2;
            //openFileDialog1.RestoreDirectory = true;
            //openFileDialog1.ShowDialog();
            //Uri imgUri = new Uri(openFileDialog1.FileName.ToString().Trim(), UriKind.RelativeOrAbsolute);
            Uri imgUri = new Uri(Path.ToString().Trim(), UriKind.RelativeOrAbsolute);
            img.Source = new BitmapImage(imgUri);
            //return openFileDialog1.FileName.ToString().Trim();
        }
    }
}
