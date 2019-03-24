using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EosShared
{
    public enum EosPacketType
    {
        RequstRegistration,     // Client registeres with ID and DATA
        Registered,             // Server registered the client as active for (INTERVAL UP TO 10 min)
        Message,                // Server Sent Message
        Data,                   // Server Sent Data
        Shutdown                // Server Registration Canceled - Shutdown Imminent
    }

}
