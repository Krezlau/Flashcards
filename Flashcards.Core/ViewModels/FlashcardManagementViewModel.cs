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
        private readonly IDialogService _dialogService;

        public ICommand NewFlashcardCommand { get; set; }

        public ICommand GoBackCommand { get; set; }

        public ICommand EditCommand { get; set; }

        public ICommand DeleteCommand { get; set; }

        public Flashcard SelectedFlashcard
        {
            get => _userDecksStore.SelectionStore.SelectedFlashcard;
            set => _userDecksStore.SelectionStore.SelectedFlashcard = value;
        }

        public ObservableCollection<Flashcard> Flashcards => _userDecksStore.SelectionStore.SelectedDeck.Flashcards;

        public FlashcardManagementViewModel(NavigationService<DeckPreviewViewModel> deckPreviewService, UserDecksStore userDecksStore, NavigationService<AlterFlashcardViewModel> newFlashcardService, IDialogService dialogService)
        {
            _deckPreviewService = deckPreviewService;
            _userDecksStore = userDecksStore;
            _newFlashcardService = newFlashcardService;

            NewFlashcardCommand = new RelayCommand(OnNewFlashcardClick);
            GoBackCommand = new RelayCommand(OnGoBackClick);
            EditCommand = new RelayCommand(OnEditClick);
            DeleteCommand = new RelayCommand(OnDeleteClick);
            _dialogService = dialogService;
        }

        private async void OnDeleteClick()
        {
            await _userDecksStore.RemoveCurrentFlashcard();
            _dialogService.ShowSnackbarMessage("SUCCESS", "Flashcard deleted.");
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
            _userDecksStore.SelectionStore.SelectedFlashcard = null;
            _newFlashcardService.Navigate();
        }
    }
}
