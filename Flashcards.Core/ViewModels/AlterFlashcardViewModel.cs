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
    public class AlterFlashcardViewModel : ObservableObject
    {
        private readonly NavigationService<FlashcardManagementViewModel> _navigationService;
        private readonly UserDecksStore _userDecksStore;

        public ICommand GoBackCommand { get; set; }

        public string ButtonContent { get; set; } = "Add";

        public AlterFlashcardViewModel(NavigationService<FlashcardManagementViewModel> navigationService, UserDecksStore userDecksStore)
        {
            _navigationService = navigationService;
            _userDecksStore = userDecksStore;
            ButtonCommand = new RelayCommand(OnAddClick);
            GoBackCommand = new RelayCommand(OnGoBackClick);

            if (_userDecksStore.SelectionStore.SelectedFlashcard != null)
            {
                Front = _userDecksStore.SelectionStore.SelectedFlashcard.Front;
                Back = _userDecksStore.SelectionStore.SelectedFlashcard.Back;
                ButtonContent = "Edit";
                ButtonCommand = new RelayCommand(OnEditClick);
            }
        }

        private async void OnEditClick()
        {
            await _userDecksStore.AlterFlashcard(Front, Back);
            _navigationService.Navigate();
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
                DeckId = _userDecksStore.SelectionStore.SelectedDeck.Id,
                Level = 0,
                NextReview = DateTime.Now
            });
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

        public ICommand ButtonCommand { get; set; }
    }
}
