using Flashcards.Core.Services;
using Flashcards.Core.Stores;
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

        public DeckFutureReviewsCalendarViewModel(UserDecksStore userDecksStore, NavigationService<DeckPreviewViewModel> navService) : base(userDecksStore)
        {
            _navService = navService;

            bool outcome = _dataOrganizer.OrganizeFutureReviews(_userDecksStore.SelectionStore.SelectedDeck);
            if (!outcome)
            {
                NoReviewsMessage = "No reviews in sight.";
            }

            Title = $"Reviews for \"{_userDecksStore.SelectionStore.SelectedDeck}\"";
        }

        public override void OnGoBackClick()
        {
            _navService.Navigate();
        }
    }
}
