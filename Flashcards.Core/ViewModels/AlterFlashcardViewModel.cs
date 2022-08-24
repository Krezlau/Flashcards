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
        private readonly IDialogService _dialogService;
        private readonly UserDecksStore _userDecksStore;

        public ICommand GoBackCommand { get; set; }

        public string ButtonContent { get; set; } = "Add";

        public string TopText { get; set; }

        public AlterFlashcardViewModel(NavigationService<FlashcardManagementViewModel> navigationService, UserDecksStore userDecksStore, IDialogService dialogService)
        {
            _navigationService = navigationService;
            _userDecksStore = userDecksStore;
            ButtonCommand = new RelayCommand(OnAddClick);
            GoBackCommand = new RelayCommand(OnGoBackClick);

            TopText = $"{_userDecksStore.SelectionStore.SelectedDeck.Name}: add new flashcard";

            if (_userDecksStore.SelectionStore.SelectedFlashcard != null)
            {
                Front = _userDecksStore.SelectionStore.SelectedFlashcard.Front;
                Back = _userDecksStore.SelectionStore.SelectedFlashcard.Back;
                ButtonContent = "Edit";
                TopText = $"{_userDecksStore.SelectionStore.SelectedDeck.Name}: change flashcard";
                ButtonCommand = new RelayCommand(OnEditClick);
            }
            _dialogService = dialogService;
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
            if (UserInputValidator.ValidateFlashcardTextField(Front) == 1 || UserInputValidator.ValidateFlashcardTextField(Back) == 1)
            {
                _dialogService.ShowMessageDialog("ERROR", "Flashcard text fields must be at least 3 characters long.");
                return;
            }
            if (UserInputValidator.ValidateFlashcardTextField(Front) == 2 || UserInputValidator.ValidateFlashcardTextField(Back) == 2)
            {
                _dialogService.ShowMessageDialog("ERROR", "Flashcard text fields must be not longer than 100 characters.");
                return;
            }
            await _userDecksStore.AddFlashcardToSelectedDeck(new Flashcard
            {
                Front = Front,
                Back = Back,
                DeckId = _userDecksStore.SelectionStore.SelectedDeck.Id,
                Level = 0,
                NextReview = DateTime.Today
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
