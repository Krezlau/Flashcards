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
        private ContentControl _content;

        public void SetLoadingScreen(Border loadingScreen)
        {
            _loadingScreen = loadingScreen;
        }

        public void SetContent(ContentControl content)
        {
            _content = content;
        }

        public ContentControl GetContent()
        {
            if (_content is null)
            {
                throw new InvalidOperationException();
            }

            return _content;
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
