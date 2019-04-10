using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EosShared;

namespace LighthouseServer.Factory
{
    public interface INotification
    {

        NotificationType Type { get; set; }
        string Message { get; set; }

        EosPacket CreatePacket();
    }
}
