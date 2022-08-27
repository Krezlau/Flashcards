using Flashcards.Core.Models;
using Flashcards.Core.Services;
using Flashcards.Core.Stores;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;
using System;
using System.ComponentModel.DataAnnotations;
using System.Windows.Input;

namespace Flashcards.Core.ViewModels
{
    public class AddNewDeckViewModel : ObservableValidator
    {
        private readonly NavigationService<UserWelcomeViewModel> _userWelcomeService;
        private readonly NavigationService<DeckPreviewViewModel> _navigationService;
        private readonly IDialogService dialogService;
        private readonly UserDecksStore userDecksStore;

        public ICommand ButtonCommand { get; set; }

        public ICommand GoBackCommand { get; set; }

        private string deckName;
        public string DeckName
        {
            get => deckName;
            set => SetProperty(ref deckName, value);
        }

        public string BigText { get; set; } = "Create new deck";

        public string ButtonContent { get; set; } = "Add";

        public AddNewDeckViewModel(NavigationService<DeckPreviewViewModel> navigationService, UserDecksStore userDecksStore, IDialogService dialogService, NavigationService<UserWelcomeViewModel> userWelcomeService)
        {
            ButtonCommand = new RelayCommand(OnAddClick);
            if (userDecksStore.SelectionStore.SelectedDeck != null)
            {
                DeckName = userDecksStore.SelectionStore.SelectedDeck.Name;
                ButtonContent = "Edit";
                BigText = "Rename " + DeckName;
                ButtonCommand = new RelayCommand(OnEditClick);
            }

            GoBackCommand = new RelayCommand(OnGoBackClick);
            _navigationService = navigationService;
            this.userDecksStore = userDecksStore;
            this.dialogService = dialogService;
            _userWelcomeService = userWelcomeService;
        }

        private void OnGoBackClick()
        {
            _userWelcomeService.Navigate();
        }

        private async void OnEditClick()
        {
            if (UserInputValidator.ValidateDeckName(DeckName) == 1)
            {
                dialogService.ShowMessageDialog("ERROR", "Inserted name is too short.");
                return;
            }
            if (UserInputValidator.ValidateDeckName(DeckName) == 2)
            {
                dialogService.ShowMessageDialog("ERROR", "Inserted name is too long.");
                return;
            }
            
            bool outcome = await userDecksStore.AlterDeck(DeckName);
            if (outcome == false)
            {
                dialogService.ShowMessageDialog("ERROR", $"You already have a deck named {DeckName}.");
                return;
            }
            userDecksStore.SelectionStore.SelectedDeck.Name = DeckName;
            _navigationService.Navigate();
        }

        private async void OnAddClick()
        {
            if (UserInputValidator.ValidateDeckName(DeckName) == 1)
            {
                dialogService.ShowMessageDialog("ERROR", "Inserted name is too short.");
                return;
            }
            if (UserInputValidator.ValidateDeckName(DeckName) == 2)
            {
                dialogService.ShowMessageDialog("ERROR", "Inserted name is too long.");
                return;
            }
            Deck deck = new Deck(DeckName, userDecksStore.User.Id);
            bool outcome = await userDecksStore.AddNewDeck(deck);
            if (outcome == false)
            {
                dialogService.ShowMessageDialog("ERROR", $"You already have a deck named {DeckName}.");
                return;
            }
            dialogService.ShowSnackbarMessage("Success", "Deck created.");
            userDecksStore.SelectionStore.SelectedDeck = deck;
            _navigationService.Navigate();
        }
    }
}
