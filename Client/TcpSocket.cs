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
        NetworkStream stream = null;

        public bool Connect(String server,Int32 port)
        {
            try
            {
                client = new TcpClient(server, port);
                stream = client.GetStream();
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
            stream.Write(data, 0, data.Length);
        }

        public String receiveMessage()
        {
            String responsData;
            Byte[] data = new Byte[256];
            Int32 bytes;
            try
            {
                bytes = stream.Read(data, 0, data.Length);
            }
            catch(Exception e)
            {
                System.Windows.MessageBox.Show(e.ToString());
                return null;
            }
            responsData = Encoding.ASCII.GetString(data, 0, bytes);
            return responsData;

        }

        public void closeConnect()
        {
            stream.Close();
            client.Close();
        }

        public bool getStatus()
        {
            return client.Connected;
        }

    }
}
