using Flashcards.Core.Models;
using LiveChartsCore.Defaults;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards.Core.Services
{
    /// <summary>
    /// Helper class to organize data for charts
    /// </summary>
    public class ActivityDataOrganizer
    {
        public ObservableCollection<DateTimePoint> DailyReviewedCount { get; private set; }

        public ObservableCollection<DateTimePoint> DailyMinutesSpent { get; private set; }

        public int TotalReviewedCount { get; private set; }

        public double TotalMinutesSpent { get; private set; }

        public double AverageTimePerFlashcard { get; private set; }

        public bool OrganizeActivityData(User user)
        {
            DailyReviewedCount = new ObservableCollection<DateTimePoint>();
            DailyMinutesSpent = new ObservableCollection<DateTimePoint>();
            TotalMinutesSpent = 0;
            TotalReviewedCount = 0;

            var reviewedCount_InitialList = new List<DateTimePoint>();
            var minutesSpent_InitialList = new List<DateTimePoint>();
            var daysRegisteredList = new List<DateTime>();

            // getting data from user activity (deleted decks' activity)
            if (user.Activity.Count != 0)
            {
                daysRegisteredList.Add(user.Activity[0].Day);
                reviewedCount_InitialList.Add(new DateTimePoint(user.Activity[0].Day, user.Activity[0].ReviewedFlashcardsCount));
                minutesSpent_InitialList.Add(new DateTimePoint(user.Activity[0].Day, user.Activity[0].MinutesSpentLearning));
                TotalReviewedCount += user.Activity[0].ReviewedFlashcardsCount;
                TotalMinutesSpent += user.Activity[0].MinutesSpentLearning;

                for (int i = 1; i < user.Activity.Count; i++)
                {
                    daysRegisteredList.Add(user.Activity[i].Day);
                    reviewedCount_InitialList.Add(new DateTimePoint(user.Activity[i].Day, user.Activity[i].ReviewedFlashcardsCount));
                    minutesSpent_InitialList.Add(new DateTimePoint(user.Activity[i].Day, user.Activity[i].MinutesSpentLearning));
                    TotalReviewedCount += user.Activity[i].ReviewedFlashcardsCount;
                    TotalMinutesSpent += user.Activity[i].MinutesSpentLearning;
                }
            }

            // getting data from each deck's activity
            foreach (Deck deck in user.Decks)
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
                    }
                    if (index >= 0)
                    {
                        reviewedCount_InitialList[index].Value += activity.ReviewedFlashcardsCount;
                        minutesSpent_InitialList[index].Value += activity.MinutesSpentLearning;
                    }
                    TotalReviewedCount += activity.ReviewedFlashcardsCount;
                    TotalMinutesSpent += activity.MinutesSpentLearning;
                }
            }

            if (daysRegisteredList.Count == 0) return false;

            FillHolesBetweenDays(reviewedCount_InitialList, minutesSpent_InitialList);

            AverageTimePerFlashcard = TotalMinutesSpent / TotalReviewedCount;

            return true;
        }

        public bool OrganizeActivityData(Deck selectedDeck)
        {
            DailyReviewedCount = new ObservableCollection<DateTimePoint>();
            DailyMinutesSpent = new ObservableCollection<DateTimePoint>();
            TotalMinutesSpent = 0;
            TotalReviewedCount = 0;

            var reviewedCount_InitialList = new List<DateTimePoint>();
            var minutesSpent_InitialList = new List<DateTimePoint>();

            foreach (DeckActivity activity in selectedDeck.Activity)
            {
                reviewedCount_InitialList.Add(new DateTimePoint(activity.Day, activity.ReviewedFlashcardsCount));
                minutesSpent_InitialList.Add(new DateTimePoint(activity.Day, activity.MinutesSpentLearning));
                TotalReviewedCount += activity.ReviewedFlashcardsCount;
                TotalMinutesSpent += activity.MinutesSpentLearning;
            }
            if (TotalReviewedCount == 0)
            {
                return false;
            }

            FillHolesBetweenDays(reviewedCount_InitialList, minutesSpent_InitialList);

            AverageTimePerFlashcard = TotalMinutesSpent / TotalReviewedCount;

            return true;
        }

        private void FillHolesBetweenDays(List<DateTimePoint> reviewedCount_InitialList, List<DateTimePoint> minutesSpent_InitialList)
        {
            for (int i = 0; i < reviewedCount_InitialList.Count - 1; i++)
            {
                DailyReviewedCount.Add(reviewedCount_InitialList[i]);
                DailyMinutesSpent.Add(minutesSpent_InitialList[i]);

                while (DailyReviewedCount[^1].DateTime.AddDays(1) != reviewedCount_InitialList[i + 1].DateTime)
                {
                    DailyReviewedCount.Add(new DateTimePoint(DailyReviewedCount[^1].DateTime.AddDays(1), 0));
                    DailyMinutesSpent.Add(new DateTimePoint(DailyMinutesSpent[^1].DateTime.AddDays(1), 0));
                }
            }
            DailyReviewedCount.Add(reviewedCount_InitialList[^1]);
            DailyMinutesSpent.Add(minutesSpent_InitialList[^1]);

            while (DailyReviewedCount[^1].DateTime.AddDays(1) < DateTime.Today)
            {
                DailyReviewedCount.Add(new DateTimePoint(DailyReviewedCount[^1].DateTime.AddDays(1), 0));
                DailyMinutesSpent.Add(new DateTimePoint(DailyMinutesSpent[^1].DateTime.AddDays(1), 0));
            }
        }
    }
}
