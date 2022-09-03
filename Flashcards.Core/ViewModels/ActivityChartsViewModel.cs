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

            var values = new ObservableCollection<DateTimePoint>();
            int sum = 0;

            if (_userDecksStore.User.Activity.Count != 0)
            {
                values.Add(new DateTimePoint(_userDecksStore.User.Activity[0].Day, _userDecksStore.User.Activity[0].ReviewedFlashcardsCount));
                sum += _userDecksStore.User.Activity[0].ReviewedFlashcardsCount;

                for (int i = 1; i < _userDecksStore.User.Activity.Count; i++)
                {
                    while (values[^1].DateTime.AddDays(1) != _userDecksStore.User.Activity[i].Day)
                    {
                        values.Add(new DateTimePoint(values[^1].DateTime.AddDays(1), 0));
                    }

                    values.Add(new DateTimePoint(_userDecksStore.User.Activity[i].Day, _userDecksStore.User.Activity[i].ReviewedFlashcardsCount));
                    sum += _userDecksStore.User.Activity[i].ReviewedFlashcardsCount;
                }
                while (values[^1].DateTime.AddDays(1) <= DateTime.Today)
                {
                    values.Add(new DateTimePoint(values[^1].DateTime.AddDays(1), 0));
                }
            }
            if (_userDecksStore.User.Activity.Count == 0) NoActivityMessage = "No activity data on this account.";

            Series.Add(new LineSeries<DateTimePoint>
            {
                TooltipLabelFormatter = (chartPoint) =>
                $"{new DateTime((long)chartPoint.SecondaryValue):MMMM dd}: {chartPoint.PrimaryValue} reviewed",
                Values = values,
                LineSmoothness = 0,
                GeometrySize = 10
            });

            TotalReviewCount = $"Total review count: {sum}";
        }

        public ICommand GoBackCommand { get; set; }

        public string TotalReviewCount { get; set; }

        public string NoActivityMessage { get; set; }

        public ObservableCollection<ISeries> Series { get; set; } = new ObservableCollection<ISeries>();

        public Axis[] XAxes { get; set; } =
        {
            new Axis
            {
                Labeler = value => new DateTime((long) value).ToString("MMMM dd"),
                LabelsRotation = 45,
                UnitWidth = TimeSpan.FromDays(1).Ticks, 
                MinStep = TimeSpan.FromDays(1).Ticks
            }
        };

        public Axis[] YAxes { get; set; } =
        {
            new Axis
            {
                MinStep = 1,
                MinLimit = 0
            }
        };

        public void OnGoBackClick()
        {
            _navService.Navigate();
        }
    }
}
