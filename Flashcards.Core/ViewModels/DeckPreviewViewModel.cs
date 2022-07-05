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
        private readonly NavigationService<UserWelcomeViewModel> _userWelcomeNavigatonService;
        private readonly NavigationService<FlashcardManagementViewModel> _flashcardManagementService;
        private readonly UserDecksStore userDecksStore;

        private readonly Deck _currentDeck;

        public string CurrentDeckName { get; set; }

        public string CurrentDeckSize { get; set; }

        public ICommand LearnCommand { get; set; }

        public ICommand ManageFlashcardsCommand { get; set; }

        public ICommand DeleteDeckCommand { get; set; }

        public DeckPreviewViewModel(NavigationService<AddNewFlashcardViewModel> newFlashcardNavigationService, UserDecksStore userDecksStore, NavigationService<UserWelcomeViewModel> userWelcomeNavigatonService, NavigationService<FlashcardManagementViewModel> flashcardManagementService)
        {
            _newFlashcardNavigationService = newFlashcardNavigationService;
            this.userDecksStore = userDecksStore;
            _currentDeck = userDecksStore.SelectedDeck;

            if (_currentDeck != null)
            {
                CurrentDeckName = _currentDeck.Name;
                CurrentDeckSize = "Flashcards: " + _currentDeck.Flashcards.Count;
            }
            LearnCommand = new RelayCommand(OnLearnClick);
            ManageFlashcardsCommand = new RelayCommand(OnManageClick);
            DeleteDeckCommand = new RelayCommand(OnDeleteClick);
            _userWelcomeNavigatonService = userWelcomeNavigatonService;
            _flashcardManagementService = flashcardManagementService;
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
            _newFlashcardNavigationService.Navigate();
        }
    }
}
