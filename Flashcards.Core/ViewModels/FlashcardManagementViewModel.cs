using Flashcards.Core.Models;
using Flashcards.Core.Services;
using Flashcards.Core.Stores;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Flashcards.Core.ViewModels
{
    public class FlashcardManagementViewModel : ObservableObject
    {
        private readonly UserDecksStore _userDecksStore;
        private readonly NavigationService<DeckPreviewViewModel> _deckPreviewService;
        private readonly NavigationService<AlterFlashcardViewModel> _newFlashcardService;

        public ICommand NewFlashcardCommand { get; set; }

        public ICommand GoBackCommand { get; set; }

        public ICommand EditCommand { get; set; }

        public Flashcard SelectedFlashcard
        {
            get => _userDecksStore.SelectedFlashcard;
            set => _userDecksStore.SelectedFlashcard = value;
        }

        public ObservableCollection<Flashcard> Flashcards => _userDecksStore.SelectedDeck.Flashcards;

        public FlashcardManagementViewModel(NavigationService<DeckPreviewViewModel> deckPreviewService, UserDecksStore userDecksStore, NavigationService<AlterFlashcardViewModel> newFlashcardService)
        {
            _deckPreviewService = deckPreviewService;
            _userDecksStore = userDecksStore;
            _newFlashcardService = newFlashcardService;

            NewFlashcardCommand = new RelayCommand(OnNewFlashcardClick);
            GoBackCommand = new RelayCommand(OnGoBackClick);
            EditCommand = new RelayCommand(OnEditClick);
        }

        private void OnEditClick()
        {
            if (SelectedFlashcard == null)
            {
                return;
            }
            _newFlashcardService.Navigate();
        }

        private void OnGoBackClick()
        {
            _deckPreviewService.Navigate();
        }

        private void OnNewFlashcardClick()
        {
            _userDecksStore.SelectedFlashcard = null;
            _newFlashcardService.Navigate();
        }
    }
}
