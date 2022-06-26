using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards.Core.Models
{
    public class User : ObservableObject
    {
        public string Username { get; set; }
        private ObservableCollection<Deck> deckList;

        public ObservableCollection<Deck> DeckList
        {
            get => deckList;
            set => SetProperty(ref deckList, value);
        }

        public User(string username)
        {
            Username = username;
            DeckList = new ObservableCollection<Deck>();
        }

        public User(string username, ObservableCollection<Deck> decks)
        {
            Username = username;
            DeckList = decks;
        }
    }
}
