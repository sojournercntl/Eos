using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using EosShared;
using Reactor.Crypto;
using Reactor.Networking.Client;

namespace EosClient
{
    public class EosCoreClient : ReactorClient
    {
        public string AccessKey { get; set; }
        public string Username { get; set; }
        public int PortReceiver { get; set; }

        public EosReceiver Receiver { get; set; }

        public EosCoreClient(){ }

        public void Start(string username, string accessKey, int portReceiver, int port, string ip)
        {
            this.AccessKey = accessKey;
            this.Username = username;
            this.PortReceiver = portReceiver;

            ConnectedEvent += OnConnectedEvent;
            CrashedEvent += OnCrashedEvent;
            DisconnectedEvent += OnDisconnectedEvent;
            TerminatingEvent += OnTerminatingEvent;
            PacketReceivedEvent += OnPacketReceivedEvent;
            SecuredEvent += OnSecuredEvent;
            base.Start(IPAddress.Parse(ip),port );
        }

        public virtual void ReceiverOnReceivedPacketEvent(byte[] data)
        {
            byte[] d = Encryption.AES_Decrypt(data, Key, Salt);
            EosPacket p = new EosPacket(d);

            if (p.Type == EosPacketType.Message)
            {
                HandlePushMessage(p);
            }
            else if (p.Type == EosPacketType.Shutdown)
            {
                HandleServerShutdown();
            }
        }

        public virtual void OnSecuredEvent()
        {
            //throw new NotImplementedException();
        }

        public virtual void OnPacketReceivedEvent(byte[] data)
        {
            EosPacket p = new EosPacket(data);
            HandlePacket(p);
        }

        public virtual void OnTerminatingEvent()
        {
            //throw new NotImplementedException();
        }

        public virtual void OnDisconnectedEvent()
        {
            //throw new NotImplementedException();
        }

        public virtual void OnCrashedEvent()
        {
            //throw new NotImplementedException();
        }

        public virtual void OnConnectedEvent()
        {
            //throw new NotImplementedException();
        }

        #region Handlers

        public virtual void HandleServerShutdown()
        {
            Receiver.StopListening();
            base.Reconnect();
        }

        protected virtual void HandlePushMessage(EosPacket p)
        {
            // OVERWRITE
        }

        protected virtual void HandlePacket(EosPacket p)
        {
            switch (p.Type)
            {
                case EosPacketType.Registered:
                    // Start the Receiver
                    Receiver = new EosReceiver(PortReceiver);
                    Receiver.ReceivedPacketEvent += ReceiverOnReceivedPacketEvent;
                    Receiver.StartListening();
                    SendTerminate();
                    break;
            }
        }

        #endregion

        #region Custom Messages (TCP)

        protected void RequestRegistration()
        {
            EosPacket p = new EosPacket();
            p.Sender = Id;
            p.Type = EosPacketType.RequstRegistration;
            p.Data.Add(Encoding.Unicode.GetBytes(AccessKey));
            p.Data.Add(Encoding.Unicode.GetBytes(Username));
            base.SendPacket(p.ToBytes());
        }

        #endregion


    }
}
