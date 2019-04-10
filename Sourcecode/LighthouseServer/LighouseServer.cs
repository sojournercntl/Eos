using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EosServer;
using EosShared;
using LighthouseServer.Factory;
using LighthouseServer.Strategy;
using Reactor.Networking.Server;
using Reactor.Util;

namespace LighthouseServer
{
    /// <summary>
    /// This example sends a signal to all subscribed clients every 5seconds
    /// </summary>
    public class Server: EosCoreServer
    {
        private static Server _instance = null;
        private static readonly object padlock = new object();

        private const int _portTcp = 5022;
        private const int _portUdp = 5023;
        private Timer _timer;

        public static Server Instance
        {
            get
            {
                lock (padlock)
                {
                    if(_instance == null)
                        _instance = new Server();
                    return _instance;
                }
            }
        }

        private Server()
        {
            base.Start(_portUdp,1000*20,1000,IPTool.GetLocalIPAddress(),_portTcp);
            _timer = new Timer(SendSignal, "LIGHTHOUSE", 0, 1000);
        }

        public override bool ValidateAccessKey(byte[] key)
        {
            Console.WriteLine(DateTime.Now.ToString()+ "> Server | Validating Access Key");
            return base.ValidateAccessKey(key);
        }

        public override void ReactorServerOnClientConnectedEvent(ReactorVirtualClient c)
        {
            Console.WriteLine(DateTime.Now.ToString() + "> Server | Client connected: "+c.Id);
            base.ReactorServerOnClientConnectedEvent(c);
        }

        public override void ReactorServerOnClientConnectionSecuredEvent(ReactorVirtualClient c)
        {
            Console.WriteLine(DateTime.Now.ToString() + "> Server | Connection to "+c.Id+ " is now secured.");
            base.ReactorServerOnClientConnectionSecuredEvent(c);
        }

        public override void ReactorServerOnClientCrashedEvent(ReactorVirtualClient c)
        {
            Console.WriteLine(DateTime.Now.ToString() + "> Server | Client Crashed: "+c.Id);
            base.ReactorServerOnClientCrashedEvent(c);
        }

        public override void ReactorServerOnClientDisconnectedEvent(ReactorVirtualClient c)
        {
            Console.WriteLine(DateTime.Now.ToString() + "> Server | Client Disconnected: "+c.Id);
            base.ReactorServerOnClientDisconnectedEvent(c);
        }

        

        public void SendSignal(object state)
        {
            NotificationFactory factory = NotificationFactory.Instance;
            INotification notification = factory.CreateNotification(NotificationType.Time);

            // Send 
            foreach (var x in TtlClients)
            {
                string username = Encoding.Unicode.GetString((byte[])x.Key.Tag);
                // Strategy Pattern implementation
                ISendStrategy strategy = new ConcreteDefaultSender();
                strategy.SendPacket(notification.CreatePacket(),this,username);
            }

            //EosPacket p = new EosPacket();
            //p.Sender = ReactorServer.Id;
            //p.Data.Add(Encoding.Unicode.GetBytes(DateTime.Now.ToString()));

        }

    }
}
