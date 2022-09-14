﻿using Flashcards.Core.Models;
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
            set
            {
                if (UserInputValidator.ValidateDeckName(value) == 1)
                {
                    ErrorText = "Name too short.";
                    deckName = value;
                    return;
                }
                if (UserInputValidator.ValidateDeckName(value) == 2)
                {
                    ErrorText = "Name too long.";
                    deckName = value;
                    return;
                }
                ErrorText = "";
                deckName = value;
            }
        }

        private string _errorText = "";
        public string ErrorText
        {
            get => _errorText;
            set => SetProperty(ref _errorText, value);
        }

        public string BigText { get; set; } = "Create new deck";

        public string ButtonContent { get; set; } = "Add";

        public AddNewDeckViewModel(NavigationService<DeckPreviewViewModel> navigationService, UserDecksStore userDecksStore, IDialogService dialogService, NavigationService<UserWelcomeViewModel> userWelcomeService)
        {
            ButtonCommand = new RelayCommand(OnAddClick);
            GoBackCommand = new RelayCommand(OnGoBackClick);
            if (userDecksStore.SelectionStore.SelectedDeck != null)
            {
                DeckName = userDecksStore.SelectionStore.SelectedDeck.Name;
                ButtonContent = "Edit";
                BigText = "Rename " + DeckName;
                ButtonCommand = new RelayCommand(OnEditClick);
            }

            _navigationService = navigationService;
            this.userDecksStore = userDecksStore;
            this.dialogService = dialogService;
            _userWelcomeService = userWelcomeService;
        }

        private void OnGoBackClick()
        {
            if (userDecksStore.SelectionStore.SelectedDeck != null)
            {
                _navigationService.Navigate();
                return;
            }
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
            dialogService.ShowSnackbarMessage("Success", "Deck's name changed.");
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
