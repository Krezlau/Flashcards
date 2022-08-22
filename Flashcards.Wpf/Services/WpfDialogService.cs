using Flashcards.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Flashcards.WpfApp.Services
{
    public class WpfDialogService : IDialogService
    {
        private readonly MainWindow _mainWindow;

        public WpfDialogService(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
        }

        public void ShowMessageDialog(string title, string message)
        {
            Dialog dialog = new Dialog(_mainWindow, title, message);
            dialog.ShowDialog();
        }
    }
}
