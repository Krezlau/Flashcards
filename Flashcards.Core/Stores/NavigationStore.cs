using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards.Core.Stores
{
    public class NavigationService
    {
        private ObservableObject _currentViewModel;
        public ObservableObject CurrentViewModel
        {
            get => _currentViewModel;
            set
            {
                _currentViewModel = value;
                OnCurrentViewModelChanged();
            }
        }

        private ObservableObject _leftViewModel;
        public ObservableObject LeftViewModel
        {
            get => _leftViewModel;
            set
            {
                _leftViewModel = value;
                OnLeftViewModelChanged();
            }
        }

        private ObservableObject _rightViewModel;
        public ObservableObject RightViewModel
        {
            get => _rightViewModel;
            set
            {
                _rightViewModel = value;
                OnRightViewModelChanged();
            }
        }

        public event Action LeftViewModelChanged;

        private void OnLeftViewModelChanged()
        {
            LeftViewModelChanged?.Invoke();
        }

        public event Action RightViewModelChanged;

        private void OnRightViewModelChanged()
        {
            RightViewModelChanged?.Invoke();
        }

        public event Action CurrentViewModelChanged;

        private void OnCurrentViewModelChanged()
        {
            CurrentViewModelChanged?.Invoke();
        }
    }
}
