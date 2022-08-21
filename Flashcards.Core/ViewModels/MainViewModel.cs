using Flashcards.Core.Stores;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards.Core.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        private readonly NavigationService _navigationStore;

        public MainViewModel(NavigationService navigationStore)
        {
            _navigationStore = navigationStore;

            _navigationStore.CurrentViewModelChanged += OnCurrentViewModelChanged;
            _navigationStore.LeftViewModelChanged += OnLeftViewModelChanged;
            _navigationStore.RightViewModelChanged += OnRightViewModelChanged;
        }

        private void OnRightViewModelChanged()
        {
            OnPropertyChanged(nameof(RightViewModel));
        }

        private void OnLeftViewModelChanged()
        {
            OnPropertyChanged(nameof(LeftViewModel));
        }

        private void OnCurrentViewModelChanged()
        {
            OnPropertyChanged(nameof(CurrentViewModel));
        }

        public ObservableObject CurrentViewModel => _navigationStore.CurrentViewModel;
        public ObservableObject LeftViewModel => _navigationStore.LeftViewModel;
        public ObservableObject RightViewModel => _navigationStore.RightViewModel;
    }
}