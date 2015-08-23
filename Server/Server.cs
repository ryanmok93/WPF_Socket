using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Server_socket
{
    public class ServerSocket
    {
        private String receiveStringMessage = null;
        private String sendStringMessage = null;
        private static Socket serverSocket = null;
        private int myPort=8080; //Set default socket port       
        IPAddress ip = IPAddress.Parse("127.0.0.1");
        private static bool statue = false;
        
        public bool StartServer()
        {

            serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverSocket.Bind(new IPEndPoint(ip, myPort));
            serverSocket.Listen(32);
            
            statue = true;
            return true;
        }

        public void sendMessage()
        {

        }

        public bool StopServer()
        {
            serverSocket.Close();
            statue = false;
            return true;
        }

        public int getPort()
        {
            return myPort;
        }

        public void setPort(int port)
        {
            this.myPort = port;
        }
        public bool socketStatus()
        {
            return statue;
        }
        
    }
}
