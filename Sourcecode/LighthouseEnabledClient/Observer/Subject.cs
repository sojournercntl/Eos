using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using EosShared;

namespace LighthouseEnabledClient.Observer
{
    public interface ISubject
    {
        void Attach(IObserver observer);

        void Detach(IObserver observer);

        void Notify(EosPacket packet);

    }
}
