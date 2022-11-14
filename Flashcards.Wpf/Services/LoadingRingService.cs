using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Wpf.Ui.Controls;

namespace Flashcards.WpfApp.Services
{
    public class LoadingRingService
    {
        private Border _loadingScreen;

        public void SetLoadingScreen(Border loadingScreen)
        {
            _loadingScreen = loadingScreen;
        }

        public Border GetLoadingScreen()
        {
            if (_loadingScreen is null)
            {
                throw new InvalidOperationException();
            }

            return _loadingScreen;
        }
    }
}
