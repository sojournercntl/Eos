using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EosShared;

namespace LighthouseEnabledClient.Observer
{
    public interface IObserver
    {
        void Update(EosPacket packet);
    }
}
