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
using Server_socket;

namespace Server
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {

        private ServerSocket myServerSocket;

        public MainWindow()
        {
            InitializeComponent();
            closeServerButton.IsEnabled = false;

            myServerSocket = new ServerSocket();
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void openServerButton_Click(object sender, RoutedEventArgs e)
        {
            
            myServerSocket.setPort(Convert.ToInt32(portTextBox.Text));
            if (myServerSocket.StartServer() == true)
            {

               openServerButton.IsEnabled = false;
               closeServerButton.IsEnabled = true;
                string str = "打开服务器（端口："+portTextBox.Text+"）成功\n";
                receiveTextBox.AppendText(str);
            }
            else
            {
                
            }

        }

        private void clearButton_Click(object sender, RoutedEventArgs e)
        {
            receiveTextBox.Text = "";
        }

        private void closeServerButton_Click(object sender, RoutedEventArgs e)
        {
            if(myServerSocket.StopServer() == true)
            {
                closeServerButton.IsEnabled = false;
                openServerButton.IsEnabled = true;
            }
            receiveTextBox.AppendText("服务器已停止\n");
        }

        private void exitButton_Click(object sender, RoutedEventArgs e)
        {
            if(myServerSocket.socketStatus() == true)
            {
                myServerSocket.StopServer();
            }
            receiveTextBox.Text = "清理缓存...";

            this.Close();
        }
    }
}
