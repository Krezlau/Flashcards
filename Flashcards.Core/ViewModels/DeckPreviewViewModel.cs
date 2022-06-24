using Flashcards.Core.Models;
using Flashcards.Core.Services;
using Flashcards.Core.Stores;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards.Core.ViewModels
{
    public class DeckPreviewViewModel : ObservableObject
    {
        private readonly NavigationService<AddNewFlashcardViewModel> _newFlashcardNavigationService;
        private readonly UserDecksStore userDecksStore;

        private readonly Deck _currentDeck;

        public string CurrentDeckName { get; set; }

        public string CurrentDeckSize { get; set; }

        public DeckPreviewViewModel(NavigationStore navigationStore, UserDecksStore userDecksStore)
        {
            _newFlashcardNavigationService = new NavigationService<AddNewFlashcardViewModel>(navigationStore, () => new AddNewFlashcardViewModel(navigationStore));
            this.userDecksStore = userDecksStore;
            _currentDeck = userDecksStore.SelectedDeck;

            CurrentDeckName = _currentDeck.Name;
            CurrentDeckSize = "Flashcards: " + _currentDeck.Size;
        }
    }
}
