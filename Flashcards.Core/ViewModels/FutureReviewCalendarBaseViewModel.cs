using Flashcards.Core.Services;
using Flashcards.Core.Stores;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Flashcards.Core.ViewModels
{
    public abstract class FutureReviewCalendarBaseViewModel
    {
        protected readonly UserDecksStore _userDecksStore;
        protected readonly ActivityDataOrganizer _dataOrganizer;

        protected FutureReviewCalendarBaseViewModel(UserDecksStore userDecksStore)
        {
            _userDecksStore = userDecksStore;
            _dataOrganizer = new ActivityDataOrganizer();
        }

        public string Title { get; set; }

        public string NoReviewsMessage { get; set; }

        public ICommand GoBackCommand { get; set; }

        public ISeries[] FutureReviewsSeries { get; set; }

        public Axis[] FutureReviewsXAxes { get; set; }

        public Axis[] FutureReviewsYAxes { get; set; } =
        {
            new Axis
            {
                Labels = {"Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"}
            }
        };

        public abstract void OnGoBackClick();
    }
}
