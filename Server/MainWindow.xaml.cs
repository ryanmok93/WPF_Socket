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
            if(myServerSocket.socketStatus() == false)
            {
                MessageBox.Show("请先打开服务器", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if(sendTextBox.Text=="")
            {
                MessageBox.Show("请输入需要发送的内容", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            myServerSocket.setSendStringMessage(sendTextBox.Text);

            receiveTextBox.AppendText(System.DateTime.Now.ToString()+": " +sendTextBox.Text+"\n");

            this.receiveTextBox.ScrollToEnd();
        }

        private void openServerButton_Click(object sender, RoutedEventArgs e)
        {
            int tmpPort;
            Int32.TryParse(portTextBox.Text, out tmpPort);
            if(tmpPort >65536 || tmpPort < 1024)
            {
                MessageBox.Show("请输入正确的端口号", "Warning",MessageBoxButton.OK,MessageBoxImage.Error);
                return;
            }
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
                receiveTextBox.AppendText("打开服务器失败");
            }

        }

        private void clearButton_Click(object sender, RoutedEventArgs e)
        {
            receiveTextBox.Clear();
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
