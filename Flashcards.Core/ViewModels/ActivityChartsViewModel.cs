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

        public ActivityChartsViewModel(UserDecksStore userDecksStore, NavigationService<AccountManagementViewModel> navService)
        {
            _userDecksStore = userDecksStore;
            _navService = navService;

            GoBackCommand = new RelayCommand(OnGoBackClick);

            var reviewedCount_InitialList = new List<DateTimePoint>();
            var minutesSpent_InitialList = new List<DateTimePoint>();
            var daysRegisteredList = new List<DateTime>();
            int reviewCountSum = 0;

            // getting data from user activity (deleted decks' activity)
            if (_userDecksStore.User.Activity.Count != 0)
            {
                reviewedCount_InitialList.Add(new DateTimePoint(_userDecksStore.User.Activity[0].Day, _userDecksStore.User.Activity[0].ReviewedFlashcardsCount));
                minutesSpent_InitialList.Add(new DateTimePoint(_userDecksStore.User.Activity[0].Day, _userDecksStore.User.Activity[0].MinutesSpentLearning));
                daysRegisteredList.Add(_userDecksStore.User.Activity[0].Day);
                reviewCountSum += _userDecksStore.User.Activity[0].ReviewedFlashcardsCount;

                for (int i = 1; i < _userDecksStore.User.Activity.Count; i++)
                {
                    reviewedCount_InitialList.Add(new DateTimePoint(_userDecksStore.User.Activity[i].Day, _userDecksStore.User.Activity[i].ReviewedFlashcardsCount));
                    minutesSpent_InitialList.Add(new DateTimePoint(_userDecksStore.User.Activity[i].Day, _userDecksStore.User.Activity[i].MinutesSpentLearning));
                    daysRegisteredList.Add(_userDecksStore.User.Activity[i].Day);
                    reviewCountSum += _userDecksStore.User.Activity[i].ReviewedFlashcardsCount;
                }
            }

            // getting data from each deck's activity
            foreach (Deck deck in _userDecksStore.User.Decks)
            {
                if (deck.Activity.Count == 0) continue;
                foreach (DeckActivity activity in deck.Activity)
                {
                    int index = daysRegisteredList.BinarySearch(activity.Day);
                    if (index < 0)
                    {
                        daysRegisteredList.Insert(~index, activity.Day);
                        reviewedCount_InitialList.Insert(~index, new DateTimePoint(activity.Day, activity.ReviewedFlashcardsCount));
                        minutesSpent_InitialList.Insert(~index, new DateTimePoint(activity.Day, activity.MinutesSpentLearning));
                        reviewCountSum += activity.ReviewedFlashcardsCount;
                    }
                    if (index >= 0)
                    {
                        reviewedCount_InitialList[index].Value += activity.ReviewedFlashcardsCount;
                        minutesSpent_InitialList[index].Value += activity.MinutesSpentLearning;
                        reviewCountSum += activity.ReviewedFlashcardsCount;
                    }
                }
            }

            if (daysRegisteredList.Count == 0)
            {
                NoActivityMessage = "No activity data on this account.";
                return;
            }

            //filling the holes
            var reviewCount = new ObservableCollection<DateTimePoint>();
            var minutesSpent = new ObservableCollection<DateTimePoint>();

            for (int i = 0; i < daysRegisteredList.Count - 1; i++)
            {
                reviewCount.Add(reviewedCount_InitialList[i]);
                minutesSpent.Add(minutesSpent_InitialList[i]);

                while (reviewCount[^1].DateTime.AddDays(1) != daysRegisteredList[i + 1])
                {
                    reviewCount.Add(new DateTimePoint(reviewCount[^1].DateTime.AddDays(1), 0));
                    minutesSpent.Add(new DateTimePoint(reviewCount[^1].DateTime.AddDays(1), 0));
                }
            }
            reviewCount.Add(reviewedCount_InitialList[^1]);
            minutesSpent.Add(minutesSpent_InitialList[^1]);

            while (reviewCount[^1].DateTime.AddDays(1) <= DateTime.Today)
            {
                reviewCount.Add(new DateTimePoint(reviewCount[^1].DateTime.AddDays(1), 0));
                minutesSpent.Add(new DateTimePoint(reviewCount[^1].DateTime.AddDays(1), 0));
            }



            ReviewCountSeries.Add(new LineSeries<DateTimePoint>
            {
                TooltipLabelFormatter = (chartPoint) =>
                $"{new DateTime((long)chartPoint.SecondaryValue):MMMM dd}: {chartPoint.PrimaryValue} reviewed",
                Values = reviewCount,
                LineSmoothness = 0,
                GeometrySize = 10
            });

            MinutesSpentSeries.Add(new LineSeries<DateTimePoint>
            {
                TooltipLabelFormatter = (chartPoint) =>
                $"{new DateTime((long)chartPoint.SecondaryValue):MMMM dd}: {ToWholeMinutesAndSeconds(chartPoint.PrimaryValue)} spent",
                Values = minutesSpent,
                LineSmoothness = 0,
                GeometrySize = 10
            });

            TotalReviewCount = $"Total review count: {reviewCountSum}";
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
