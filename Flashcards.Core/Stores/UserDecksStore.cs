using Flashcards.Core.Models;
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
