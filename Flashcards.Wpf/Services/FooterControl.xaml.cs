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
using Wpf.Ui.Controls;
using Wpf.Ui.Controls.Interfaces;

namespace Flashcards.WpfApp.Services
{
    /// <summary>
    /// Interaction logic for FooterControl.xaml
    /// </summary>
    public partial class FooterControl : UserControl
    {

        private readonly IDialogControl owner;

        public FooterControl(IDialogControl owner)
        {
            InitializeComponent();
            this.owner = owner;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            owner.Hide();
        }
    }
}
