using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EosShared;

namespace LighthouseServer.Strategy
{
    public class ConcreteDefaultSender : ISendStrategy
    {
        public void SendPacket(EosPacket packet, Server server,string username)
        {
            server.SendPushMessage(username, packet);
        }

    }
}
