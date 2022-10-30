using Flashcards.Core.Services;
using Flashcards.Core.Stores;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
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
                $"Avg per flashcard: 0.00 min";
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
                Labeler = value => new DateTime((long) value).ToString("MMM dd"),
                LabelsRotation = 45,
                UnitWidth = TimeSpan.FromDays(1).Ticks,
                MinStep = TimeSpan.FromDays(1).Ticks
            }
        };

        public Axis[] MinutesSpentXAxes { get; set; } =
        {
            new Axis
            {
                Labeler = value => new DateTime((long) value).ToString("MMM dd"),
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
            if (NoActivityMessage is not null || BottomLabel == "All time") return;

            var minutes = GroupIntoMonths(_dataOrganizer.DailyMinutesSpent);
            var count = GroupIntoMonths(_dataOrganizer.DailyReviewedCount);

            SetTooltipLabelIntoMonths(new ObservableCollection<DateTimePoint>(count), new ObservableCollection<DateTimePoint>(minutes));

            BottomLabel = "All time";
            BottomText = $"Total review count: {_dataOrganizer.TotalReviewedCount}, " +
                $"Total time spent: {ToWholeMinutesAndSeconds(_dataOrganizer.TotalMinutesSpent)}, " +
                $"Avg per flashcard: {_dataOrganizer.AverageTimePerFlashcard:N2} min";
        }

        public void OnLastYearClick()
        {
            if (NoActivityMessage is not null || BottomLabel == "Last 365 days") return;

            var minutes = GroupIntoMonths(_dataOrganizer.DailyMinutesSpent);
            var count = GroupIntoMonths(_dataOrganizer.DailyReviewedCount);

            var countObservable = new ObservableCollection<DateTimePoint>(count.Skip(Math.Max(0, count.Count - 12)));
            var minutesObservable = new ObservableCollection<DateTimePoint>(minutes.Skip(Math.Max(0, minutes.Count - 12)));

            SetTooltipLabelIntoMonths(countObservable, minutesObservable);

            int totalCount = (int)count.Sum(c => c.Value);
            double totalMinutes = (double)minutes.Sum(m => m.Value);

            BottomLabel = "Last 365 days";
            BottomText = $"Total review count: {totalCount}, " +
                $"Total time spent: {ToWholeMinutesAndSeconds(totalMinutes)}, " +
                $"Avg per flashcard: {totalMinutes / totalCount:N2} min";
        }

        public void OnLastMonthClick()
        {
            if (NoActivityMessage is not null || BottomLabel == "Last 30 days") return;

            var minutes = new ObservableCollection<DateTimePoint>(_dataOrganizer.DailyMinutesSpent
                                .Skip(Math.Max(0, _dataOrganizer.DailyMinutesSpent.Count - 30)));
            var count = new ObservableCollection<DateTimePoint>(_dataOrganizer.DailyReviewedCount
                                .Skip(Math.Max(0, _dataOrganizer.DailyReviewedCount.Count - 30)));

            ReviewCountXAxes[0].Labeler = value => new DateTime((long)value).ToString("MMM dd");
            ReviewCountXAxes[0].MinStep = TimeSpan.FromDays(1).Ticks;
            ReviewCountXAxes[0].MinStep = TimeSpan.FromDays(1).Ticks;

            MinutesSpentXAxes[0].Labeler = value => new DateTime((long)value).ToString("MMM dd");
            MinutesSpentXAxes[0].MinStep = TimeSpan.FromDays(1).Ticks;
            MinutesSpentXAxes[0].MinStep = TimeSpan.FromDays(1).Ticks;

            ReviewCountSeries.Clear();
            ReviewCountSeries.Add(new LineSeries<DateTimePoint>
            {
                TooltipLabelFormatter = (chartPoint) =>
                $"{new DateTime((long)chartPoint.SecondaryValue):MMMM dd}: {chartPoint.PrimaryValue} reviewed",
                Values = count,
                LineSmoothness = 0,
                GeometrySize = 10,
                Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 6 },
                Fill = new LinearGradientPaint(SKColors.Blue, SKColors.Empty, new SKPoint(0.5f, 0), new SKPoint(0.5f, 1)),
                GeometryFill = new SolidColorPaint(SKColors.Blue),
                GeometryStroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 4 }
            });

            MinutesSpentSeries.Clear();
            MinutesSpentSeries.Add(new LineSeries<DateTimePoint>
            {
                TooltipLabelFormatter = (chartPoint) =>
                $"{new DateTime((long)chartPoint.SecondaryValue):MMMM dd}: {ToWholeMinutesAndSeconds(chartPoint.PrimaryValue)} spent",
                Values = minutes,
                LineSmoothness = 0,
                GeometrySize = 10,
                Stroke = new SolidColorPaint(SKColors.Purple) { StrokeThickness = 6 },
                Fill = new LinearGradientPaint(SKColors.Purple, SKColors.Empty, new SKPoint(0.5f, 0), new SKPoint(0.5f, 1)),
                GeometryFill = new SolidColorPaint(SKColors.Purple),
                GeometryStroke = new SolidColorPaint(SKColors.Purple) { StrokeThickness = 4 }
            });

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

        private List<DateTimePoint> GroupIntoMonths(ObservableCollection<DateTimePoint> source)
        {
            return new List<DateTimePoint>(
                source
                .GroupBy(m => $"{m.DateTime.Year}-{m.DateTime.Month}")
                .Select(s => new DateTimePoint(s.First().DateTime, s.Sum(m => m.Value)))
                .ToList());
        }

        private void SetTooltipLabelIntoMonths(ObservableCollection<DateTimePoint> countSeries, ObservableCollection<DateTimePoint> minutesSeries)
        {
            ReviewCountSeries.Clear();
            ReviewCountSeries.Add(new LineSeries<DateTimePoint>
            {
                TooltipLabelFormatter = (chartPoint) =>
                $"{new DateTime((long)chartPoint.SecondaryValue):yyyy MMMM}: {chartPoint.PrimaryValue} reviewed",
                Values = countSeries,
                LineSmoothness = 0,
                GeometrySize = 10,
                Stroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 6},
                Fill = new LinearGradientPaint(SKColors.Blue, SKColors.Empty, new SKPoint(0.5f, 0), new SKPoint(0.5f, 1)),
                GeometryFill = new SolidColorPaint(SKColors.Blue),
                GeometryStroke = new SolidColorPaint(SKColors.Blue) { StrokeThickness = 4 }
            });

            ReviewCountXAxes[0].Labeler = value => new DateTime((long)value).ToString("yyyy MMM");
            ReviewCountXAxes[0].MinStep = TimeSpan.FromDays(30).Ticks;
            ReviewCountXAxes[0].MinStep = TimeSpan.FromDays(30).Ticks;

            MinutesSpentXAxes[0].Labeler = value => new DateTime((long)value).ToString("yyyy MMM");
            MinutesSpentXAxes[0].MinStep = TimeSpan.FromDays(30).Ticks;
            MinutesSpentXAxes[0].MinStep = TimeSpan.FromDays(30).Ticks;

            MinutesSpentSeries.Clear();
            MinutesSpentSeries.Add(new LineSeries<DateTimePoint>
            {
                TooltipLabelFormatter = (chartPoint) =>
                $"{new DateTime((long)chartPoint.SecondaryValue):yyyy MMMM}: {ToWholeMinutesAndSeconds(chartPoint.PrimaryValue)} spent",
                Values = minutesSeries,
                LineSmoothness = 0,
                GeometrySize = 10,
                Stroke = new SolidColorPaint(SKColors.Purple) { StrokeThickness = 6 },
                Fill = new LinearGradientPaint(SKColors.Purple, SKColors.Empty, new SKPoint(0.5f, 0), new SKPoint(0.5f, 1)),
                GeometryFill = new SolidColorPaint(SKColors.Purple),
                GeometryStroke = new SolidColorPaint(SKColors.Purple) { StrokeThickness = 4 }
            });
        }
    }
}
