﻿using Flashcards.Core.Models;
using LiveChartsCore;
using LiveChartsCore.Defaults;
using LiveChartsCore.SkiaSharpView;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards.Core.Services
{
    public class FutureReviewsOrganizer
    {
        public FutureReviewsOrganizer(Axis[] futureReviewsXAxes, ObservableCollection<ISeries> futureReviewsSeries)
        {
            FutureReviewsXAxes = futureReviewsXAxes;
            FutureReviewsSeries = futureReviewsSeries;
        }

        public Axis[] FutureReviewsXAxes { get; set; }

        public ObservableCollection<ISeries> FutureReviewsSeries { get; set; }

        public ObservableCollection<WeightedPoint> FutureReviewsObservable { get; private set; }

        public bool OrganizeFutureReviews(User user)
        {
            var reviewsCountPerDate = new List<DateTimePoint>();
            var coveredDates = new List<DateTime>();

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
            int weekNumber = 0;
            int dayOfTheWeek = (int)values[0].DateTime.DayOfWeek;
            FutureReviewsXAxes[0].Labels.Add(values[0].DateTime.AddDays(-dayOfTheWeek).ToString("d"));
            FutureReviewsObservable.Add(new WeightedPoint(weekNumber, dayOfTheWeek++, values[0].Value));
            if (dayOfTheWeek > 6)
            {
                dayOfTheWeek = 0;
                FutureReviewsXAxes[0].Labels.Add(values[0].DateTime.AddDays(1).ToString("d"));
                weekNumber++;
            }

            for (int i = 1; i < values.Count - 1; i++)
            {
                dayOfTheWeek = (int)values[i].DateTime.DayOfWeek;
                FutureReviewsObservable.Add(new WeightedPoint(weekNumber, dayOfTheWeek++, values[i].Value));
                if (dayOfTheWeek > 6)
                {
                    dayOfTheWeek = 0;
                    FutureReviewsXAxes[0].Labels.Add(values[i].DateTime.AddDays(1).ToString("d"));
                    weekNumber++;
                }
                DateTime currentDate = values[i].DateTime;

                while (currentDate.AddDays(1) != values[i + 1].DateTime)
                {
                    FutureReviewsObservable.Add(new WeightedPoint(weekNumber, dayOfTheWeek++, 0));
                    if (dayOfTheWeek > 6)
                    {
                        dayOfTheWeek = 0;
                        FutureReviewsXAxes[0].Labels.Add(currentDate.AddDays(1).ToString("d"));
                        weekNumber++;
                    }
                    currentDate = currentDate.AddDays(1);
                }
            }
            FutureReviewsObservable.Add(new WeightedPoint(weekNumber, dayOfTheWeek++, values[^1].Value));
            if (dayOfTheWeek > 6)
            {
                dayOfTheWeek = 0;
                FutureReviewsXAxes[0].Labels.Add(values[^1].DateTime.AddDays(1).ToString("d"));
                weekNumber++;
            }

            // finish last week
            while (dayOfTheWeek != 6)
            {
                FutureReviewsObservable.Add(new WeightedPoint(weekNumber, dayOfTheWeek++, 0));
            }
        }

        private void SetUpTheSeries()
        {
            FutureReviewsSeries.Add(new HeatSeries<WeightedPoint>
            {
                HeatMap = new[]
                {
                    new SKColor(255, 241, 118).AsLvcColor(), // the first element is the "coldest"
                    SKColors.DarkSlateGray.AsLvcColor(),
                    SKColors.Blue.AsLvcColor()
                },
                Values = FutureReviewsObservable
            });
        }
    }
}
