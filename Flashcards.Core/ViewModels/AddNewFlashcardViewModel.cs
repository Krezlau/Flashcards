using Flashcards.Core.Services;
using Flashcards.Core.Stores;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Flashcards.Core.ViewModels
{
    public class AddNewFlashcardViewModel : ObservableObject
    {
        private readonly NavigationService<DeckPreviewViewModel> _navigationService;
        private readonly UserDecksStore _userDecksStore;

        public AddNewFlashcardViewModel(NavigationService<DeckPreviewViewModel> navigationService, UserDecksStore userDecksStore)
        {
            _navigationService = navigationService;
            _userDecksStore = userDecksStore;
            AddCommand = new RelayCommand(OnAddClick);
        }

        private void OnAddClick()
        { 
            _navigationService.Navigate();
        }

        private string front;
        public string Front
        {
            get => front;
            set => SetProperty(ref front, value);
        }

        private string back;
        public string Back
        {
            get => back;
            set => SetProperty(ref back, value);
        }

        public ICommand AddCommand { get; set; }
    }
}
