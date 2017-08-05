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

using System.Net.Sockets;
using System.Net;

namespace Client
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        TcpSocket tcpSocket = null;
        IPAddress ipaddress = null;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void connectToServerButton_Click(object sender, RoutedEventArgs e)
        {
            String stringIPAddress = textBoxIPAddress.Text;
            Int32 port = Int32.Parse(textBoxPort.Text);

            //    MessageBox.Show("IP地址："+IPAddress+"\n端口："+port);

            //去除IP地址中的空格
            if (stringIPAddress.Contains(' '))
            {
                stringIPAddress = stringIPAddress.Trim();
            }
            if (!IPAddress.TryParse(stringIPAddress, out ipaddress))
            {
                MessageBox.Show("请输入合法的IP地址");
                return;
            }

            if (tcpSocket == null)
            {

                tcpSocket = new TcpSocket();
                bool succeed = tcpSocket.Connect(stringIPAddress, 8080);
                if (!succeed)
                {
                    receiveRichTextBox.AppendText("连接失败\n");
                    tcpSocket = null;
                }
                else
                {
                    receiveRichTextBox.AppendText("已连接：" + stringIPAddress + "\r\n");
                }


            }

        }

        private void clearButton_Click(object sender, RoutedEventArgs e)
        {
            receiveRichTextBox.Document.Blocks.Clear();
        }

        private void stopButton_Click(object sender, RoutedEventArgs e)
        {
            if (tcpSocket != null)
            {
                tcpSocket.closeConnect();
                tcpSocket = null;
                receiveRichTextBox.AppendText("已关闭连接\r\n");
            }

        }

        private void exitButton_Click(object sender, RoutedEventArgs e)
        {
            if (tcpSocket != null && tcpSocket.getStatus())
            {
                tcpSocket.closeConnect();
            }

            this.Close();
        }

        private void sendButton_Click(object sender, RoutedEventArgs e)
        {
            if (tcpSocket == null )
            {
                MessageBox.Show("请先连接服务器");
                return;
            }
            if (!tcpSocket.getStatus())
            {
                receiveRichTextBox.AppendText("服务器连接异常\r\n");
                return;
            }
            String sendMsg = StringFromTextBox(sendRichTextBox);

            tcpSocket.sendMessage(sendMsg);
            
            
        }

        static string StringFromTextBox(RichTextBox rtb)
        {
            TextRange textRange = new TextRange(rtb.Document.ContentStart, rtb.Document.ContentEnd);

            return textRange.Text;
        }
    }
}
