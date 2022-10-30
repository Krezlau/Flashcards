using Flashcards.Core.Services;
using Flashcards.Core.Stores;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards.Core.ViewModels
{
    public class FutureReviewCalendarViewModel : FutureReviewCalendarBaseViewModel
    {
        private readonly NavigationService<AccountManagementViewModel> _navService;
        private readonly NavigationService<ActivityChartsViewModel> _activityNav;

        public FutureReviewCalendarViewModel(UserDecksStore userDecksStore,
                                             NavigationService<AccountManagementViewModel> navService,
                                             NavigationService<ActivityChartsViewModel> activityNav) : base(userDecksStore)
        {
            _navService = navService;
            _activityNav = activityNav;

            bool outcome = _dataOrganizer.OrganizeFutureReviews(_userDecksStore.User);
            if (!outcome)
            {
                NoReviewsMessage = "No reviews in sight.";
            }

            Title = $"Future reviews for \"{_userDecksStore.User.Name}\"";

            ActivityCommand = new RelayCommand(OnActivityClick);
        }

        private void OnActivityClick()
        {
            _activityNav.Navigate();
        }

        public override void OnGoBackClick()
        {
            _navService.Navigate();
        }
    }
}
