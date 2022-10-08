using Flashcards.Core.Models;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards.Core.Stores
{
    public class SelectionStore : ObservableObject
    {
        private Deck _selectedDeck;
        public Deck SelectedDeck
        {
            get => _selectedDeck;
            set => SetProperty(ref _selectedDeck, value);
        }

        private Flashcard _selectedFlashcard;
        public Flashcard SelectedFlashcard
        {
            get => _selectedFlashcard;
            set => SetProperty(ref _selectedFlashcard, value);
        }

        public int GetSelectedDeckIndex(User user)
        {
            if (user.Decks is null) return -1;
            return user.Decks.IndexOf(SelectedDeck);
        }

        public int GetSelectedFlashcardIndex()
        {
            if (SelectedDeck.Flashcards is null) return -1;
            return SelectedDeck.Flashcards.IndexOf(SelectedFlashcard);
        }
    }
}
