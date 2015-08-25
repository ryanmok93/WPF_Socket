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

        private static string receiveStringMessage = null;
        private static string sendStringMessage = null;
        private static Socket serverSocket;
        private static Socket clientSocket;
        private static Thread myThread;
        private static bool socketStatue = false;
        private static byte[] recvBytes = new byte[1024];
        delegate void myDelegate(string str);

        public MainWindow()
        {
            InitializeComponent();
            closeServerButton.IsEnabled = false;


        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            if(socketStatue == false)
            {
                MessageBox.Show("请先打开服务器", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if(sendTextBox.Text=="")
            {
                MessageBox.Show("请输入需要发送的内容", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            receiveTextBox.AppendText(System.DateTime.Now.ToString()+": " +sendTextBox.Text+"\n");

            this.receiveTextBox.ScrollToEnd();
        }

        private void openServerButton_Click(object sender, RoutedEventArgs e)
        {
            int tmpPort;
            int myPort = 8080;                      //Set default socket port       
            IPAddress hostIP = IPAddress.Parse("127.0.0.1");
            Int32.TryParse(portTextBox.Text, out tmpPort);
            if(tmpPort >65536 || tmpPort < 1024)
            {
                MessageBox.Show("请输入正确的端口号", "Warning",MessageBoxButton.OK,MessageBoxImage.Error);
                return;
            }
            myPort = tmpPort;
            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ipe = new IPEndPoint(hostIP, myPort);
            serverSocket.Bind(ipe);
            serverSocket.Listen(32);

            myThread = new Thread(ListenClientConnect);
            myThread.Start();
           

            if(serverSocket.IsBound == true)
            {
                socketStatue = true;
                string str = "打开服务器（端口：" + portTextBox.Text + "）成功\n";
                receiveTextBox.AppendText(str);
                openServerButton.IsEnabled = false;
                closeServerButton.IsEnabled = true;
            }
            else
            {
                socketStatue = false;
                receiveTextBox.AppendText("打开服务器失败");
            }
            
        }

        public void ListenClientConnect()
        {

            clientSocket = serverSocket.Accept();
            //    Thread receiveThread = new Thread(ReceiveMessage);
            //   receiveThread.Start(clientSocket);
            ReceiveMessage(clientSocket); 
           
        }
        public void ReceiveMessage(object clientSocket)
        {
            myDelegate d = new myDelegate(updateReceiveTextBox);
            Socket myClientSocket = (Socket)clientSocket;
            while(true)
            {
                try
                {
                    int len = myClientSocket.Receive(recvBytes);
                    if (len == 0)
                    {
                        MessageBox.Show("Client left");
                        myClientSocket.Shutdown(SocketShutdown.Both);
                        myClientSocket.Close();
                        break;
                    }
              //      this.Dispatcher.Invoke(d, Encoding.ASCII.GetString(recvBytes,0,len));
                    this.Dispatcher.Invoke(d, Encoding.UTF8.GetString(recvBytes, 0, len));
                    Thread.Sleep(100);
                }
                catch(Exception ex)
                {
                    this.Dispatcher.Invoke(d, ex.ToString());
                }
            }
        }
        public void updateReceiveTextBox(string str)
        {
            receiveTextBox.AppendText(str + "\n");
            receiveTextBox.ScrollToEnd();
        }

        private void clearButton_Click(object sender, RoutedEventArgs e)
        {
            receiveTextBox.Clear();
            
        }

        private void closeServerButton_Click(object sender, RoutedEventArgs e)
        {
            if(socketStatue == true)
            {
                
            //    clientSocket.Close();
            //    serverSocket.Close();

                closeServerButton.IsEnabled = false;
                openServerButton.IsEnabled = true;
            }
            receiveTextBox.AppendText("服务器已停止\n");
        }

        private void exitButton_Click(object sender, RoutedEventArgs e)
        {
            if(socketStatue == true)
            {
                //    clientSocket.Close();
                //    serverSocket.Close();
            }

            this.Close();
        }
    }
}
