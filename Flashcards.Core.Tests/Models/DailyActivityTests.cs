using Flashcards.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Flashcards.Core.Tests.Models
{
    public class DailyActivityTests
    {
        [Fact]
        public void EqualsTest()
        {
            DailyActivity primaryDailyActivity = new DailyActivity()
            {
                Id = 1,
                Day = DateTime.Parse("2022-12-12"),
                MinutesSpentLearning = 1,
                ReviewedFlashcardsCount = 12,
                UserId = 1
            };

            DailyActivity activityNull = null;
            Deck notAnActivity = new Deck("deck", 1);
            DailyActivity activityWithDifferentId = new DailyActivity { Id = 10 };
            DailyActivity activityWithTheSameId = new DailyActivity { Id = 1 };

            Assert.False(primaryDailyActivity.Equals(activityNull));
            Assert.False(primaryDailyActivity.Equals(notAnActivity));
            Assert.False(primaryDailyActivity.Equals(activityWithDifferentId));
            Assert.True(primaryDailyActivity.Equals(activityWithTheSameId));
            Assert.True(primaryDailyActivity.Equals(primaryDailyActivity));
        }
    }
}
