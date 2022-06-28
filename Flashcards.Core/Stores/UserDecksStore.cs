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
            UserDecksModel = new User(_username);
            _dataProvider = dataProvider;
            _dataCreator = dataCreator;
            _dataDestroyer = dataDestroyer;
        }

        public void Initialize()
        {
            User user = _dataProvider.LoadUserDecks(_username);
            UserDecksModel = user;
        }

        public async Task AddNewDeck(Deck deck)
        {
            UserDecksModel.DeckList.Add(deck);
            await _dataCreator.SaveNewDeck(deck, _username);
        }

        public void RemoveCurrentDeck()
        {
            _dataDestroyer.DeleteDeck(SelectedDeck, _username);
            UserDecksModel.DeckList.Remove(SelectedDeck);
        }

        private User _userDecksModel;
        public User UserDecksModel
        {
            get => _userDecksModel;
            set => SetProperty(ref _userDecksModel, value);
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
