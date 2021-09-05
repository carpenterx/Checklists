using Checklists.Properties;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Checklists
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void OpenChecklistFile(object sender, StartupEventArgs e)
        {
            string[] arguments = Environment.GetCommandLineArgs();

            if (arguments.GetLength(0) > 1)
            {
                if (arguments[1].EndsWith(Settings.Default.ChecklistExtension))
                {
                    OpenMainWindow(arguments[1]);
                }
                else
                {
                    OpenMainWindow();
                }
            }
            else
            {
                OpenMainWindow();
            }
        }

        private static void OpenMainWindow(string arguments = "")
        {
            MainWindow mainWindow;
            if (arguments == "")
            {
                mainWindow = new MainWindow();
            }
            else
            {
                mainWindow = new MainWindow(arguments);
            }

            mainWindow.Show();
        }
    }
}
