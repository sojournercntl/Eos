using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EosClient
{
    
    /// <summary>
    /// Delegate - Packet Received
    /// </summary>
    /// <param name="data">Data-Received</param>
    public delegate void ReceivedPacket(byte[] data);

    /// <summary>
    /// Receiver - For EOS notifications
    /// </summary>
    public class EosReceiver
    {
        /// <summary>
        /// The port to receive messages
        /// </summary>
        public int RECEIVER_PORT = 5124;

        private Thread receiverThread;
        private IPEndPoint RemoteIpEndPoint;
        private UdpClient udpClient;

        private bool quitThread = false;

        public event ReceivedPacket ReceivedPacketEvent;

        public EosReceiver(int port)
        {
            this.RECEIVER_PORT = port;
        }

        public void StartListening()
        {
            quitThread = false;
            RemoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
            receiverThread = new Thread(receive);
            receiverThread.Start();
        }

        public void StopListening()
        {
            close();
            if (receiverThread != null)
            {
                quitThread = true;
            }
        }

        public void receive()
        {
            udpClient = new UdpClient(RECEIVER_PORT);
            while (!quitThread)
            {
                byte[] data = udpClient.Receive(ref RemoteIpEndPoint);
                ReceivedPacketEvent?.Invoke(data);
            }
        }

        public void close()
        {
            if (udpClient != null)
            {
                udpClient.Close();
            }
        }
    }
}
