using System;
using System.Windows;

namespace NasaAPODViewer
{
    public class Program
    {
        [STAThread]
        public static void Main()
        {
            var application = new Application();
            var mainWindow = new MainWindow();
            application.Run(mainWindow);
        }
    }
}