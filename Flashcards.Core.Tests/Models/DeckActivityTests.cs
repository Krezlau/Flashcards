using Flashcards.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Flashcards.Core.Tests.Models
{
    public class DeckActivityTests
    {
        [Fact]
        public void EqualsTest()
        {
            DeckActivity primaryActivity = new DeckActivity()
            {
                Id = 1,
                Day = DateTime.Parse("2022-12-12"),
                MinutesSpentLearning = 1,
                ReviewedFlashcardsCount = 14,
                DeckId = 1
            };

            DeckActivity activityNull = null;
            Deck notAnActivity = new Deck("deck", 1);
            DeckActivity activityWithDifferentId = new DeckActivity() { Id = 10 };
            DeckActivity activityWithTheSameId = new DeckActivity() { Id = 1 };

            Assert.False(primaryActivity.Equals(activityNull));
            Assert.False(primaryActivity.Equals(notAnActivity));
            Assert.False(primaryActivity.Equals(activityWithDifferentId));
            Assert.True(primaryActivity.Equals(activityWithTheSameId));
            Assert.True(primaryActivity.Equals(primaryActivity));
        }

        [Fact]
        public void ToDailyActivityTest()
        {
            DeckActivity deckActivity = new DeckActivity()
            {
                Id = 1,
                Day = DateTime.Parse("2022-12-12"),
                MinutesSpentLearning = 1,
                ReviewedFlashcardsCount = 14,
                DeckId = 1
            };

            DailyActivity dailyActivity = deckActivity.ToDailyActivity(1);

            Assert.Equal(1, dailyActivity.UserId);
            Assert.Equal(deckActivity.Day, dailyActivity.Day);
            Assert.Equal(deckActivity.MinutesSpentLearning, dailyActivity.MinutesSpentLearning);
            Assert.Equal(deckActivity.ReviewedFlashcardsCount, dailyActivity.ReviewedFlashcardsCount);
        }
    }
}
