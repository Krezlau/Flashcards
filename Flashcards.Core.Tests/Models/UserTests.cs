using Flashcards.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Flashcards.Core.Tests.Models
{
    public class UserTests
    {

        public static IEnumerable<object[]> CalculateStreakData_OnlyDaily()
        {
            var userOne = new User()
            {
                Id = 1,
                Name = "user1",
                Email = "email",
                PasswordHash = "hash"
            };

            var userTwo = new User()
            {
                Id = 2,
                Name = "user2",
                Email = "email",
                PasswordHash = "hash",
                Decks = new ObservableCollection<Deck>(),
                Activity = new List<DailyActivity>()
            };

            var userThree = new User()
            {
                Id = 3,
                Name = "user3",
                Email = "email",
                PasswordHash = "hash",
                Activity = new List<DailyActivity>()
            };

            var userFour = new User()
            {
                Id = 4,
                Name = "user4",
                Email = "email",
                PasswordHash = "hash",
                Activity = new List<DailyActivity>()
            };

            var activList = new List<DailyActivity>();
            for (int i = 0; i < 7; i++)
            {
                activList.Add(new DailyActivity()
                {
                    Id = i + 1,
                    Day = DateTime.Parse("2022-11-15").AddDays(-i),
                    MinutesSpentLearning = i + 1,
                    ReviewedFlashcardsCount = i + 1,
                });
            }

            userTwo.Activity.AddRange(activList.OrderBy(a => a.Day));
            userThree.Activity.AddRange(activList.Where(a => a.Id < 3 || a.Id > 5).OrderBy(a => a.Day));
            userFour.Activity.AddRange(activList.Where(a => a.Id > 1).OrderBy(a => a.Day));

            return new List<object[]>
            {
                new object[] {userOne, 0},
                new object[] {userTwo, 7},
                new object[] {userThree, 2 },
                new object[] {userFour, 0 }
            };
        }

        public static IEnumerable<object[]> CalculateStreakData_OnlyDeck()
        {
            var userOne = new User()
            {
                Id = 1,
                Name = "user1",
                Email = "email",
                PasswordHash = "hash"
            };

            var userTwo = new User()
            {
                Id = 2,
                Name = "user2",
                Email = "email",
                PasswordHash = "hash",
                Decks = new ObservableCollection<Deck>(),
                Activity = new List<DailyActivity>()
            };

            var userThree = new User()
            {
                Id = 3,
                Name = "user3",
                Email = "email",
                PasswordHash = "hash",
                Decks = new ObservableCollection<Deck>()
            };

            var userFour = new User()
            {
                Id = 4,
                Name = "user4",
                Email = "email",
                PasswordHash = "hash",
                Decks = new ObservableCollection<Deck>()
            };

            var userFive = new User()
            {
                Id = 5,
                Name = "user5",
                Email = "email",
                PasswordHash = "hash",
                Decks = new ObservableCollection<Deck>()
            };

            var deckList = new List<Deck>();
            for (int i = 0; i < 5; i++)
            {
                deckList.Add(new Deck()
                {
                    Activity = new List<DeckActivity>(),
                    Flashcards = new ObservableCollection<Flashcard>(),
                    Id = i + 1,
                    Name = $"deck{i + 1}"
                });
            }
            deckList.Add(new Deck()
            {
                Id = 6,
                Name = "deck6"
            });


            var activList = new List<DeckActivity>();
            for (int i = 0; i < 7; i++)
            {
                activList.Add(new DeckActivity()
                {
                    Id = i + 1,
                    Day = DateTime.Parse("2022-11-15").AddDays(-i),
                    MinutesSpentLearning = i + 1,
                    ReviewedFlashcardsCount = i + 1,
                });
            }

            deckList[1].Activity.AddRange(activList.OrderBy(a => a.Day));
            deckList[2].Activity.AddRange(activList.Where(a => a.Id > 1).OrderBy(a => a.Day));
            deckList[3].Activity.AddRange(activList.Where(a => a.Id == 1 || a.Id == 5).OrderBy(a => a.Day));
            deckList[4].Activity.AddRange(activList.Where(a => a.Id > 1 && a.Id < 5).OrderBy(a => a.Day));

            userTwo.Decks.Add(deckList[0]);
            userTwo.Decks.Add(deckList[5]);

            userThree.Decks.Add(deckList[0]);
            userThree.Decks.Add(deckList[5]);
            userThree.Decks.Add(deckList[1]);
            userThree.Decks.Add(deckList[3]);

            userFour.Decks.Add(deckList[0]);
            userFour.Decks.Add(deckList[5]);
            userFour.Decks.Add(deckList[4]);

            userFive.Decks.Add(deckList[0]);
            userFive.Decks.Add(deckList[3]);
            userFive.Decks.Add(deckList[4]);

            return new List<object[]>
            {
                new object[] {userOne, 0 },
                new object[] {userTwo, 0 },
                new object[] {userThree, 7 },
                new object[] {userFour, 0 },
                new object[] {userFive, 5}
            };
        }

        public static IEnumerable<object[]> CalculateStreakData_Both()
        {
            var userOne = new User()
            {
                Id = 1,
                Name = "user1",
                Email = "email",
                PasswordHash = "hash"
            };

            var userTwo = new User()
            {
                Id = 2,
                Name = "user2",
                Email = "email",
                PasswordHash = "hash",
                Decks = new ObservableCollection<Deck>(),
                Activity = new List<DailyActivity>()
            };

            var userThree = new User()
            {
                Id = 3,
                Name = "user3",
                Email = "email",
                PasswordHash = "hash",
                Decks = new ObservableCollection<Deck>(),
                Activity = new List<DailyActivity>()
            };

            var userFour = new User()
            {
                Id = 4,
                Name = "user4",
                Email = "email",
                PasswordHash = "hash",
                Decks = new ObservableCollection<Deck>(),
                Activity = new List<DailyActivity>()
            };

            var userFive = new User()
            {
                Id = 5,
                Name = "user5",
                Email = "email",
                PasswordHash = "hash",
                Decks = new ObservableCollection<Deck>(),
                Activity = new List<DailyActivity>()
            };

            var deckList = new List<Deck>();
            for (int i = 0; i < 5; i++)
            {
                deckList.Add(new Deck()
                {
                    Activity = new List<DeckActivity>(),
                    Flashcards = new ObservableCollection<Flashcard>(),
                    Id = i + 1,
                    Name = $"deck{i + 1}"
                });
            }
            deckList.Add(new Deck()
            {
                Id = 6,
                Name = "deck6"
            });


            var activList = new List<DeckActivity>();
            for (int i = 0; i < 7; i++)
            {
                activList.Add(new DeckActivity()
                {
                    Id = i + 1,
                    Day = DateTime.Parse("2022-11-15").AddDays(-i),
                    MinutesSpentLearning = i + 1,
                    ReviewedFlashcardsCount = i + 1,
                });
            }

            var userActivList = new List<DailyActivity>();
            for (int i = 0; i < 7; i++)
            {
                userActivList.Add(new DailyActivity()
                {
                    Id = i + 1,
                    Day = DateTime.Parse("2022-11-15").AddDays(-i),
                    MinutesSpentLearning = i + 1,
                    ReviewedFlashcardsCount = i + 1,
                });
            }

            userTwo.Activity.AddRange(userActivList.OrderBy(a => a.Day));
            userThree.Activity.AddRange(userActivList.Where(a => a.Id < 3 || a.Id > 5).OrderBy(a => a.Day));
            userFour.Activity.AddRange(userActivList.Where(a => a.Id > 1).OrderBy(a => a.Day));
            userFive.Activity.AddRange(userActivList.Where(a => a.Id % 2 == 0).OrderBy(a => a.Day));

            deckList[1].Activity.AddRange(activList.OrderBy(a => a.Day));
            deckList[2].Activity.AddRange(activList.Where(a => a.Id % 2 == 1).OrderBy(a => a.Day));
            deckList[3].Activity.AddRange(activList.Where(a => a.Id == 1 || a.Id == 5).OrderBy(a => a.Day));
            deckList[4].Activity.AddRange(activList.Where(a => a.Id > 1 && a.Id < 5).OrderBy(a => a.Day));

            userTwo.Decks.Add(deckList[0]);
            userTwo.Decks.Add(deckList[5]);

            userThree.Decks.Add(deckList[0]);
            userThree.Decks.Add(deckList[5]);
            userThree.Decks.Add(deckList[4]);
            userThree.Decks.Add(deckList[3]);

            userFour.Decks.Add(deckList[0]);
            userFour.Decks.Add(deckList[5]);
            userFour.Decks.Add(deckList[4]);

            userFive.Decks.Add(deckList[0]);
            userFive.Decks.Add(deckList[2]);
            userFive.Decks.Add(deckList[5]);

            return new List<object[]>
            {
                new object[] {userOne, 0 },
                new object[] {userTwo, 7 },
                new object[] {userThree, 7 },
                new object[] {userFour, 0 },
                new object[] {userFive, 7}
            };
        }

        public static IEnumerable<object[]> IfLearnedTodayFalseData()
        {
            var list = CalculateStreakData_Both();
            var list2 = CalculateStreakData_OnlyDaily();
            var list3 = CalculateStreakData_OnlyDeck();

            var ret = new List<object[]>();
            
            foreach (object[] o in list)
            {
                int x = (int)o[1];
                if (x == 0)
                {
                    ret.Add(new object[] { o[0] });
                }
            }

            foreach (object[] o in list2)
            {
                int x = (int)o[1];
                if (x == 0)
                {
                    ret.Add(new object[] { o[0] });
                }
            }

            foreach (object[] o in list3)
            {
                int x = (int)o[1];
                if (x == 0)
                {
                    ret.Add(new object[] { o[0] });
                }
            }

            return ret;
        }

        public static IEnumerable<object[]> IfLearnedTodayTrueData()
        {
            var list = CalculateStreakData_Both();
            var list2 = CalculateStreakData_OnlyDaily();
            var list3 = CalculateStreakData_OnlyDeck();

            var ret = new List<object[]>();

            foreach (object[] o in list)
            {
                int x = (int)o[1];
                if (x != 0)
                {
                    ret.Add(new object[] { o[0] });
                }
            }

            foreach (object[] o in list2)
            {
                int x = (int)o[1];
                if (x != 0)
                {
                    ret.Add(new object[] { o[0] });
                }
            }

            foreach (object[] o in list3)
            {
                int x = (int)o[1];
                if (x != 0)
                {
                    ret.Add(new object[] { o[0] });
                }
            }

            return ret;
        }

        [Fact]
        public void CalculateStreak_UserWithNoActivity_ShouldReturnZero()
        {
            var userWithNulls = new User()
            {
                Id = 1,
                Name = "user1",
                Email = "email",
                PasswordHash = "hash"
            };

            var userWithDecksEqualsNull = new User()
            {
                Id = 1,
                Name = "user1",
                Email = "email",
                PasswordHash = "hash",
                Activity = new List<DailyActivity>()
            };

            var userWithNoDecks = new User()
            {
                Id = 1,
                Name = "user1",
                Email = "email",
                PasswordHash = "hash",
                Activity = new List<DailyActivity>(),
                Decks = new ObservableCollection<Deck>()
            };

            var userWithDecksButNoActivity_Null = new User()
            {
                Id = 1,
                Name = "user1",
                Email = "email",
                PasswordHash = "hash",
                Activity = new List<DailyActivity>(),
                Decks = new ObservableCollection<Deck>()
            };

            var deckWithActivityNull = new Deck()
            {
                Id = 1,
                Name = "deck"
            };
            userWithDecksButNoActivity_Null.Decks.Add(deckWithActivityNull);

            var userWithDecksButNoActivity = new User()
            {
                Id = 1,
                Name = "user1",
                Email = "email",
                PasswordHash = "hash",
                Activity = new List<DailyActivity>(),
                Decks = new ObservableCollection<Deck>()
            };

            var deckWithNoActivity = new Deck()
            {
                Activity = new List<DeckActivity>(),
                Id = 2,
                Name = "deck"
            };
            userWithDecksButNoActivity.Decks.Add(deckWithNoActivity);

            Assert.Equal(0, userWithNulls.CalculateStreak(DateTime.Parse("2022-11-15")));
            Assert.Equal(0, userWithDecksEqualsNull.CalculateStreak(DateTime.Parse("2022-11-15")));
            Assert.Equal(0, userWithNoDecks.CalculateStreak(DateTime.Parse("2022-11-15")));
            Assert.Equal(0, userWithDecksButNoActivity_Null.CalculateStreak(DateTime.Parse("2022-11-15")));
            Assert.Equal(0, userWithDecksButNoActivity.CalculateStreak(DateTime.Parse("2022-11-15")));
        }

        [Theory]
        [MemberData(nameof(CalculateStreakData_OnlyDaily))]
        public void CalculateStreak_UserWithOnlyUserActivity(User user, int expectedValue)
        {
            Assert.Equal(expectedValue, user.CalculateStreak(DateTime.Parse("2022-11-15")));
        }

        [Theory]
        [MemberData(nameof(CalculateStreakData_OnlyDeck))]
        public void CalculateStreak_UserWithOnlyDeckActivity(User user, int expectedValue)
        {
            Assert.Equal(expectedValue, user.CalculateStreak(DateTime.Parse("2022-11-15")));
        }

        [Theory]
        [MemberData(nameof(CalculateStreakData_Both))]
        public void CalculateStreak_UserWithBothDeckAndUserActivity(User user, int expectedValue)
        {
            Assert.Equal(expectedValue, user.CalculateStreak(DateTime.Parse("2022-11-15")));
        }

        [Fact]
        public void IfLearnedToday_UserWithNoActivityAndDecks_ShouldReturnFalse()
        {
            var userWithNulls = new User()
            {
                Id = 1,
                Name = "user1",
                Email = "email",
                PasswordHash = "hash"
            };

            var userWithDecksEqualsNull = new User()
            {
                Id = 1,
                Name = "user1",
                Email = "email",
                PasswordHash = "hash",
                Activity = new List<DailyActivity>()
            };

            var userWithNoDecks = new User()
            {
                Id = 1,
                Name = "user1",
                Email = "email",
                PasswordHash = "hash",
                Activity = new List<DailyActivity>(),
                Decks = new ObservableCollection<Deck>()
            };

            var userWithDecksButNoActivity_Null = new User()
            {
                Id = 1,
                Name = "user1",
                Email = "email",
                PasswordHash = "hash",
                Activity = new List<DailyActivity>(),
                Decks = new ObservableCollection<Deck>()
            };

            var deckWithActivityNull = new Deck()
            {
                Id = 1,
                Name = "deck"
            };
            userWithDecksButNoActivity_Null.Decks.Add(deckWithActivityNull);

            var userWithDecksButNoActivity = new User()
            {
                Id = 1,
                Name = "user1",
                Email = "email",
                PasswordHash = "hash",
                Activity = new List<DailyActivity>(),
                Decks = new ObservableCollection<Deck>()
            };

            var deckWithNoActivity = new Deck()
            {
                Activity = new List<DeckActivity>(),
                Id = 2,
                Name = "deck"
            };
            userWithDecksButNoActivity.Decks.Add(deckWithNoActivity);

            Assert.False(userWithNulls.IfLearnedToday(DateTime.Parse("2022-11-15")));
            Assert.False(userWithDecksEqualsNull.IfLearnedToday(DateTime.Parse("2022-11-15")));
            Assert.False(userWithNoDecks.IfLearnedToday(DateTime.Parse("2022-11-15")));
            Assert.False(userWithDecksButNoActivity_Null.IfLearnedToday(DateTime.Parse("2022-11-15")));
            Assert.False(userWithDecksButNoActivity.IfLearnedToday(DateTime.Parse("2022-11-15")));
        }

        [Theory]
        [MemberData(nameof(IfLearnedTodayFalseData))]
        public void IfLearnedToday_NoActivityToday_ShouldReturnFalse(User user)
        {
            Assert.False(user.IfLearnedToday(DateTime.Parse("2022-11-15")));
        }

        [Theory]
        [MemberData(nameof(IfLearnedTodayTrueData))]
        public void IfLearnedToday_ActivityToday_ShouldReturnTrue(User user)
        {
            Assert.True(user.IfLearnedToday(DateTime.Parse("2022-11-15")));
        }

        [Fact]
        public void EqualsTest()
        {
            User primaryUser = new User
            {
                Name = "user",
                Id = 1,
                Email = "email",
                PasswordHash = "hash"
            };

            User userNull = null;
            Deck notAUser = new Deck("deck", 2);
            User userWithDifferentId = new User { Id = 10 };
            User userWithTheSameId = new User { Id = 1 };

            Assert.False(primaryUser.Equals(userNull));
            Assert.False(primaryUser.Equals(notAUser));
            Assert.False(primaryUser.Equals(userWithDifferentId));
            Assert.True(primaryUser.Equals(userWithTheSameId));
            Assert.True(primaryUser.Equals(primaryUser));
        }
    }
}
