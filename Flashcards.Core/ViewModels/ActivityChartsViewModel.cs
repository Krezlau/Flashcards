using Flashcards.Core.Models;
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
    public class ActivityChartsViewModel : ObservableObject
    {
        private readonly UserDecksStore _userDecksStore;
        private readonly NavigationService<AccountManagementViewModel> _navService;
        private readonly ActivityDataOrganizer _dataOrganizer;

        public ActivityChartsViewModel(UserDecksStore userDecksStore, NavigationService<AccountManagementViewModel> navService)
        {
            _userDecksStore = userDecksStore;
            _navService = navService;

            GoBackCommand = new RelayCommand(OnGoBackClick);

            _dataOrganizer = new ActivityDataOrganizer();
            bool ifActivityEver = _dataOrganizer.OrganizeActivityData(_userDecksStore.User);

            if (!ifActivityEver)
            {
                NoActivityMessage = "No activity data on this account.";
                return;
            }

            ReviewCountSeries.Add(new LineSeries<DateTimePoint>
            {
                TooltipLabelFormatter = (chartPoint) =>
                $"{new DateTime((long)chartPoint.SecondaryValue):MMMM dd}: {chartPoint.PrimaryValue} reviewed",
                Values = _dataOrganizer.DailyReviewedCount,
                LineSmoothness = 0,
                GeometrySize = 10
            });

            MinutesSpentSeries.Add(new LineSeries<DateTimePoint>
            {
                TooltipLabelFormatter = (chartPoint) =>
                $"{new DateTime((long)chartPoint.SecondaryValue):MMMM dd}: {ToWholeMinutesAndSeconds(chartPoint.PrimaryValue)} spent",
                Values = _dataOrganizer.DailyMinutesSpent,
                LineSmoothness = 0,
                GeometrySize = 10
            });

            TotalReviewCount = $"Total review count: {_dataOrganizer.TotalReviewedCount}";
        }

        public ICommand GoBackCommand { get; set; }

        public string TotalReviewCount { get; set; }

        public string NoActivityMessage { get; set; }

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

        public void OnGoBackClick()
        {
            _navService.Navigate();
        }

        private string ToWholeMinutesAndSeconds(double minutes)
        {
            int wholeMinutes = (int)minutes;
            int seconds = (int)((minutes - wholeMinutes) * 60);
            return $"{wholeMinutes} min {seconds} seconds";
        }
    }
}
