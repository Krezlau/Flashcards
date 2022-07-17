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
            for (int i = 0; i < user.Decks.Count; i++)
            {
                if (user.Decks[i].Id == SelectedDeck.Id)
                {
                    return i;
                }
            }
            return -1;
        }

        public int GetSelectedFlashcardIndex()
        {
            for (int i = 0; i <= SelectedDeck.Flashcards.Count; i++)
            {
                if (SelectedDeck.Flashcards[i].Id == SelectedFlashcard.Id)
                {
                    return i;
                }
            }
            return -1;
        }
    }
}
