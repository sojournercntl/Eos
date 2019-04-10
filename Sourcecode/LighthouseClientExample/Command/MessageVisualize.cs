using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LighthouseClientExample.View;

namespace LighthouseClientExample.Command
{
    public class MessageVisualize : ICommand
    {
        private MainWindow mw;
        private VisualNotification vn;

        public MessageVisualize(VisualNotification v, MainWindow m)
        {
            mw = m;
            vn = v;
            mw.stack_messages.Children.Add(vn);
        }

        public void Undo()
        {
            mw.stack_messages.Children.Remove(vn);
        }

        public void Redo()
        {
            mw.stack_messages.Children.Add(vn);
        }
    }
}
