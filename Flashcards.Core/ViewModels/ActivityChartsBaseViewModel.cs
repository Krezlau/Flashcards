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
    public abstract class ActivityChartsBaseViewModel : ObservableObject
    {
        protected readonly UserDecksStore _userDecksStore;
        protected readonly ActivityDataOrganizer _dataOrganizer;

        protected ActivityChartsBaseViewModel(UserDecksStore userDecksStore)
        {
            _dataOrganizer = new ActivityDataOrganizer();
            _userDecksStore = userDecksStore;
            GoBackCommand = new RelayCommand(OnGoBackClick);
        }

        public ICommand GoBackCommand { get; set; }

        public string BottomText { get; set; }

        public string NoActivityMessage { get; set; }

        public string Title { get; set; }

        public ObservableCollection<ISeries> ReviewCountSeries { get; set; } = new ObservableCollection<ISeries>();

        public ObservableCollection<ISeries> MinutesSpentSeries { get; set; } = new ObservableCollection<ISeries>();

        public Axis[] ReviewCountXAxes { get; set; } =
        {
            new Axis
            {
                Labeler = value => new DateTime((long) value).ToString("MMMM dd"),
                LabelsRotation = 45,
                UnitWidth = TimeSpan.FromDays(1).Ticks,
                MinStep = TimeSpan.FromDays(1).Ticks
            }
        };

        public Axis[] MinutesSpentXAxes { get; set; } =
        {
            new Axis
            {
                Labeler = value => new DateTime((long) value).ToString("MMMM dd"),
                LabelsRotation = 45,
                UnitWidth = TimeSpan.FromDays(1).Ticks,
                MinStep = TimeSpan.FromDays(1).Ticks
            }
        };

        public Axis[] ReviewCountYAxes { get; set; } =
        {
            new Axis
            {
                MinStep = 1,
                MinLimit = 0
            }
        };

        public Axis[] MinutesSpentYAxes { get; set; } =
        {
            new Axis
            {
                MinStep = 0.1,
                MinLimit = 0
            }
        };

        public abstract void OnGoBackClick();

        protected string ToWholeMinutesAndSeconds(double minutes)
        {
            int wholeMinutes = (int)minutes;
            int seconds = (int)((minutes - wholeMinutes) * 60);
            return $"{wholeMinutes} min {seconds} seconds";
        }
    }
}
