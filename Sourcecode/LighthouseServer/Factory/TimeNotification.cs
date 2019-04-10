using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using EosShared;

namespace LighthouseServer.Factory
{
    public class TimeNotification : INotification
    {
        public NotificationType Type { get; set; }
        public string Message { get; set; }

        public EosPacket CreatePacket()
        {
            EosPacket p = new EosPacket();
            p.Type = EosPacketType.Message;
            p.Data.Add(Encoding.Unicode.GetBytes(Message));
            return p;
        }
    }
}
