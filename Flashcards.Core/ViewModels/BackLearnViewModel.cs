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
        private readonly ReviewStore _reviewStore;

        public ICommand GoBackCommand { get; set; }

        public ICommand GoodCommand { get; set; }

        public ICommand AgainCommand { get; set; }

        public string Front => _reviewStore.ToReviewList[_reviewStore.Iterator].Front;

        public string Back => _reviewStore.ToReviewList[_reviewStore.Iterator].Back;

        public BackLearnViewModel(UserDecksStore userDecksStore, NavigationService<DeckPreviewViewModel> deckPreviewService, NavigationService<FrontLearnViewModel> frontLearnService, ReviewStore reviewStore)
        {
            _userDecksStore = userDecksStore;
            _deckPreviewService = deckPreviewService;
            _frontLearnService = frontLearnService;
            _reviewStore = reviewStore;

            GoodCommand = new RelayCommand(OnGoodClick);
            AgainCommand = new RelayCommand(OnAgainClick);
            GoBackCommand = new RelayCommand(OnGoBackClick);
        }

        private void OnGoBackClick()
        {
            _reviewStore.EndOfLearning();
            _deckPreviewService.Navigate();
        }

        private async void OnAgainClick()
        {
            await _userDecksStore.FlashcardSetReviewFailed(_reviewStore.ToReviewList[_reviewStore.Iterator]);
            _reviewStore.Again();
            FlashcardDone();
        }

        private async void OnGoodClick()
        {
            await _userDecksStore.FlashcardSetReview(_reviewStore.ToReviewList[_reviewStore.Iterator]);
            _reviewStore.Good();
            FlashcardDone();
        }

        private void FlashcardDone()
        {
            if (_reviewStore.Iterator >= _reviewStore.ToReviewList.Count)
            {
                _reviewStore.EndOfLearning();
                _deckPreviewService.Navigate();
                return;
            }
            _frontLearnService.Navigate();
        }
    }
}
