using Flashcards.Core.Models;
using Flashcards.Core.Services;
using LiveChartsCore.Defaults;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Flashcards.Core.Tests.Services
{
    public class ActivityDataOrganizerTests
    {
        [Fact]
        public void OrganizeActivityData_UserWithNoActivityAndDecks_ShouldReturnFalse()
        {
            var userOne = new User()
            {
                Name = "user1",
                Email = "user@user",
                Id = 0,
                PasswordHash = "hash"
            };

            var userTwo = new User()
            {
                Name = "user2",
                Email = "user2@user",
                Id = 1,
                PasswordHash = "hash",
                Decks = new ObservableCollection<Deck>(),
                Activity = new List<DailyActivity>()
            };

            var organizer = new ActivityDataOrganizer();

            bool outcome = organizer.OrganizeActivityData(userOne);

            Assert.False(outcome);

            outcome = organizer.OrganizeActivityData(userTwo);
            
            Assert.False(outcome);
        }

        [Fact]
        public void OrganizeActivityData_UserWithDeckButNoDeckOrUserActivity_ShouldReturnFalse()
        {
            var userOne = new User()
            {
                Name = "user1",
                Email = "user@user",
                Id = 0,
                PasswordHash = "hash",
                Decks = new ObservableCollection<Deck>()
            };

            var deckOne = new Deck()
            {
                Activity = new List<DeckActivity>(),
                Id = 0,
                Name = "deck1",
                UserId = 0,
                Flashcards = new ObservableCollection<Flashcard>()
            };

            var deckTwo = new Deck()
            {
                Id = 1,
                Name = "deck2",
                UserId = 0,
                Flashcards = new ObservableCollection<Flashcard>()
            };

            userOne.Decks.Add(deckOne);
            userOne.Decks.Add(deckTwo);

            var organizer = new ActivityDataOrganizer();

            bool outcome = organizer.OrganizeActivityData(userOne);

            Assert.False(outcome);
        }

        [Fact]
        public void OrganizeActivityData_UserIsNull_ShouldThrowArgumentNullException()
        {
            User user = null;

            var organizer = new ActivityDataOrganizer();

            Assert.Throws<ArgumentNullException>(() => organizer.OrganizeActivityData(user));
        }

        [Fact]
        public void OrganizeActivityData_UserWithActivityButNoDecks_ShouldReturnTrue()
        {
            var user = new User()
            {
                Activity = new List<DailyActivity>(),
                Decks = new ObservableCollection<Deck>(),
                Email = "user@user",
                Id = 0,
                Name = "user",
                PasswordHash = "hash"
            };

            var activityOne = new DailyActivity()
            {
                Day = DateTime.Today.AddDays(-3),
                Id = 1,
                MinutesSpentLearning = 10.2,
                ReviewedFlashcardsCount = 30,
                UserId = 0
            };

            var activityTwo = new DailyActivity()
            {
                Day = DateTime.Today.AddDays(-1),
                Id = 2,
                MinutesSpentLearning = 12.4,
                ReviewedFlashcardsCount = 35,
                UserId = 0
            };

            user.Activity.Add(activityOne);
            user.Activity.Add(activityTwo);

            var expectedMinutes = new ObservableCollection<DateTimePoint>();
            var expectedCount = new ObservableCollection<DateTimePoint>();

            expectedMinutes.Add(new DateTimePoint(DateTime.Today.AddDays(-3), 10.2));
            expectedMinutes.Add(new DateTimePoint(DateTime.Today.AddDays(-2), 0));
            expectedMinutes.Add(new DateTimePoint(DateTime.Today.AddDays(-1), 12.4));
            expectedMinutes.Add(new DateTimePoint(DateTime.Today, 0));

            expectedCount.Add(new DateTimePoint(DateTime.Today.AddDays(-3), 30));
            expectedCount.Add(new DateTimePoint(DateTime.Today.AddDays(-2), 0));
            expectedCount.Add(new DateTimePoint(DateTime.Today.AddDays(-1), 35));
            expectedCount.Add(new DateTimePoint(DateTime.Today, 0));


            var organizer = new ActivityDataOrganizer();

            bool outcome = organizer.OrganizeActivityData(user);

            Assert.True(outcome);

            for (int i = 0; i < expectedCount.Count; i++)
            {
                Assert.Equal(expectedCount[i].DateTime,
                             organizer.DailyReviewedCount[i].DateTime);

                Assert.Equal((double)expectedCount[i].Value,
                             (double)organizer.DailyReviewedCount[i].Value,
                             4);

                Assert.Equal(expectedMinutes[i].DateTime,
                             organizer.DailyMinutesSpent[i].DateTime);

                Assert.Equal((double)expectedMinutes[i].Value,
                             (double)organizer.DailyMinutesSpent[i].Value,
                             4);
            }

            Assert.Equal(65, organizer.TotalReviewedCount);
            Assert.Equal(22.6, organizer.TotalMinutesSpent, 4);
            Assert.Equal(22.6 / 65.0, organizer.AverageTimePerFlashcard, 4);
        }

        [Fact]
        public void OrganizeActivityData_UserWithBothUserActivityAndDeckActivity_ShouldReturnTrue()
        {
            var user = new User()
            {
                Activity = new List<DailyActivity>(),
                Decks = new ObservableCollection<Deck>(),
                Email = "user@user",
                Id = 0,
                Name = "user",
                PasswordHash = "hash"
            };

            var activityOne = new DailyActivity()
            {
                Day = DateTime.Today.AddDays(-3),
                Id = 1,
                MinutesSpentLearning = 15.8,
                ReviewedFlashcardsCount = 57,
                UserId = 0
            };

            var activityTwo = new DailyActivity()
            {
                Day = DateTime.Today,
                Id = 2,
                MinutesSpentLearning = 8.4,
                ReviewedFlashcardsCount = 21,
                UserId = 0
            };

            user.Activity.Add(activityOne);
            user.Activity.Add(activityTwo);

            var deck = new Deck("deck", 0);
            var deckTwo = new Deck("deck2", 0);

            var DeckActivityOne = new DeckActivity()
            {
                Day = DateTime.Today.AddDays(-3),
                Id = 1,
                MinutesSpentLearning = 10.2,
                ReviewedFlashcardsCount = 30,
                DeckId = 0
            };

            var DeckActivityTwo = new DeckActivity()
            {
                Day = DateTime.Today.AddDays(-1),
                Id = 2,
                MinutesSpentLearning = 12.4,
                ReviewedFlashcardsCount = 35,
                DeckId = 0
            };

            var DeckActivityThree = new DeckActivity()
            {
                Day = DateTime.Today.AddDays(-1),
                Id = 3,
                MinutesSpentLearning = 1,
                ReviewedFlashcardsCount = 1,
                DeckId = 1
            };

            deck.Activity.Add(DeckActivityOne);
            deck.Activity.Add(DeckActivityTwo);
            deckTwo.Activity.Add(DeckActivityThree);
            user.Decks.Add(deck);
            user.Decks.Add(deckTwo);

            var expectedMinutes = new ObservableCollection<DateTimePoint>();
            var expectedCount = new ObservableCollection<DateTimePoint>();

            expectedMinutes.Add(new DateTimePoint(DateTime.Today.AddDays(-3), 26));
            expectedMinutes.Add(new DateTimePoint(DateTime.Today.AddDays(-2), 0));
            expectedMinutes.Add(new DateTimePoint(DateTime.Today.AddDays(-1), 13.4));
            expectedMinutes.Add(new DateTimePoint(DateTime.Today, 8.4));

            expectedCount.Add(new DateTimePoint(DateTime.Today.AddDays(-3), 87));
            expectedCount.Add(new DateTimePoint(DateTime.Today.AddDays(-2), 0));
            expectedCount.Add(new DateTimePoint(DateTime.Today.AddDays(-1), 36));
            expectedCount.Add(new DateTimePoint(DateTime.Today, 21));


            var organizer = new ActivityDataOrganizer();

            bool outcome = organizer.OrganizeActivityData(user);

            Assert.True(outcome);

            for (int i = 0; i < expectedCount.Count; i++)
            {
                Assert.Equal(expectedCount[i].DateTime,
                             organizer.DailyReviewedCount[i].DateTime);

                Assert.Equal((double)expectedCount[i].Value,
                             (double)organizer.DailyReviewedCount[i].Value,
                             4);

                Assert.Equal(expectedMinutes[i].DateTime,
                             organizer.DailyMinutesSpent[i].DateTime);

                Assert.Equal((double)expectedMinutes[i].Value,
                             (double)organizer.DailyMinutesSpent[i].Value,
                             4);
            }

            Assert.Equal(144, organizer.TotalReviewedCount);
            Assert.Equal(47.8, organizer.TotalMinutesSpent, 4);
            Assert.Equal(47.8 / 144.0, organizer.AverageTimePerFlashcard, 4);
        }

        [Fact]
        public void OrganizeActivityData_UserWithOnlyDeckActivity_ShouldReturnTrue()
        {
            var user = new User()
            {
                Activity = new List<DailyActivity>(),
                Decks = new ObservableCollection<Deck>(),
                Email = "user@user",
                Id = 0,
                Name = "user",
                PasswordHash = "hash"
            };

            var deck = new Deck("deck", 0);
            var deckTwo = new Deck("deck2", 0);

            var activityOne = new DeckActivity()
            {
                Day = DateTime.Today.AddDays(-3),
                Id = 1,
                MinutesSpentLearning = 10.2,
                ReviewedFlashcardsCount = 30,
                DeckId = 0
            };

            var activityTwo = new DeckActivity()
            {
                Day = DateTime.Today.AddDays(-1),
                Id = 2,
                MinutesSpentLearning = 12.4,
                ReviewedFlashcardsCount = 35,
                DeckId = 0
            };

            var activityThree = new DeckActivity()
            {
                Day = DateTime.Today.AddDays(-1),
                Id = 3,
                MinutesSpentLearning = 3.4,
                ReviewedFlashcardsCount = 5,
                DeckId = 1
            };

            deck.Activity.Add(activityOne);
            deck.Activity.Add(activityTwo);
            deckTwo.Activity.Add(activityThree);
            user.Decks.Add(deck);
            user.Decks.Add(deckTwo);

            var expectedMinutes = new ObservableCollection<DateTimePoint>();
            var expectedCount = new ObservableCollection<DateTimePoint>();

            expectedMinutes.Add(new DateTimePoint(DateTime.Today.AddDays(-3), 10.2));
            expectedMinutes.Add(new DateTimePoint(DateTime.Today.AddDays(-2), 0));
            expectedMinutes.Add(new DateTimePoint(DateTime.Today.AddDays(-1), 15.8));
            expectedMinutes.Add(new DateTimePoint(DateTime.Today, 0));

            expectedCount.Add(new DateTimePoint(DateTime.Today.AddDays(-3), 30));
            expectedCount.Add(new DateTimePoint(DateTime.Today.AddDays(-2), 0));
            expectedCount.Add(new DateTimePoint(DateTime.Today.AddDays(-1), 40));
            expectedCount.Add(new DateTimePoint(DateTime.Today, 0));


            var organizer = new ActivityDataOrganizer();

            bool outcome = organizer.OrganizeActivityData(user);

            Assert.True(outcome);

            for (int i = 0; i < expectedCount.Count; i++)
            {
                Assert.Equal(expectedCount[i].DateTime,
                             organizer.DailyReviewedCount[i].DateTime);

                Assert.Equal((double)expectedCount[i].Value,
                             (double)organizer.DailyReviewedCount[i].Value,
                             4);

                Assert.Equal(expectedMinutes[i].DateTime,
                             organizer.DailyMinutesSpent[i].DateTime);

                Assert.Equal((double)expectedMinutes[i].Value,
                             (double)organizer.DailyMinutesSpent[i].Value,
                             4);
            }

            Assert.Equal(70, organizer.TotalReviewedCount);
            Assert.Equal(26, organizer.TotalMinutesSpent, 4);
            Assert.Equal(26.0 / 70.0, organizer.AverageTimePerFlashcard, 4);
        }

        [Fact]
        public void OrganizeActivityData_DeckIsNull_ShouldThrowArgumentNullException()
        {
            Deck deck = null;

            var organizer = new ActivityDataOrganizer();

            Assert.Throws<ArgumentNullException>(() => organizer.OrganizeActivityData(deck));
        }

        [Fact]
        public void OrganizeActivityData_EmptyDeckWithNoActivity_ShouldReturnFalse()
        {
            Deck deckOne = new Deck()
            {
                Activity = new List<DeckActivity>(),
                Flashcards = new ObservableCollection<Flashcard>(),
                Id = 0,
                Name = "deck1",
                UserId = 0
            };

            Deck deckTwo = new Deck()
            {
                Flashcards = new ObservableCollection<Flashcard>(),
                Id = 1,
                Name = "deck2",
                UserId = 0
            };

            var organizer = new ActivityDataOrganizer();

            Assert.False(organizer.OrganizeActivityData(deckOne));
            Assert.False(organizer.OrganizeActivityData(deckTwo));
        }

        [Fact]
        public void OrganizeActivityData_DeckWithActivity_ShouldReturnTrue()
        {
            var deck = new Deck("deck", 0);

            var activityOne = new DeckActivity()
            {
                Day = DateTime.Today.AddDays(-3),
                Id = 1,
                MinutesSpentLearning = 10.2,
                ReviewedFlashcardsCount = 30,
                DeckId = 0
            };

            var activityTwo = new DeckActivity()
            {
                Day = DateTime.Today.AddDays(-1),
                Id = 2,
                MinutesSpentLearning = 12.4,
                ReviewedFlashcardsCount = 35,
                DeckId = 0
            };

            deck.Activity.Add(activityOne);
            deck.Activity.Add(activityTwo);

            var expectedMinutes = new ObservableCollection<DateTimePoint>();
            var expectedCount = new ObservableCollection<DateTimePoint>();

            expectedMinutes.Add(new DateTimePoint(DateTime.Today.AddDays(-3), 10.2));
            expectedMinutes.Add(new DateTimePoint(DateTime.Today.AddDays(-2), 0));
            expectedMinutes.Add(new DateTimePoint(DateTime.Today.AddDays(-1), 12.4));
            expectedMinutes.Add(new DateTimePoint(DateTime.Today, 0));

            expectedCount.Add(new DateTimePoint(DateTime.Today.AddDays(-3), 30));
            expectedCount.Add(new DateTimePoint(DateTime.Today.AddDays(-2), 0));
            expectedCount.Add(new DateTimePoint(DateTime.Today.AddDays(-1), 35));
            expectedCount.Add(new DateTimePoint(DateTime.Today, 0));


            var organizer = new ActivityDataOrganizer();

            bool outcome = organizer.OrganizeActivityData(deck);

            Assert.True(outcome);

            for (int i = 0; i < expectedCount.Count; i++)
            {
                Assert.Equal(expectedCount[i].DateTime,
                             organizer.DailyReviewedCount[i].DateTime);

                Assert.Equal((double)expectedCount[i].Value,
                             (double)organizer.DailyReviewedCount[i].Value,
                             4);

                Assert.Equal(expectedMinutes[i].DateTime,
                             organizer.DailyMinutesSpent[i].DateTime);

                Assert.Equal((double)expectedMinutes[i].Value,
                             (double)organizer.DailyMinutesSpent[i].Value,
                             4);
            }

            Assert.Equal(65, organizer.TotalReviewedCount);
            Assert.Equal(22.6, organizer.TotalMinutesSpent, 4);
            Assert.Equal(22.6 / 65.0, organizer.AverageTimePerFlashcard, 4);
        }
    }
}
