using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace EosServer
{
    /// <summary>
    /// Signal class - Sends UDP Messages to the client
    /// </summary>
    public static class EosSignal
    {
        public static void SendNotification(string ip,int port, byte[] data)
        {
            UdpClient udpClient = new UdpClient();
            udpClient.Connect(IPAddress.Parse(ip), port);
            udpClient.Send(data, data.Length);
            udpClient.Close();
            udpClient.Dispose();
        }

    }
}
