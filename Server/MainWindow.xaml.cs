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
using System.Web;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace Server
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            closeServerButton.IsEnabled = false;
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void openServerButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void clearButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void closeServerButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void exitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
