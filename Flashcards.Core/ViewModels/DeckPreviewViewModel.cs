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
        private readonly NavigationService<UserWelcomeViewModel> _userWelcomeNavigatonService;
        private readonly NavigationService<FlashcardManagementViewModel> _flashcardManagementService;
        private readonly NavigationService<FrontLearnViewModel> _frontLearnService;
        private readonly NavigationService<AddNewDeckViewModel> _newDeckService;
        private readonly NavigationService<DeckActivityChartsViewModel> _deckActivityService;
        private readonly UserDecksStore userDecksStore;
        private readonly ReviewStore _reviewStore;
        private readonly IDialogService _dialogService;

        private readonly Deck _currentDeck;

        private string _currentDeckName;
        public string CurrentDeckName
        {
            get => _currentDeckName;
            set => SetProperty(ref _currentDeckName, value);
        }

        public string CurrentDeckSize { get; set; }

        public string CurrentDeckToReviewCount { get; set; }

        public ICommand LearnCommand { get; set; }

        public ICommand ManageFlashcardsCommand { get; set; }

        public ICommand DeleteDeckCommand { get; set; }

        public ICommand RenameCommand { get; set; }

        public ICommand DeckActivityCommand { get; set; }

        public ICommand FutureReviewsCommand { get; set; }


        public DeckPreviewViewModel(UserDecksStore userDecksStore, NavigationService<UserWelcomeViewModel> userWelcomeNavigatonService, NavigationService<FlashcardManagementViewModel> flashcardManagementService, NavigationService<FrontLearnViewModel> frontLearnService, ReviewStore reviewStore, NavigationService<AddNewDeckViewModel> newDeckService, IDialogService dialogService, NavigationService<DeckActivityChartsViewModel> deckActivityService)
        {
            this.userDecksStore = userDecksStore;
            _currentDeck = userDecksStore.SelectionStore.SelectedDeck;
            _reviewStore = reviewStore;

            if (_currentDeck != null)
            {
                _reviewStore.SelectedDeck = _currentDeck;
                CurrentDeckName = _currentDeck.Name;
                CurrentDeckSize = "Total number of flashcards: " + _currentDeck.Flashcards.Count;
                CurrentDeckToReviewCount = "To review: " + _reviewStore.ToReviewList.Count;
            }
            LearnCommand = new RelayCommand(OnLearnClick);
            ManageFlashcardsCommand = new RelayCommand(OnManageClick);
            DeleteDeckCommand = new RelayCommand(OnDeleteClick);
            RenameCommand = new RelayCommand(OnRenameClick);
            DeckActivityCommand = new RelayCommand(OnDeckActivityClick);
            FutureReviewsCommand = new RelayCommand(OnFutureReviewsClick);
            _userWelcomeNavigatonService = userWelcomeNavigatonService;
            _flashcardManagementService = flashcardManagementService;
            _frontLearnService = frontLearnService;
            _newDeckService = newDeckService;
            _dialogService = dialogService;
            _deckActivityService = deckActivityService;
        }

        private void OnFutureReviewsClick()
        {
            _dialogService.ShowSnackbarMessage("Coming soon!", "Feature not available yet.");
        }

        private void OnDeckActivityClick()
        {
            _deckActivityService.Navigate();
        }

        private void OnRenameClick()
        {
            _newDeckService.Navigate();
        }

        private async void OnDeleteClick()
        {
            await userDecksStore.RemoveCurrentDeck();
            _userWelcomeNavigatonService.Navigate();
        }

        private void OnManageClick()
        {
            _flashcardManagementService.Navigate();
        }

        private void OnLearnClick()
        {
            if (_reviewStore.ToReviewList.Count > 0)
            {
                _frontLearnService.Navigate();
                return;
            }
            if (_reviewStore.ToReviewList.Count == 0)
            {
                _dialogService.ShowMessageDialog("Done for now", "No flashcards to review for now! Come back tomorrow!");
            }
        }
    }
}
