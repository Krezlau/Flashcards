using Flashcards.Core.Models;
using Flashcards.Core.Services.UserDataCreators;
using Flashcards.Core.Services.UserDataDestroyers;
using Flashcards.Core.Services.UserDataProviders;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards.Core.Stores
{
    public class UserDecksStore : ObservableObject
    {
        private readonly IUserDataProvider _dataProvider;
        private readonly IUserDataCreator _dataCreator;
        private readonly IUserDataDestroyer _dataDestroyer;
        private string _username = "lmao";

        public UserDecksStore(IUserDataProvider dataProvider, IUserDataCreator dataCreator, IUserDataDestroyer dataDestroyer)
        {
            _dataProvider = dataProvider;
            _dataCreator = dataCreator;
            _dataDestroyer = dataDestroyer;
        }

        public void Initialize()
        {
            User user = _dataProvider.LoadUserDecks(_username);
            User = user;
        }

        public async Task AddNewDeck(Deck deck)
        {
            User.Decks.Add(deck);
            await _dataCreator.SaveNewDeck(deck);
        }

        public async Task RemoveCurrentDeck()
        {
            await _dataDestroyer.DeleteDeck(SelectedDeck);
            User.Decks.Remove(SelectedDeck);
        }

        private User _user;
        public User User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }

        private Deck _selectedDeck;
        public Deck SelectedDeck
        {
            get => _selectedDeck;
            set => SetProperty(ref _selectedDeck, value);
        }

        public void AddFlashcardToSelectedDeck(Flashcard flashcard)
        {

        }
    }
}
