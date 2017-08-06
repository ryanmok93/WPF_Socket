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

using System.Threading;

namespace Client
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        TcpSocket tcpSocket = null;
        IPAddress ipaddress = null;

        Thread receiveMessageThread = null;

        //声明委托
        public delegate void UpdateTextCallBack(String msg);

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
                    this.UpdateTextBox("> 连接失败");
                    tcpSocket = null;
                }
                else
                {
                    this.UpdateTextBox("> 已连接：" + stringIPAddress);

                    //    receiveMessageThread = new Thread(handleReceiveThread);
                    //    receiveMessageThread.Start();

                    receiveMessageThread = new Thread(new ThreadStart(UpdateTextBoxThread));
                    receiveMessageThread.Start();
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

                if (receiveMessageThread.IsAlive)
                {
                    receiveMessageThread.Abort();

                }

                tcpSocket = null;
                this.UpdateTextBox("> 已关闭连接");
            }

        }

        private void exitButton_Click(object sender, RoutedEventArgs e)
        {
            if (tcpSocket != null && tcpSocket.getStatus())
            {
                tcpSocket.closeConnect();
                if (receiveMessageThread.IsAlive)
                {
                    receiveMessageThread.Abort();

                }
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
                this.UpdateTextBox("!> 服务器连接异常");
                return;
            }
            String sendMsg = StringFromTextBox(sendRichTextBox);

            tcpSocket.sendMessage(sendMsg);
            this.UpdateTextBox("# 发送消息："+sendMsg);

        }

        //从RichTextBox获取String内容
        static String StringFromTextBox(RichTextBox rtb)
        {
            TextRange textRange = new TextRange(rtb.Document.ContentStart, rtb.Document.ContentEnd);
            return textRange.Text.Replace("\r\n", "");
        }

        //接收消息 更新文本框线程
        private void UpdateTextBoxThread()
        {
            while (true)
            {
                Thread.Sleep(1000);
                receiveRichTextBox.Dispatcher.Invoke(
                    new UpdateTextCallBack(this.HandleReceiveMsgTextBox),
                    new object[] { tcpSocket.receiveMessage() }
                    );
            }

        }

        //处理接收到的消息
        private void HandleReceiveMsgTextBox(String msg)
        {
            this.UpdateTextBox("# 接收服务器："+msg);
        }

        //更新接收文本框
        public void UpdateTextBox(String msg)
        {
            receiveRichTextBox.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            receiveRichTextBox.ScrollToEnd();
            receiveRichTextBox.AppendText(msg + "\n");
            receiveRichTextBox.Focus();
        }
    }
}
