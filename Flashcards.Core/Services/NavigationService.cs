using Flashcards.Core.Stores;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards.Core.Services
{
    public class NavigationService<TViewModel>
        where TViewModel : ObservableObject
    {
        private readonly NavigationStore _navigationStore;
        private readonly Func<ObservableObject> _createViewModel;

        public NavigationService(NavigationStore navigationStore, Func<TViewModel> createViewModel)
        {
            _navigationStore = navigationStore;
            _createViewModel = createViewModel;
        }

        public void Navigate()
        {
            _navigationStore.CurrentViewModel = _createViewModel();
        }

        // propably will need to separate those two into different class
        public void NavigateLeft()
        {
            _navigationStore.LeftViewModel = _createViewModel();
        }

        public void NavigateRight()
        {
            _navigationStore.RightViewModel = _createViewModel();
        }
    }
}
