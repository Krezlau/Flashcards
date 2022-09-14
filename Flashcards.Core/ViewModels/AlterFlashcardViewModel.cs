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

        private string _backErrorText = "";
        public string BackErrorText
        {
            get => _backErrorText;
            set => SetProperty(ref _backErrorText, value);
        }

        private string _frontErrorText = "";
        public string FrontErrorText
        {
            get => _frontErrorText;
            set => SetProperty(ref _frontErrorText, value);
        }

        private string front;
        public string Front
        {
            get => front;
            set
            {
                if (UserInputValidator.ValidateFlashcardTextField(value) == 1)
                {
                    FrontErrorText = "Front text too short.";
                    front = value;
                    return;
                }
                if (UserInputValidator.ValidateFlashcardTextField(value) == 2)
                {
                    FrontErrorText = "Front text too long. Must be no longer than 100 characters.";
                    front = value;
                    return;
                }
                FrontErrorText = "";
                front = value;
            }
        }

        private string back;
        public string Back
        {
            get => back;
            set
            {
                if (UserInputValidator.ValidateFlashcardTextField(value) == 1)
                {
                    BackErrorText = "Back text too short.";
                    back = value;
                    return;
                }
                if (UserInputValidator.ValidateFlashcardTextField(value) == 2)
                {
                    BackErrorText = "Back text too long. Must be no longer than 100 characters.";
                    back = value;
                    return;
                }
                BackErrorText = "";
                back = value;
            }
        }

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
            _dialogService.ShowSnackbarMessage("SUCCESS", "Flashcard changed.");
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
            _dialogService.ShowSnackbarMessage("SUCCESS", "Flashcard created.");
            _navigationService.Navigate();
        }

        public ICommand ButtonCommand { get; set; }
    }
}
