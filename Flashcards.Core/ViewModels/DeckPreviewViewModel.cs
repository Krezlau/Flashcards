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
        private readonly NavigationService<AddNewDeckViewModel> _alterDeckService;
        private readonly UserDecksStore userDecksStore;

        private readonly Deck _currentDeck;

        private string _currentDeckName;
        public string CurrentDeckName
        {
            get => _currentDeckName;
            set => SetProperty(ref _currentDeckName, value);
        }

        public string CurrentDeckSize { get; set; }

        public ICommand LearnCommand { get; set; }

        public ICommand ManageFlashcardsCommand { get; set; }

        public ICommand DeleteDeckCommand { get; set; }

        public ICommand RenameCommand { get; set; }

        public ICommand EscPressedCommand { get; set; }

        public DeckPreviewViewModel(UserDecksStore userDecksStore, NavigationService<UserWelcomeViewModel> userWelcomeNavigatonService, NavigationService<FlashcardManagementViewModel> flashcardManagementService, NavigationService<AddNewDeckViewModel> alterDeckService)
        {
            this.userDecksStore = userDecksStore;
            _currentDeck = userDecksStore.SelectionStore.SelectedDeck;

            if (_currentDeck != null)
            {
                CurrentDeckName = _currentDeck.Name;
                CurrentDeckSize = "Flashcards: " + _currentDeck.Flashcards.Count;
            }
            LearnCommand = new RelayCommand(OnLearnClick);
            ManageFlashcardsCommand = new RelayCommand(OnManageClick);
            DeleteDeckCommand = new RelayCommand(OnDeleteClick);
            RenameCommand = new RelayCommand(OnRenameClick);
            EscPressedCommand = new RelayCommand(OnEscPressed);
            _userWelcomeNavigatonService = userWelcomeNavigatonService;
            _flashcardManagementService = flashcardManagementService;
            _alterDeckService = alterDeckService;
        }

        private void OnEscPressed()
        {
            CurrentDeckName = _currentDeck.Name;
        }

        private async void OnRenameClick()
        {
            await userDecksStore.AlterDeck(CurrentDeckName);
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
            //TODO
        }
    }
}
