using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LighthouseClientExample.Controller;

namespace LighthouseClientExample
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private LighthouseController controller;

        public MainWindow()
        {
            InitializeComponent();
            this.controller = new LighthouseController(this);
        }

        private void Btn_start_Click(object sender, RoutedEventArgs e)
        {
            controller.Start();
        }

        private void Btn_stop_Click(object sender, RoutedEventArgs e)
        {
            controller.Stop();
        }

        private void Btn_undo_Click(object sender, RoutedEventArgs e)
        {
            controller.Undo();
        }

        private void Btn_redo_Click(object sender, RoutedEventArgs e)
        {
            controller.Redo();
        }
    }
}
