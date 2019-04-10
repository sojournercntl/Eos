using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EosClient;
using EosShared;
using LighthouseClientExample.Observer;
using Reactor.Util;

namespace LighthouseClientExample
{
    public class Client : EosCoreClient, ISubject
    {
        private static Client _instance = null;
        private static object padlock = new object();

        public static Client Instance
        {
            get
            {
                lock (padlock)
                {
                    if (_instance == null)
                        _instance = new Client();
                    return _instance;
                }
            }
        }

        private const int _portTCP = 5022;
        private const int _portUDP = 5023;

        private List<IObserver> _observers = new List<IObserver>();

        private Client() : base()
        {
        start:
            try
            {
                Start(DataGenerator.Generate(12), "ACCESSKEY", _portUDP, _portTCP, IPTool.GetLocalIPAddress());
            }
            catch
            {
                Thread.Sleep(500);
                goto start;
            }
        }

        public override void OnSecuredEvent()
        {
            Console.WriteLine(DateTime.Now.ToString() + "> Client | Secured connection - Registering!");
            RequestRegistration();
            base.OnSecuredEvent();
        }

        protected override void HandlePushMessage(EosPacket p)
        {
            Notify(p);
            Console.WriteLine(DateTime.Now.ToString() + "> Client | Received Push Message: " + Encoding.Unicode.GetString(p.Data[0]));
            base.HandlePushMessage(p);
        }

        public override void OnConnectedEvent()
        {
            Console.WriteLine(DateTime.Now.ToString() + "> Client | Connected to Server");
            base.OnConnectedEvent();
        }

        public override void OnCrashedEvent()
        {
            Console.WriteLine(DateTime.Now.ToString() + "> Client | Crash!!!");
            base.OnCrashedEvent();
        }

        public override void OnDisconnectedEvent()
        {
            Console.WriteLine(DateTime.Now.ToString() + "> Client | Disconnected from Server");
            base.OnDisconnectedEvent();
        }

        public override void OnPacketReceivedEvent(byte[] data)
        {
            Console.WriteLine(DateTime.Now.ToString() + "> Client | Received a TCP Packet");
            base.OnPacketReceivedEvent(data);
        }

        public void Attach(IObserver observer)
        {
            _observers.Add(observer);
        }

        public void Detach(IObserver observer)
        {
            _observers.Remove(observer);
        }

        public void Notify(EosPacket packet)
        {
            foreach (var o in _observers)
            {
                o.Update(packet);
            }
        }
    }
}
