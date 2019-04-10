using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EosShared;

namespace LighthouseClientExample.Observer
{
    public interface ISubject
    {
        void Attach(IObserver observer);

        void Detach(IObserver observer);

        void Notify(EosPacket packet);

    }
}
