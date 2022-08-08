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
        private readonly NavigationService<DeckPreviewViewModel> _navigationService;
        private readonly IDialogService dialogService;
        private readonly UserDecksStore userDecksStore;

        public ICommand AddCommand { get; set; }

        private string deckName;
        public string DeckName
        {
            get => deckName;
            set => SetProperty(ref deckName, value);
        }

        public string ButtonContent { get; set; } = "Add";

        public AddNewDeckViewModel(NavigationService<DeckPreviewViewModel> navigationService, UserDecksStore userDecksStore, IDialogService dialogService)
        {
            AddCommand = new RelayCommand(OnAddClick);
            if (userDecksStore.SelectionStore.SelectedDeck != null)
            {
                DeckName = userDecksStore.SelectionStore.SelectedDeck.Name;
                ButtonContent = "Edit";
                AddCommand = new RelayCommand(OnEditClick);
            }

            _navigationService = navigationService;
            this.userDecksStore = userDecksStore;
            this.dialogService = dialogService;
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
            userDecksStore.SelectionStore.SelectedDeck.Name = DeckName;
            await userDecksStore.AlterDeck(DeckName);
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
            Deck deck = new Deck(DeckName, userDecksStore.User.Name);
            userDecksStore.SelectionStore.SelectedDeck = deck;
            await userDecksStore.AddNewDeck(deck);
            _navigationService.Navigate();
        }
    }
}
