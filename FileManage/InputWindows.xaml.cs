using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FileManage
{
    /// <summary>
    /// InputWindows.xaml 的交互逻辑
    /// </summary>
    public partial class InputWindows : Window
    {
        public VirtualDesignerCanvas virtualdesigner;
        public InputWindows(VirtualDesignerCanvas virtualdesignername)
        {
            InitializeComponent();
            this.CommandBindings.Add(new CommandBinding(ApplicationCommands.Close,Close_Executed));
            virtualdesigner = virtualdesignername; 
            //virtualdesigner = new VirtualDesignerCanvas();
            Binding binding = new Binding();
            binding.Source = virtualdesigner;
            binding.Path = new PropertyPath("AlbumName");
            BindingOperations.SetBinding(AlbumFileName, TextBox.TextProperty, binding);
        }
        private void Close_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            this.Close();
        }
    }
}
