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
        private readonly ReviewStore _reviewStore;

        public ICommand GoBackCommand { get; set; }

        public ICommand FlipCommand { get; set; }

        public string Front => _reviewStore.ToReviewList[_reviewStore.Iterator].Front;

        public FrontLearnViewModel(UserDecksStore userDecksStore, NavigationService<DeckPreviewViewModel> deckPreviewService, NavigationService<BackLearnViewModel> backLearnService, ReviewStore reviewStore)
        {
            _userDecksStore = userDecksStore;
            _deckPreviewService = deckPreviewService;
            _backLearnService = backLearnService;
            _reviewStore = reviewStore;

            _reviewStore.StartSession();

            FlipCommand = new RelayCommand(OnFlipClick);
            GoBackCommand = new RelayCommand(OnGoBackClick);
        }

        private void OnFlipClick()
        {
            // some stuff
            _backLearnService.Navigate();
        }

        private async void OnGoBackClick()
        {
            await _userDecksStore.SaveSessionTime(_reviewStore.EndOfLearning());
            _deckPreviewService.Navigate();
        }
    }
}
