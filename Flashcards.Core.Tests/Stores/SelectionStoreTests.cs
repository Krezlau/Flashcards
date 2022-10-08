using Flashcards.Core.Models;
using Flashcards.Core.Stores;
using Flashcards.Core.Tests.DbConnection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Flashcards.Core.Tests.Stores
{
    public class SelectionStoreTests
    {
        private List<User> PrepareData(string name)
        {
            TestDbContextFactory _contextFactory = new TestDbContextFactory(name);
            var context = _contextFactory.CreateDbContext();

            User userOne = new User()
            {
                Name = "user1",
                Email = "user1@test",
                PasswordHash = "hash"
            };
            context.Users.Add(userOne);

            User userTwo = new User()
            {
                Name = "user2",
                Email = "user2@test",
                PasswordHash = "hash"
            };
            context.Users.Add(userTwo);

            User userThree = new User()
            {
                Name = "user3",
                Email = "user3@test",
                PasswordHash = "hash"
            };
            context.Users.Add(userThree);
            context.SaveChanges();


            Deck deckOne = new Deck()
            {
                Name = "deck1",
                UserId = userOne.Id
            };
            context.Decks.Add(deckOne);

            Deck deckTwo = new Deck()
            {
                Name = "deck2",
                UserId = userOne.Id
            };
            context.Decks.Add(deckTwo);

            Deck deckThree = new Deck()
            {
                Name = "deck3",
                UserId = userTwo.Id
            };
            context.Decks.Add(deckThree);
            context.SaveChanges();


            Flashcard flashcardOne = new Flashcard()
            {
                Front = "front1",
                Back = "back1",
                DeckId = deckOne.Id,
                Level = 0,
                NextReview = DateTime.Parse("2022-04-14")
            };
            context.Flashcards.Add(flashcardOne);

            Flashcard flashcardTwo = new Flashcard()
            {
                Front = "front2",
                Back = "back2",
                DeckId = deckOne.Id,
                Level = 0,
                NextReview = DateTime.Parse("2022-04-14")
            };
            context.Flashcards.Add(flashcardTwo);

            Flashcard flashcardThree = new Flashcard()
            {
                Front = "front3",
                Back = "back3",
                DeckId = deckTwo.Id,
                Level = 0,
                NextReview = DateTime.Parse("2022-04-14")
            };
            context.Flashcards.Add(flashcardThree);
            context.SaveChanges();

            DailyActivity activityOne = new DailyActivity()
            {
                Day = DateTime.Parse("2022-03-30"),
                ReviewedFlashcardsCount = 1,
                UserId = userOne.Id
            };
            context.DailyActivity.Add(activityOne);

            DailyActivity activityTwo = new DailyActivity()
            {
                Day = DateTime.Today,
                ReviewedFlashcardsCount = 1,
                UserId = userTwo.Id
            };
            context.DailyActivity.Add(activityTwo);

            DailyActivity activityThree = new DailyActivity()
            {
                Day = DateTime.Today.AddDays(-1),
                ReviewedFlashcardsCount = 1,
                UserId = userTwo.Id
            };
            context.DailyActivity.Add(activityThree);

            context.SaveChanges();

            return context.Users.ToList();
        }

        [Fact]
        public void GetSelectedDeckIndexTest_WithCorrectDeck_ShouldReturnDeckIndex()
        {
            SelectionStore store = new SelectionStore();
            var users = PrepareData(nameof(GetSelectedDeckIndexTest_WithCorrectDeck_ShouldReturnDeckIndex));

            for (int i = 0; i < users.Count; i++)
            {
                if (users[i].Decks is null) continue;
                for (int j = 0; j < users[i].Decks.Count; j++)
                {
                    store.SelectedDeck = users[i].Decks[j];
                    int index = store.GetSelectedDeckIndex(users[i]);
                    Assert.Equal(j, index);
                }
            }
        }

        [Fact]
        public void GetSelectedDeckIndexTest_WithIncorrectDeck_ShouldReturnMinusOne()
        {
            SelectionStore store = new SelectionStore();
            var users = PrepareData(nameof(GetSelectedDeckIndexTest_WithIncorrectDeck_ShouldReturnMinusOne));
            store.SelectedDeck = new Deck("dek", 39);

            for (int i = 0; i < users.Count; i++)
            {
                int index = store.GetSelectedDeckIndex(users[i]);
                Assert.Equal(-1, index);
            }
        }

        [Fact]
        public void GetSelectedFlashcardIndexTest_WithCorrectFlashcard_ShouldReturnFlashcardIndex()
        {
            SelectionStore store = new SelectionStore();
            var users = PrepareData(nameof(GetSelectedFlashcardIndexTest_WithCorrectFlashcard_ShouldReturnFlashcardIndex));

            for (int i = 0; i < users.Count; i++)
            {
                if (users[i].Decks is null) continue;
                for (int j = 0; j < users[i].Decks.Count; j++)
                {
                    store.SelectedDeck = users[i].Decks[j];
                    if (users[i].Decks[j].Flashcards is null) continue;
                    for (int k = 0; k < users[i].Decks[j].Flashcards.Count; k++)
                    {
                        store.SelectedFlashcard = users[i].Decks[j].Flashcards[k];
                        int index = store.GetSelectedFlashcardIndex();
                        Assert.Equal(k, index);
                    }
                }
            }
        }

        [Fact]
        public void GetSelectedFlashcardIndexTest_WithIncorrectFlashcard_ShouldReturnMinusOne()
        {
            SelectionStore store = new SelectionStore();
            var users = PrepareData(nameof(GetSelectedFlashcardIndexTest_WithIncorrectFlashcard_ShouldReturnMinusOne));
            store.SelectedFlashcard = new Flashcard()
            {
                Id = 1000,
                Front = "frontt",
                Back = "backk",
                DeckId = 1000,
                Level = 0,
                NextReview = DateTime.Today
            };


            for (int i = 0; i < users.Count; i++)
            {
                if (users[i].Decks is null) continue;
                for (int j = 0; j < users[i].Decks.Count; j++)
                {
                    store.SelectedDeck = users[i].Decks[j];
                    int index = store.GetSelectedFlashcardIndex();
                    Assert.Equal(-1, index);
                }
            }
        }
    }
}
