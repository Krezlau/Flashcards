﻿using Flashcards.Core.Models;
using Flashcards.Core.Services;
using Flashcards.Core.Stores;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace Flashcards.Core.ViewModels
{
    public class HomeViewModel : ObservableRecipient
    {
        private readonly NavigationService<AddNewDeckViewModel> _newDeckNavigationService;
        private readonly NavigationService<DeckPreviewViewModel> _deckPreviewNavigationService;
        private readonly UserDecksStore userDecksStore;

        public ObservableCollection<Deck> Decks => userDecksStore.User.Decks;

        public Deck SelectedDeck
        {
            get => userDecksStore.SelectionStore.SelectedDeck;
            set
            {
                userDecksStore.SelectionStore.SelectedDeck = value;
                OnDeckSelect();
            }
        }

        public ICommand AddNewDeckCommand { get; set; }

        public HomeViewModel(NavigationService<DeckPreviewViewModel> deckPreviewNavigationService,
                            NavigationService<AddNewDeckViewModel> newDeckNavigationService,
                            UserDecksStore userDecksStore)
        {
            AddNewDeckCommand = new RelayCommand(OnAddNewDeckClick);
            this.userDecksStore = userDecksStore;
            _deckPreviewNavigationService = deckPreviewNavigationService;
            _newDeckNavigationService = newDeckNavigationService;
        }

        private void OnDeckSelect()
        {
            _deckPreviewNavigationService.Navigate();
        }

        private void OnAddNewDeckClick()
        {
            userDecksStore.SelectionStore.SelectedDeck = null;
           _newDeckNavigationService.Navigate();
        }

        
    }
}
