using Quick;
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
using System.Windows.Shapes;

namespace AppDemo
{
    /// <summary>
    /// Window2.xaml 的交互逻辑
    /// </summary>
    public partial class Window2
    {
        public Window2()
        {
            InitializeComponent();
        }
        private void CustomWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.F5)
            {
                this.WindowState = WindowState.Maximized;
            }
        }
    }
}
