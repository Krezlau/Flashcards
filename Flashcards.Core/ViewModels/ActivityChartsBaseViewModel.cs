using Flashcards.Core.Services;
using Flashcards.Core.Stores;
using LiveChartsCore;
using LiveChartsCore.Defaults;
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
            AllTimeCommand = new RelayCommand(OnAllTimeClick);
            LastYearCommand = new RelayCommand(OnLastYearClick);
            LastMonthCommand = new RelayCommand(OnLastMonthClick);

            BottomLabel = "All time";
            BottomText = $"Total review count: 0, " +
                $"Total time spent: 0 min 0 seconds, " +
                $"Avg per flashcard: 0,00 min";
        }

        public ICommand GoBackCommand { get; set; }

        public ICommand AllTimeCommand { get; set; }

        public ICommand LastYearCommand { get; set; }
        
        public ICommand LastMonthCommand { get; set; }

        private string _bottomText;
        public string BottomText
        {
            get => _bottomText;
            set => SetProperty(ref _bottomText, value);
        }


        private string _bottomLabel;
        public string BottomLabel
        {
            get => _bottomLabel;
            set => SetProperty(ref _bottomLabel, value);
        }

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

        public void OnAllTimeClick()
        {
            if (NoActivityMessage is not null) return;

            ReviewCountSeries[0].Values = _dataOrganizer.DailyReviewedCount;
            MinutesSpentSeries[0].Values = _dataOrganizer.DailyMinutesSpent;

            BottomLabel = "All time";
            BottomText = $"Total review count: {_dataOrganizer.TotalReviewedCount}, " +
                $"Total time spent: {ToWholeMinutesAndSeconds(_dataOrganizer.TotalMinutesSpent)}, " +
                $"Avg per flashcard: {_dataOrganizer.AverageTimePerFlashcard:N2} min";
        }

        public void OnLastYearClick()
        {
            if (NoActivityMessage is not null) return;

            var minutes = new ObservableCollection<DateTimePoint>(_dataOrganizer.DailyMinutesSpent
                                .Skip(Math.Max(0, _dataOrganizer.DailyMinutesSpent.Count - 365)));
            var count = new ObservableCollection<DateTimePoint>(_dataOrganizer.DailyReviewedCount
                                .Skip(Math.Max(0, _dataOrganizer.DailyReviewedCount.Count - 365)));

            ReviewCountSeries[0].Values = count;
            MinutesSpentSeries[0].Values = minutes;

            int totalCount = (int)count.Sum(c => c.Value);
            double totalMinutes = (double)minutes.Sum(m => m.Value);

            BottomLabel = "Last 365 days";
            BottomText = $"Total review count: {totalCount}, " +
                $"Total time spent: {ToWholeMinutesAndSeconds(totalMinutes)}, " +
                $"Avg per flashcard: {totalMinutes / totalCount:N2} min";
        }

        public void OnLastMonthClick()
        {
            if (NoActivityMessage is not null) return;

            var minutes = new ObservableCollection<DateTimePoint>(_dataOrganizer.DailyMinutesSpent
                                .Skip(Math.Max(0, _dataOrganizer.DailyMinutesSpent.Count - 30)));
            var count = new ObservableCollection<DateTimePoint>(_dataOrganizer.DailyReviewedCount
                                .Skip(Math.Max(0, _dataOrganizer.DailyReviewedCount.Count - 30)));

            ReviewCountSeries[0].Values = count;
            MinutesSpentSeries[0].Values = minutes;

            int totalCount = (int)count.Sum(c => c.Value);
            double totalMinutes = (double)minutes.Sum(m => m.Value);

            BottomLabel = "Last 30 days";
            BottomText = $"Total review count: {totalCount}, " +
                $"Total time spent: {ToWholeMinutesAndSeconds(totalMinutes)}, " +
                $"Avg per flashcard: {totalMinutes / totalCount:N2} min";
        }

        protected string ToWholeMinutesAndSeconds(double minutes)
        {
            int wholeMinutes = (int)minutes;
            int seconds = (int)((minutes - wholeMinutes) * 60);
            return $"{wholeMinutes} min {seconds} seconds";
        }
    }
}
