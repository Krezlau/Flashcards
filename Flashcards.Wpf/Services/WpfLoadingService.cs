using Flashcards.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards.WpfApp.Services
{
    public class WpfLoadingService : ILoadingService
    {
        private readonly LoadingRingService _loadingRingService;

        public WpfLoadingService(LoadingRingService loadingRingService)
        {
            _loadingRingService = loadingRingService;
        }

        public void Enable()
        {
            _loadingRingService.GetLoadingScreen().Visibility = System.Windows.Visibility.Visible;
        }

        public void Disable()
        {
            _loadingRingService.GetLoadingScreen().Visibility = System.Windows.Visibility.Hidden;
        }
    }
}
