using Flashcards.Core.Models;
using Flashcards.Core.Services;
using Flashcards.Core.Stores;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Flashcards.Core.ViewModels
{
    public class DeckPreviewViewModel : ObservableObject
    {
        private readonly NavigationService<AddNewFlashcardViewModel> _newFlashcardNavigationService;
        private readonly UserDecksStore userDecksStore;

        private readonly Deck _currentDeck;

        public string CurrentDeckName { get; set; }

        public string CurrentDeckSize { get; set; }

        public ICommand LearnCommand { get; set; }

        public ICommand ManageFlashcardsCommand { get; set; }

        public DeckPreviewViewModel(NavigationService<AddNewFlashcardViewModel> newFlashcardNavigationService, UserDecksStore userDecksStore)
        {
            _newFlashcardNavigationService = newFlashcardNavigationService;
            this.userDecksStore = userDecksStore;
            _currentDeck = userDecksStore.SelectedDeck;

            if (_currentDeck != null)
            {
                CurrentDeckName = _currentDeck.Name;
                CurrentDeckSize = "Flashcards: " + _currentDeck.Size;
            }
            LearnCommand = new RelayCommand(OnLearnClick);
            ManageFlashcardsCommand = new RelayCommand(OnManageClick);
        }

        private void OnManageClick()
        {
            //there needs to be navigate to some home view or something
            userDecksStore.RemoveCurrentDeck();
        }

        private void OnLearnClick()
        {
            _newFlashcardNavigationService.Navigate();
        }
    }
}
