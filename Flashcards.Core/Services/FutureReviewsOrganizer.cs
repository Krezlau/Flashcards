using Flashcards.Core.Models;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using LiveChartsCore;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards.Core.Services
{
    public class FutureReviewsOrganizer
    {
        public FutureReviewsOrganizer(Axis[] futureReviewsXAxes,
                                      ObservableCollection<ISeries> futureReviewsSeries)
        {
            FutureReviewsXAxes = futureReviewsXAxes;
            FutureReviewsSeries = futureReviewsSeries;
        }

        public Axis[] FutureReviewsXAxes { get; private set; }

        public ObservableCollection<ISeries> FutureReviewsSeries { get; private set; }

        public ObservableCollection<WeightedPoint> FutureReviewsObservable { get; private set; }

        public bool OrganizeFutureReviews(User user)
        {
            if (user is null) throw new ArgumentNullException(nameof(user));

            var reviewsCountPerDate = new List<DateTimePoint>();
            var coveredDates = new List<DateTime>();

            if (user.Decks == null) return false;

            foreach (Deck deck in user.Decks)
            {
                TakeDataFromDeck(deck, reviewsCountPerDate, coveredDates);
            }
            if (coveredDates.Count == 0) return false;

            FillTheGapsAndSetUpTheAxis(reviewsCountPerDate);

            SetUpTheSeries();

            return true;
        }

        public bool OrganizeFutureReviews(Deck deck)
        {
            if (deck is null) throw new ArgumentNullException(nameof(deck));

            var reviewsCountPerDate = new List<DateTimePoint>();
            var coveredDates = new List<DateTime>();

            TakeDataFromDeck(deck, reviewsCountPerDate, coveredDates);

            if (coveredDates.Count == 0) return false;

            FillTheGapsAndSetUpTheAxis(reviewsCountPerDate);

            SetUpTheSeries();

            return true;
        }

        private void TakeDataFromDeck(Deck deck, List<DateTimePoint> list, List<DateTime> coveredDates)
        {
            if (deck.Flashcards is null) return;
            foreach (Flashcard flashcard in deck.Flashcards)
            {
                int index = coveredDates.BinarySearch(flashcard.NextReview);
                if (index < 0)
                {
                    coveredDates.Insert(~index, flashcard.NextReview);
                    list.Insert(~index, new DateTimePoint(flashcard.NextReview, 1));
                }
                if (index >= 0)
                {
                    list[index].Value += 1;
                }
            }
        }

        private void FillTheGapsAndSetUpTheAxis(List<DateTimePoint> values)
        {
            FutureReviewsObservable = new ObservableCollection<WeightedPoint>();

            int index = 0;
            double sum = 0;
            while (index < values.Count && values[index].DateTime < DateTime.Today)
            {
                sum += (double)values[index++].Value;
            }

            DateTime currentDate = DateTime.Today;
            int weekNumber = 0;
            int dayOfTheWeek = (int)DateTime.Today.DayOfWeek;
            FutureReviewsXAxes[0].Labels.Add(DateTime.Today.AddDays(-dayOfTheWeek).ToString("d"));

            AddDayToFutureReviews(ref dayOfTheWeek, ref weekNumber, currentDate, (int)sum);

            for (int i = index; i < values.Count; i++)
            {
                dayOfTheWeek = (int)values[i].DateTime.DayOfWeek;
                AddDayToFutureReviews(ref dayOfTheWeek, ref weekNumber, values[i].DateTime, (int)values[i].Value);
                currentDate = values[i].DateTime;

                while (i < values.Count - 1 && currentDate.AddDays(1) != values[i + 1].DateTime)
                {
                    AddDayToFutureReviews(ref dayOfTheWeek, ref weekNumber, currentDate, 0);
                    currentDate = currentDate.AddDays(1);
                }
            }

            // finish last week
            while (dayOfTheWeek <= 6)
            {
                FutureReviewsObservable.Add(new WeightedPoint(weekNumber, dayOfTheWeek++, 0));
                currentDate = currentDate.AddDays(1);
            }
            weekNumber++;
            while (weekNumber <= 25)
            {
                FutureReviewsXAxes[0].Labels.Add(currentDate.AddDays(1).ToString("d"));
                dayOfTheWeek = 0;
                FutureReviewsObservable.Add(new WeightedPoint(weekNumber, dayOfTheWeek++, 0));
                FutureReviewsObservable.Add(new WeightedPoint(weekNumber, dayOfTheWeek++, 0));
                FutureReviewsObservable.Add(new WeightedPoint(weekNumber, dayOfTheWeek++, 0));
                FutureReviewsObservable.Add(new WeightedPoint(weekNumber, dayOfTheWeek++, 0));
                FutureReviewsObservable.Add(new WeightedPoint(weekNumber, dayOfTheWeek++, 0));
                FutureReviewsObservable.Add(new WeightedPoint(weekNumber, dayOfTheWeek++, 0));
                FutureReviewsObservable.Add(new WeightedPoint(weekNumber++, dayOfTheWeek++, 0));
                currentDate = currentDate.AddDays(6);
            }
        }

        private void SetUpTheSeries()
        {
            FutureReviewsSeries.Add(new HeatSeries<WeightedPoint>
            {
                PointPadding = new LiveChartsCore.Drawing.Padding(1),
                HeatMap = new[]
                {
                    SKColors.Black.AsLvcColor(),
                    SKColors.SpringGreen.AsLvcColor()
                },
                Values = FutureReviewsObservable,
                TooltipLabelFormatter = (chartPoint) =>
                $"{DateTime.Parse(FutureReviewsXAxes[0].Labels[(int)chartPoint.Coordinate.SecondaryValue]).AddDays((int)chartPoint.PrimaryValue):d}" +
                $" {(int)chartPoint.TertiaryValue}"
            });
        }

        private void AddDayToFutureReviews(ref int dayOfTheWeek, ref int weekNumber, DateTime currentDate, int value)
        {
            FutureReviewsObservable.Add(new WeightedPoint(weekNumber, dayOfTheWeek++, value));
            if (dayOfTheWeek > 6)
            {
                dayOfTheWeek = 0;
                FutureReviewsXAxes[0].Labels.Add(currentDate.AddDays(1).ToString("d"));
                weekNumber++;
            }
        }
    }
}
