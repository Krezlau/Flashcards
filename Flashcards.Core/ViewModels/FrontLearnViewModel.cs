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
    public class FrontLearnViewModel : ObservableObject
    {
        private readonly UserDecksStore _userDecksStore;
        private readonly NavigationService<DeckPreviewViewModel> _deckPreviewService;
        private readonly NavigationService<BackLearnViewModel> _backLearnService;

        public ICommand GoBackCommand { get; set; }

        public ICommand FlipCommand { get; set; }

        public string Front => _userDecksStore.SelectionStore.SelectedFlashcard.Front;

        public FrontLearnViewModel(UserDecksStore userDecksStore, NavigationService<DeckPreviewViewModel> deckPreviewService, NavigationService<BackLearnViewModel> backLearnService)
        {
            _userDecksStore = userDecksStore;
            _deckPreviewService = deckPreviewService;
            _backLearnService = backLearnService;

            FlipCommand = new RelayCommand(OnFlipClick);
            GoBackCommand = new RelayCommand(OnGoBackClick);
        }

        private void OnFlipClick()
        {
            // some stuff
            _backLearnService.Navigate();
        }

        private void OnGoBackClick()
        {
            _deckPreviewService.Navigate();
        }
    }
}
