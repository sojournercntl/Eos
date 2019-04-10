using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EosShared;
using LighthouseClientExample.Command;
using LighthouseClientExample.Observer;

namespace LighthouseClientExample.Controller
{
    public class LighthouseController : IObserver
    {
        private Client client = null;
        private bool active = false;
        public MainWindow mw;

        private CommandManager cmdManager;

        public LighthouseController(MainWindow mw)
        {
            this.mw = mw;
            cmdManager = new CommandManager(this);
        }

        public void Start()
        {
            active = true;
            client = Client.Instance;
            mw.lbl_status.Content = "Active";
            client.Attach(this);
        }

        public void Stop()
        {
            active = false;
            client = Client.Instance;
            mw.lbl_status.Content = "Stopped";
            client.Detach(this);
        }

        public void Undo()
        {
            cmdManager.Undo();
        }

        public void Redo()
        {
            cmdManager.Redo();
        }

        public void Update(EosPacket packet)
        {
            cmdManager.AddMessage(Encoding.Unicode.GetString(packet.Data[0]));
        }
    }
}
