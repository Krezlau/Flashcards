using Flashcards.Core.Models;
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
        private readonly NavigationService<FlashcardManagementViewModel> _navigationService;
        private readonly UserDecksStore _userDecksStore;

        public ICommand GoBackCommand { get; set; }

        public AddNewFlashcardViewModel(NavigationService<FlashcardManagementViewModel> navigationService, UserDecksStore userDecksStore)
        {
            _navigationService = navigationService;
            _userDecksStore = userDecksStore;
            AddCommand = new RelayCommand(OnAddClick);
            GoBackCommand = new RelayCommand(OnGoBackClick);
        }

        private void OnGoBackClick()
        {
            _navigationService.Navigate();
        }

        private async void OnAddClick()
        {
            await _userDecksStore.AddFlashcardToSelectedDeck(new Flashcard
            {
                Front = Front,
                Back = Back,
                DeckId = _userDecksStore.SelectedDeck.Id,
                Level = 0,
                NextReview = DateTime.Now
            });
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
