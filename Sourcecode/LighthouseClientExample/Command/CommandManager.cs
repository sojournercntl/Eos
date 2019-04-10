using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Threading;
using LighthouseClientExample.Controller;
using LighthouseClientExample.View;

namespace LighthouseClientExample.Command
{
    public class CommandManager
    {
        private List<ICommand> undoStack = new List<ICommand>();
        private List<ICommand> redoStack = new List<ICommand>();

        private LighthouseController controller;

        public CommandManager(LighthouseController cntl)
        {
            controller = cntl;
        }

        public void AddMessage(string message)
        {
            controller.mw.Dispatcher.Invoke(new Action(() =>
            {
                VisualNotification vn = new VisualNotification();
                vn.lbl_message.Content = message;
                MessageVisualize m = new MessageVisualize(vn, controller.mw);
                undoStack.Add(m);
            }));
        }

        public void Undo()
        {
            ICommand com = undoStack[undoStack.Count - 1];
            com.Undo();
            undoStack.Remove(com);
            redoStack.Add(com);
        }

        public void Redo()
        {
            ICommand com = redoStack[redoStack.Count - 1];
            com.Redo();
            redoStack.Remove(com);
            undoStack.Add(com);
        }

    }
}
