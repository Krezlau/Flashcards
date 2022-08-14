using Flashcards.Core.Models;
using Flashcards.Core.Stores;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards.Core.ViewModels
{
    public class AccountManagementViewModel : ObservableObject
    {
        private readonly UserDecksStore _userDecksStore;

        public string Username => _userDecksStore.User.Name;

        public string Email { get; set; }

        public string UsernameText { get; set; }

        public string StreakTileText { get; set; }

        public string DeckTileText1 { get; set; }

        public string DeckTileText2 { get; set; }

        public string FlashcardTileText { get; set; }

        public AccountManagementViewModel(UserDecksStore userDecksStore)
        {
            _userDecksStore = userDecksStore;

            Deck largestDeck = FindLargestDeck();
            int flashcardCount = CountFlashcards();

            StreakTileText = "You have been learning for 5 days in a row. \n Keep going!"; //todo
            if (largestDeck is not null)
            {
                DeckTileText2 = "The biggest deck is " + largestDeck.Name + " - with " + largestDeck.Size + " flashcards inside.";
            }
            DeckTileText1 = "There are " + _userDecksStore.User.Decks.Count + " decks on your account.";
            FlashcardTileText = "You have currently " + flashcardCount + " flashcards across your decks!";

            UsernameText = "Username: " + _userDecksStore.User.Name;
            Email = "Email: " + _userDecksStore.User.Email;
        }

        private Deck FindLargestDeck()
        {
            int max = -1;
            Deck maxDeck = null;
            foreach (Deck d in _userDecksStore.User.Decks)
            {
                if (d.Flashcards.Count > max)
                {
                    max = d.Flashcards.Count;
                    maxDeck = d;
                }
            }
            return maxDeck;
        }

        private int CountFlashcards()
        {
            int sum = 0;
            foreach (Deck d in _userDecksStore.User.Decks)
            {
                sum += d.Flashcards.Count;
            }
            return sum;
        }

    }
}
