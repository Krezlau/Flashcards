using Flashcards.Core.Services;
using Flashcards.Core.Stores;
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

        public FutureReviewCalendarViewModel(UserDecksStore userDecksStore, NavigationService<AccountManagementViewModel> navService) : base(userDecksStore)
        {
            _navService = navService;
        }

        public override void OnGoBackClick()
        {
            _navService.Navigate();
        }
    }
}
