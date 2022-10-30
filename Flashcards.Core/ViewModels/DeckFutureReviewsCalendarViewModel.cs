using Flashcards.Core.Services;
using Flashcards.Core.Stores;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards.Core.ViewModels
{
    public class DeckFutureReviewsCalendarViewModel : FutureReviewCalendarBaseViewModel
    {
        private readonly NavigationService<DeckPreviewViewModel> _navService;
        private readonly NavigationService<FlashcardManagementViewModel> _flashcardsService;
        private readonly NavigationService<DeckActivityChartsViewModel> _chartsService;

        public DeckFutureReviewsCalendarViewModel(UserDecksStore userDecksStore,
                                                  NavigationService<DeckPreviewViewModel> navService,
                                                  NavigationService<FlashcardManagementViewModel> flashcardsService,
                                                  NavigationService<DeckActivityChartsViewModel> chartsService) : base(userDecksStore)
        {
            _navService = navService;
            _flashcardsService = flashcardsService;
            _chartsService = chartsService;

            bool outcome = _dataOrganizer.OrganizeFutureReviews(_userDecksStore.SelectionStore.SelectedDeck);
            if (!outcome)
            {
                NoReviewsMessage = "No reviews in sight.";
            }

            ActivityCommand = new RelayCommand(() => _chartsService.Navigate());
            ManageFlashcardsCommand = new RelayCommand(() => _flashcardsService.Navigate());
        }

        public override void OnGoBackClick()
        {
            _navService.Navigate();
        }
    }
}
