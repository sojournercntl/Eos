using System.Windows.Controls;
using System.Windows.Documents;

namespace LighthouseClientExample.Command
{
    public interface ICommand
    {
        void Undo();

        void Redo();

    }
}