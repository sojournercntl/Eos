using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EosShared;

namespace LighthouseServer.Strategy
{
    public interface ISendStrategy
    {
        void SendPacket(EosPacket packet, Server server,string username);
    }
}
