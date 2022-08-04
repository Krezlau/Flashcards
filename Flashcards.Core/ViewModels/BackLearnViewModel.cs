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
    public class BackLearnViewModel : ObservableObject
    {
        private readonly UserDecksStore _userDecksStore;
        private readonly NavigationService<DeckPreviewViewModel> _deckPreviewService;
        private readonly NavigationService<FrontLearnViewModel> _frontLearnService;

        public ICommand GoBackCommand { get; set; }

        public ICommand GoodCommand { get; set; }

        public ICommand AgainCommand { get; set; }

        public string Front => _userDecksStore.SelectionStore.SelectedFlashcard.Front;

        public string Back => _userDecksStore.SelectionStore.SelectedFlashcard.Back;

        public BackLearnViewModel(UserDecksStore userDecksStore, NavigationService<DeckPreviewViewModel> deckPreviewService, NavigationService<FrontLearnViewModel> frontLearnService)
        {
            _userDecksStore = userDecksStore;
            _deckPreviewService = deckPreviewService;
            _frontLearnService = frontLearnService;

            GoodCommand = new RelayCommand(OnGoodClick);
            AgainCommand = new RelayCommand(OnAgainClick);
            GoBackCommand = new RelayCommand(OnGoBackClick);
        }

        private void OnGoBackClick()
        {
            _deckPreviewService.Navigate();
        }

        private void OnAgainClick()
        {
            throw new NotImplementedException();
        }

        private void OnGoodClick()
        {
            throw new NotImplementedException();
        }
    }
}
