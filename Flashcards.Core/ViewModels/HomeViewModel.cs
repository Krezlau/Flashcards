using Flashcards.Core.Messages;
using Flashcards.Core.Models;
using Flashcards.Core.Services;
using Flashcards.Core.Stores;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;
using System;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Flashcards.Core.ViewModels
{
    public class HomeViewModel : ObservableRecipient
    {
        private readonly NavigationService<AddNewDeckViewModel> _newDeckNavigationService;
        private readonly NavigationService<DeckPreviewViewModel> _deckPreviewNavigationService;
        public readonly UserDecksStore userDecksStore;

        public ObservableCollection<Deck> Decks
        {
            get => userDecksStore.UserDecksModel.DeckList;
            set => userDecksStore.UserDecksModel.DeckList = value;
        }
        public Deck SelectedDeck
        {
            get => userDecksStore.SelectedDeck;
            set
            {
                userDecksStore.SelectedDeck = value;
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
           _newDeckNavigationService.Navigate();
        }

        
    }
}
