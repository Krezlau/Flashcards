using Flashcards.Core.Services;
using Flashcards.Core.Stores;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Flashcards.Core.ViewModels
{
    public abstract class FutureReviewCalendarBaseViewModel : ObservableObject
    {
        protected readonly UserDecksStore _userDecksStore;
        protected readonly FutureReviewsOrganizer _dataOrganizer;

        protected FutureReviewCalendarBaseViewModel(UserDecksStore userDecksStore)
        {
            _userDecksStore = userDecksStore;
            _dataOrganizer = new FutureReviewsOrganizer(FutureReviewsXAxes, FutureReviewsSeries);
            GoBackCommand = new RelayCommand(OnGoBackClick);
        }

        public string Title { get; set; }

        public string NoReviewsMessage { get; set; }

        public ICommand GoBackCommand { get; set; }

        public ICommand ActivityCommand { get; set; }

        public ICommand ManageFlashcardsCommand { get; set; }

        public ObservableCollection<ISeries> FutureReviewsSeries { get; set; } = new ObservableCollection<ISeries>();

        public Axis[] FutureReviewsXAxes { get; set; } =
        {
            new Axis
            {
                Labels = new List<string>()
            }
        };

        public Axis[] FutureReviewsYAxes { get; set; } =
        {
            new Axis
            {
                Labels = new[] {"Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"},
                ShowSeparatorLines = false
            }
        };

        public abstract void OnGoBackClick();
    }
}
