using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
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
using System.Windows.Interactivity;
using System.IO;

namespace FileManage
{
    class AddImage
    {
        const int GridWidth = 80;
        const int GridHeight = 100;
        const int ImgWidth = 80;
        const int ImgHeight = 50;
        const int LabelHeight = 30;
        const int TextHeight = 20;
        private static DDSImage ddsImage;
        private static BitmapImage src;
        public AddImage()
        { }
        public static HeaderedContentControl AddAlbum(string folder_name)
        {
            HeaderedContentControl headcontent = new HeaderedContentControl();
            System.Windows.Controls.Image img = new System.Windows.Controls.Image();
            img.Width = GridWidth;
            img.Height = ImgHeight;
            string album_Icon_path = AppDomain.CurrentDomain.BaseDirectory + @"\album.png";
            //img.Source = new BitmapImage(new Uri("pack://application:,,,/Icons/album.png"));
            Uri imgUri = new Uri(album_Icon_path, UriKind.RelativeOrAbsolute);
            img.Source = new BitmapImage(imgUri);
            img.IsHitTestVisible = false;
            headcontent.Header = img;
            MyTextBox textbox = new MyTextBox();
            Interaction.GetBehaviors(textbox).Add(new TextBoxEnterKeyUpdateBehavior());
            textbox.Text = folder_name;
            textbox.Width = GridWidth;
            textbox.Height = TextHeight;
            textbox.HorizontalContentAlignment = HorizontalAlignment.Center;
            textbox.BorderBrush = new SolidColorBrush(Colors.Transparent);
            headcontent.Content = textbox;
            return headcontent;
        }
        public static HeaderedContentControl AddImageandText(string path,string name,bool isVirtual)
        {
            HeaderedContentControl headcontent = new HeaderedContentControl();
            System.Windows.Controls.Image img = new System.Windows.Controls.Image();
            img.Width = ImgWidth;
            img.Height = ImgHeight;
            FileStream fs = File.OpenRead(path);
            byte[] data = new byte[fs.Length];
            fs.Read(data, 0, data.Length);
            ddsImage = new DDSImage(data);
            src = BitmapToImageSource(ddsImage.images[0]);
            //BitmapImage sourceImage = BitmapToImageSource(ddsImage.images[0]);
            //Uri imgUri = new Uri(path.ToString().Trim(), UriKind.RelativeOrAbsolute);
            img.Source = src;
            img.IsHitTestVisible = false;
            if (isVirtual)
            {
                HeaderedContentControl subHeadContent = new HeaderedContentControl();
                subHeadContent.Header = img;
                MyTextBox comment = new MyTextBox();
                //comment.IsHitTestVisible = false;
                Interaction.GetBehaviors(comment).Add(new TextBoxEnterKeyUpdateBehavior());
                comment.Text = "添加备注";
                comment.Width = GridWidth;
                comment.Height = TextHeight;
                //comment.IsEnabled = false;
                subHeadContent.Content = comment;
                Label text = new Label();
                text.Content = name;
                text.HorizontalContentAlignment = HorizontalAlignment.Center;
                text.VerticalContentAlignment = VerticalAlignment.Center;
                text.Width = GridWidth;
                text.Height = LabelHeight;
                headcontent.Header = subHeadContent;
                headcontent.Content = text;
            }
            else
            {
                headcontent.Header = img;
                Label text = new Label();
                text.Content = name;
                text.HorizontalContentAlignment = HorizontalAlignment.Center;
                text.VerticalContentAlignment = VerticalAlignment.Center;
                text.Width = GridWidth;
                text.Height = LabelHeight;
                headcontent.Content = text;
            }
            return headcontent;
        }
        public static HeaderedContentControl AddImageandText(string path,bool isVirtual)
        {
            string[] sp = path.Split('.');
            string[] filesname = sp[sp.Length - 2].Split('\\');
            string filename = filesname[filesname.Length - 1];
            HeaderedContentControl headcontent = new HeaderedContentControl();
            System.Windows.Controls.Image img = new System.Windows.Controls.Image();
            img.Width = ImgWidth;
            img.Height = ImgHeight;
            FileStream fs = File.OpenRead(path);
            byte[] data = new byte[fs.Length];
            fs.Read(data, 0, data.Length);
            ddsImage = new DDSImage(data);
            src = BitmapToImageSource(ddsImage.images[0]);
            img.Source = src;
            img.IsHitTestVisible = false;
            if (isVirtual)
            {
                HeaderedContentControl subHeadContent = new HeaderedContentControl();
                subHeadContent.Header = img;
                MyTextBox comment = new MyTextBox();
                Interaction.GetBehaviors(comment).Add(new TextBoxEnterKeyUpdateBehavior());
                comment.Text = "添加备注";
                //comment.IsHitTestVisible = false;
                comment.Width = GridWidth;
                comment.Height = TextHeight;
                //comment.IsEnabled = false;
                subHeadContent.Content = comment;
                Label text = new Label();
                text.Content = filename;
                text.HorizontalContentAlignment = HorizontalAlignment.Center;
                text.VerticalContentAlignment = VerticalAlignment.Center;
                text.Width = GridWidth;
                text.Height = LabelHeight;
                //subHeadContent.Content = text;
                headcontent.Header = subHeadContent;
                headcontent.Content = text;
            }
            else
            {
                headcontent.Header = img;
                Label text = new Label();
                text.Content = filename;
                text.HorizontalContentAlignment = HorizontalAlignment.Center;
                text.VerticalContentAlignment = VerticalAlignment.Center;
                text.Width = GridWidth;
                text.Height = LabelHeight;
                headcontent.Content = text;
            }
            //grid.IsHitTestVisible = false;
            return headcontent;
        }
        public static HeaderedContentControl AddImageandText(string path,bool isVirtual,string commentText)
        {
            string[] sp = path.Split('.');
            string[] filesname = sp[sp.Length - 2].Split('\\');
            string filename = filesname[filesname.Length - 1];
            HeaderedContentControl headcontent = new HeaderedContentControl();
            System.Windows.Controls.Image img = new System.Windows.Controls.Image();
            img.Width = ImgWidth;
            img.Height = ImgHeight;
            FileStream fs = File.OpenRead(path);
            byte[] data = new byte[fs.Length];
            fs.Read(data, 0, data.Length);
            DDSImage ddsImage = new DDSImage(data);
            BitmapImage src = BitmapToImageSource(ddsImage.images[0]);
            img.Source = src;
            img.IsHitTestVisible = false;
            if (isVirtual)
            {
                HeaderedContentControl subHeadContent = new HeaderedContentControl();
                subHeadContent.Header = img;
                MyTextBox comment = new MyTextBox();
                Interaction.GetBehaviors(comment).Add(new TextBoxEnterKeyUpdateBehavior());
                comment.Text = commentText;
                comment.Width = GridWidth;
                comment.Height = TextHeight;
                //comment.IsEnabled = false;
                subHeadContent.Content = comment;
                Label text = new Label();
                text.Content = filename;
                text.HorizontalContentAlignment = HorizontalAlignment.Center;
                text.VerticalContentAlignment = VerticalAlignment.Center;
                text.Width = GridWidth;
                text.Height = LabelHeight;
                //subHeadContent.Content = text;
                headcontent.Header = subHeadContent;
                headcontent.Content = text;
            }
            else
            {
                headcontent.Header = img;
                Label text = new Label();
                text.Content = filename;
                text.HorizontalContentAlignment = HorizontalAlignment.Center;
                text.VerticalContentAlignment = VerticalAlignment.Center;
                text.Width = GridWidth;
                text.Height = LabelHeight;
                headcontent.Content = text;
            }
            //grid.IsHitTestVisible = false;
            return headcontent;
        }
        private static BitmapImage BitmapToImageSource(Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();

                return bitmapimage;
            }
        }
    }
}
