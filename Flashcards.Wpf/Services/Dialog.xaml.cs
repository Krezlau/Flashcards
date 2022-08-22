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
using System.Windows.Shapes;

namespace Flashcards.WpfApp.Services
{
    /// <summary>
    /// Interaction logic for Dialog.xaml
    /// </summary>
    public partial class Dialog : Window
    {
        public Dialog(MainWindow mainWindow, string title, string message)
        {
            InitializeComponent();
            this.Owner = mainWindow;
            this.Title = title;
            Label.Text = message;
            this.KeyDown += OnKeyClick;
        }

        private void OnKeyClick(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.Close();
            }
        }

        public void OnButtonClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
