using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Windows.Forms;

namespace FIRSTWA_Recorder
{
    class HyperDeck
    {
        TcpClient client;
        NetworkStream stream;

        public HyperDeck(string ipAddress, int port)
        {
            client = new TcpClient(ipAddress, port);
            stream = client.GetStream();
        }

        public void Write(string command)
        {
            byte[] data = Encoding.ASCII.GetBytes(command + "\r\n");
            stream.Write(data, 0, data.Length);
        }

        public string Read()
        {
            byte[] data = new byte[256];
            int bytes = stream.Read(data, 0, data.Length);
            return Encoding.ASCII.GetString(data, 0, bytes);
        }
    }
}
