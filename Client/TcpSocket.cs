using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net.Sockets;


namespace Client
{
    class TcpSocket
    {
        TcpClient client = null;

        public bool Connect(String server,Int32 port)
        {
            try
            {
               client = new TcpClient(server, port);
                return true;
            }
            catch(ArgumentNullException e){
                System.Windows.MessageBox.Show(e.ToString());
            }
            catch(SocketException e)
            {
                System.Windows.MessageBox.Show(e.ToString());
                return false;
            }

            return false;
        }
        public void sendMessage(String message)
        {
            Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);

            NetworkStream stream = client.GetStream();

            stream.Write(data, 0, data.Length);
        }

        public void receiveMessage()
        {

        }

        public void closeConnect()
        {
            client.Close();
        }

        public bool getStatus()
        {
            return client.Connected;
        }

    }
}
