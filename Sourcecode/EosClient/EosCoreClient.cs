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
    /// <summary>
    /// EosCoreClient - Base for all EOS Client
    /// This class is built on top of a reactor client
    /// and inherits all its features and functionality.
    /// </summary>
    public class EosCoreClient : ReactorClient
    {
        
        /// <summary>
        /// The access key used to authenticate against the
        /// real time messaging system
        /// </summary>
        public string AccessKey { get; set; }

        /// <summary>
        /// The username that is used to qualify on the 
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// The port to use for the receiver (UDP)
        /// </summary>
        public int PortReceiver { get; set; }

        /// <summary>
        /// EOS Receiver
        /// </summary>
        public EosReceiver Receiver { get; set; }

        public EosCoreClient(){ }

        /// <summary>
        /// Starts the EOS Client Service
        /// </summary>
        /// <param name="username">username</param>
        /// <param name="accessKey">key</param>
        /// <param name="portReceiver">udp-port</param>
        /// <param name="port">tcp-port</param>
        /// <param name="ip">ip of target</param>
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

        /// <summary>
        /// ReactorClient packet receiver
        /// </summary>
        /// <param name="data"></param>
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

        /// <summary>
        /// Called as soon as the connection is established and the DHKE is complete.
        /// </summary>
        public virtual void OnSecuredEvent()
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// called when bytes are received. Casted to a EosPacket.
        /// </summary>
        /// <param name="data"></param>
        public virtual void OnPacketReceivedEvent(byte[] data)
        {
            EosPacket p = new EosPacket(data);
            HandlePacket(p);
        }

        /// <summary>
        /// Connection will be terminated
        /// </summary>
        public virtual void OnTerminatingEvent()
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Connection disconnected
        /// </summary>
        public virtual void OnDisconnectedEvent()
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Crash
        /// </summary>
        public virtual void OnCrashedEvent()
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Connection established
        /// </summary>
        public virtual void OnConnectedEvent()
        {
            //throw new NotImplementedException();
        }

        #region Handlers

        /// <summary>
        /// ServerShutdown Handler
        /// </summary>
        public virtual void HandleServerShutdown()
        {
            Receiver.StopListening();
            base.Reconnect();
        }

        /// <summary>
        /// Overwrite this to handle the PushMessage. The parameter is passed by the
        /// receiver.
        /// </summary>
        /// <param name="p"></param>
        protected virtual void HandlePushMessage(EosPacket p)
        {
            // OVERWRITE
        }

        /// <summary>
        /// Handle a Packet.
        /// </summary>
        /// <param name="p"></param>
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

        /// <summary>
        /// Request the registration on the messaging server.
        /// </summary>
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
