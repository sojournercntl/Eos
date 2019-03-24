using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EosShared;
using Reactor.Crypto;
using Reactor.Networking.Server;

namespace EosServer
{
    // TODO: Make Singleton
    public class EosCoreServer
    {
        protected int SignalPort { get; set; }
        protected long TimeToLive { get; set; }
        protected int TimeBetweenChecks { get; set; }

        protected Timer ttlTimer;
        protected Dictionary<ReactorVirtualClient,long> TtlClients = new Dictionary<ReactorVirtualClient, long>();

        private bool serverRunning = false;


        public EosCoreServer(){}

        public void Start(int sp, long ttl, int tbc, string ip, int port)
        {
            this.SignalPort = sp;
            this.TimeToLive = ttl;
            this.TimeBetweenChecks = tbc;

            ttlTimer = new Timer(HandleCheckTtl, "ttl", 0, TimeBetweenChecks);

            ReactorServer.ClientConnectedEvent += ReactorServerOnClientConnectedEvent;
            ReactorServer.ClientDisconnectedEvent += ReactorServerOnClientDisconnectedEvent;
            ReactorServer.ClientConnectionSecuredEvent += ReactorServerOnClientConnectionSecuredEvent;
            ReactorServer.ClientCrashedEvent += ReactorServerOnClientCrashedEvent;
            ReactorServer.ClientPacketReceivedEvent += ReactorServerOnClientPacketReceivedEvent;
            
            ReactorServer.Start(ip,port);
        }

        private void ReactorServerOnClientPacketReceivedEvent(ReactorVirtualClient c, byte[] data)
        {
            EosPacket p = new EosPacket(data);
            HandleReactorPacket(c,p);
        }

        public virtual void ReactorServerOnClientCrashedEvent(ReactorVirtualClient c)
        {
            //throw new NotImplementedException();
        }

        public virtual void ReactorServerOnClientConnectionSecuredEvent(ReactorVirtualClient c)
        {
            //throw new NotImplementedException();
        }

        public virtual void ReactorServerOnClientDisconnectedEvent(ReactorVirtualClient c)
        {
            //throw new NotImplementedException();
        }

        public virtual void ReactorServerOnClientConnectedEvent(ReactorVirtualClient c)
        {
            //throw new NotImplementedException();
        }
        


        #region Handlers

        public void HandleReactorPacket(ReactorVirtualClient c,EosPacket p)
        {
            switch (p.Type)
            {
                case EosPacketType.RequstRegistration:
                    byte[] accesskey = p.Data[0];
                    string username = Encoding.Unicode.GetString(p.Data[1]);
                    if (!ValidateAccessKey(accesskey))
                    {
                        break;
                    }
                    c.Tag = Encoding.Unicode.GetBytes(username);
                    // register
                    TtlClients.Add(c, Environment.TickCount); // Current Time in Ticks
                    SendPushRegistration(c);
                    break;
            }
        }

        public virtual bool ValidateAccessKey(byte[] key)
        {
            return true;
        }

        protected virtual void HandleCheckTtl(object state)
        {
            List<ReactorVirtualClient> clientsToRemove = new List<ReactorVirtualClient>();

            foreach (var c in TtlClients)
            {
                if (Environment.TickCount > (c.Value + TimeToLive))
                {
                    // TODO: Signal send Shutdown
                    SendPushShutdown(c.Key);
                    clientsToRemove.Add(c.Key);
                }
            }

            foreach (var c in clientsToRemove)
            {
                TtlClients.Remove(c);
            }

        }

        #endregion

        #region Messages

        protected virtual void SendPushRegistration(ReactorVirtualClient c)
        {
            EosPacket p = new EosPacket();
            p.Sender = ReactorServer.Id;
            p.Type = EosPacketType.Registered;
            p.Data.Add(Encoding.Unicode.GetBytes(TimeToLive.ToString()));
            c.SendPacket(p.ToBytes());
        }

        /// <summary>
        /// Send a push notification to a client with the specified username
        /// </summary>
        /// <param name="username"></param>
        /// <param name="p"></param>
        public void SendPushMessage(string username, EosPacket p)
        {
            var entry = (
                    from c in TtlClients where Encoding.Unicode.GetString((byte[])c.Key.Tag) == username select c)
                .SingleOrDefault();

            var client = entry.Key;

            p.Type = EosPacketType.Message;
            byte[] toSend = Encryption.AES_Encrypt(p.ToBytes(), client.Key, client.Salt);
            EosSignal.SendNotification(client.Address,SignalPort, toSend);
        }

        public void SendPushShutdown(ReactorVirtualClient c)
        {
            EosPacket p = new EosPacket();
            p.Sender = ReactorServer.Id;
            p.Type = EosPacketType.Shutdown;
            byte[] toSend = Encryption.AES_Encrypt(p.ToBytes(), c.Key, c.Salt);
            EosSignal.SendNotification(c.Address,SignalPort, toSend);
        }
        
        #endregion



    }
}
