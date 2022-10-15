using Flashcards.Core.Services;
using Flashcards.Core.Stores;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards.Core.ViewModels
{
    public class DeckActivityChartsViewModel : ActivityChartsBaseViewModel
    {
        private readonly NavigationService<DeckPreviewViewModel> _navService;

        public DeckActivityChartsViewModel(UserDecksStore userDecksStore, NavigationService<DeckPreviewViewModel> navService) : base(userDecksStore)
        {
            _navService = navService;

            bool ifActivityEver = _dataOrganizer.OrganizeActivityData(_userDecksStore.SelectionStore.SelectedDeck);

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

            Title = $"Your activity in \"{_userDecksStore.SelectionStore.SelectedDeck.Name}\"";
            BottomText = $"Total review count: {_dataOrganizer.TotalReviewedCount}, " +
                $"Total time spent: {ToWholeMinutesAndSeconds(_dataOrganizer.TotalMinutesSpent)}, " +
                $"Avg per flashcard: {_dataOrganizer.AverageTimePerFlashcard:N2} min";
        }

        public override void OnGoBackClick()
        {
            _navService.Navigate();
        }
    }
}
