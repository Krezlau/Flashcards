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
    public class ActivityChartsViewModel : ActivityChartsBaseViewModel
    {
        private readonly NavigationService<AccountManagementViewModel> _navService;

        public ActivityChartsViewModel(UserDecksStore userDecksStore, NavigationService<AccountManagementViewModel> navService) : base(userDecksStore)
        {
            _navService = navService;
            
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

            Title = "Your activity";

            OnLastMonthClick();
        }

        public override void OnGoBackClick()
        {
            _navService.Navigate();
        }
    }
}
