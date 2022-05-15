using Flashcards.Core.Commands;
using Flashcards.Core.Messages;
using Flashcards.Core.Models;
using Flashcards.Core.Services;
using Flashcards.Core.Stores;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Flashcards.Core.ViewModels
{
    public class HomeViewModel : ObservableRecipient
    {
        private readonly NavigationService<AddNewDeckViewModel> _navigationService;

        public UserDecksModel CurrentUserDecks { get; set; }
        public ICommand AddNewDeckCommand { get; set; }

        private Deck currentDeck;

        public Deck CurrentDeck
        {
            get => currentDeck;
            set => SetProperty(ref currentDeck, value);
        }

        public HomeViewModel(NavigationStore navigationStore)
        {
            CurrentUserDecks = new UserDecksModel { DeckList = new ObservableCollection<Deck>() };
            CurrentUserDecks.DeckList.Add(new Deck { Name = "lmao" });
            CurrentUserDecks.DeckList.Add(new Deck { Name = "xd" });
            CurrentUserDecks.DeckList.Add(new Deck { Name = "fajny deck" });
           // AddNewDeckCommand = new NavigateCommand<AddNewDeckViewModel>(new NavigationService<AddNewDeckViewModel>(
             //   navigationStore, () => new AddNewDeckViewModel(navigationStore)));
            AddNewDeckCommand = new RelayCommand(OnAddNewDeckClick);
            _navigationService = new NavigationService<AddNewDeckViewModel>(
                navigationStore, () => new AddNewDeckViewModel(navigationStore));
            
        }

        private void OnAddNewDeckClick()
        {
            _navigationService.Navigate();
        }

        protected override void OnActivated()
        {
            Messenger.Register<HomeViewModel, NewDeckMessage>(this, (r, m) =>
            {
                r.CurrentUserDecks.DeckList.Add(new Deck { Name = m.Value });
            });
            
        }
    }
}
