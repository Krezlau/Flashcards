using Flashcards.Core.DBConnection;
using Flashcards.Core.Models;
using Flashcards.Core.Services.UserDataDestroyers;
using Flashcards.Core.Tests.DbConnection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Flashcards.Core.Tests.Services.UserDataDestroyers
{
    public class DatabaseUserDataDestroyerTests
    {
        private User CreateSampleUser(TestDbContextFactory factory)
        {
            User user;
            using (var context = factory.CreateDbContext())
            {
                user = new User
                {
                    Name = "TestUser",
                    Email = "test@user",
                    PasswordHash = "hash"
                };
                context.Users.Add(user);

                Deck deck1 = new Deck("deckOne", user.Id);
                Deck deck2 = new Deck("deckTwo", user.Id);
                Deck deck3 = new Deck("deckThree", user.Id);

                context.Decks.Add(deck1);
                context.Decks.Add(deck2);
                context.Decks.Add(deck3);

                Flashcard flashcard1 = new Flashcard
                {
                    Back = "back1",
                    Front = "front1",
                    DeckId = deck1.Id,
                    Level = 0,
                    NextReview = DateTime.Parse("2022-12-30")
                };
                context.Flashcards.Add(flashcard1);

                Flashcard flashcard2 = new Flashcard
                {
                    Back = "back2",
                    Front = "front2",
                    DeckId = deck1.Id,
                    Level = 1,
                    NextReview = DateTime.Parse("2022-10-25")
                };
                context.Flashcards.Add(flashcard2);

                Flashcard flashcard3 = new Flashcard
                {
                    Back = "back3",
                    Front = "front3",
                    DeckId = deck2.Id,
                    Level = 3,
                    NextReview = DateTime.Parse("2022-11-13")
                };
                context.Flashcards.Add(flashcard3);

                DeckActivity deckActivity1 = new DeckActivity
                {
                    Day = DateTime.Parse("2022-01-01"),
                    MinutesSpentLearning = 15.5,
                    ReviewedFlashcardsCount = 30,
                    Deck = deck1
                };
                context.DeckActivity.Add(deckActivity1);

                DeckActivity deckActivity2 = new DeckActivity
                {
                    Day = DateTime.Parse("2022-02-02"),
                    MinutesSpentLearning = 10.2,
                    ReviewedFlashcardsCount = 40,
                    Deck = deck1
                };
                context.DeckActivity.Add(deckActivity2);

                DailyActivity dailyActivity1 = new DailyActivity
                {
                    Day = DateTime.Parse("2022-01-01"),
                    MinutesSpentLearning = 4.6,
                    ReviewedFlashcardsCount = 14,
                    User = user
                };
                context.DailyActivity.Add(dailyActivity1);

                context.SaveChanges();

            }

            return user;
        }

        [Fact]
        public async void DeleteFlashcardTest()
        {
            var contextFactory = new TestDbContextFactory(nameof(DeleteFlashcardTest));
            var context = contextFactory.CreateDbContext();
            var dataDestroyer = new DatabaseUserDataDestroyer(contextFactory);

            var user = CreateSampleUser(contextFactory);

            var anotherDeck = user.Decks[0].Flashcards[1];

            await dataDestroyer.DeleteFlashcard(user.Decks[0].Flashcards[0]);

            var deckOneRemainingFlashcard = context.Flashcards.Where(f => f.DeckId == user.Decks[0].Id).Single();

            Assert.Equal(anotherDeck, deckOneRemainingFlashcard);
        }

        [Fact]
        public async void DeleteDeckActivityAsyncTest()
        {
            var contextFactory = new TestDbContextFactory(nameof(DeleteDeckActivityAsyncTest));
            var context = contextFactory.CreateDbContext();
            var dataDestroyer = new DatabaseUserDataDestroyer(contextFactory);

            var user = CreateSampleUser(contextFactory);
            var anotherActivity = user.Decks[0].Activity[1];

            await dataDestroyer.DeleteDeckActivityAsync(user.Decks[0].Activity[0]);

            var deckOneRemainingActivity = context.DeckActivity.Where(da => da.DeckId == user.Decks[0].Id).Single();

            Assert.Equal(anotherActivity, deckOneRemainingActivity);
        }

        [Fact]
        public async void DeleteDeckTest_ShouldDeleteCorrectlyWhenDeckHasActivityAndFlashcards()
        {
            var contextFactory = new TestDbContextFactory(nameof(
                DeleteDeckTest_ShouldDeleteCorrectlyWhenDeckHasActivityAndFlashcards));
            var context = contextFactory.CreateDbContext();
            var dataDestroyer = new DatabaseUserDataDestroyer(contextFactory);

            var user = CreateSampleUser(contextFactory);

            await dataDestroyer.DeleteDeck(user.Decks[0]);

            var userActivityInDb = context.DailyActivity.ToList();
            var deckActivitiesInDb = context.DeckActivity.ToList();
            var flashcardsInDb = context.Flashcards.ToList();
            var decksInDb = context.Decks.ToList();

            Assert.Empty(deckActivitiesInDb);
            Assert.Equal(2, decksInDb.Count);
            Assert.Equal(2, userActivityInDb.Count);
            Assert.Equal(44, userActivityInDb[0].ReviewedFlashcardsCount);
            Assert.Equal(20.1, userActivityInDb[0].MinutesSpentLearning);
            Assert.Equal(40, userActivityInDb[1].ReviewedFlashcardsCount);
            Assert.Equal(10.2, userActivityInDb[1].MinutesSpentLearning);
            Assert.Single(flashcardsInDb);
            Assert.Empty(context.Decks.Where(d => d.Name == "deckOne").ToList());
        }

        [Fact]
        public async void DeleteDeckTest_ShouldDeleteCorrectlyWhenDeckHasOnlyFlashcards()
        {
            var contextFactory = new TestDbContextFactory(nameof(
                DeleteDeckTest_ShouldDeleteCorrectlyWhenDeckHasOnlyFlashcards));
            var context = contextFactory.CreateDbContext();
            var dataDestroyer = new DatabaseUserDataDestroyer(contextFactory);

            var user = CreateSampleUser(contextFactory);

            await dataDestroyer.DeleteDeck(user.Decks[1]);

            var userActivityInDb = context.DailyActivity.ToList();
            var deckActivitiesInDb = context.DeckActivity.ToList();
            var flashcardsInDb = context.Flashcards.ToList();
            var decksInDb = context.Decks.ToList();

            Assert.Equal(2, deckActivitiesInDb.Count);
            Assert.Equal(2, decksInDb.Count);
            Assert.Single(userActivityInDb);
            Assert.Equal(14, userActivityInDb[0].ReviewedFlashcardsCount);
            Assert.Equal(4.6, userActivityInDb[0].MinutesSpentLearning);
            Assert.Equal(2, flashcardsInDb.Count);
            Assert.Empty(context.Decks.Where(d => d.Name == "deckTwo").ToList());
        }

        [Fact]
        public async void DeleteDeckTest_ShouldDeleteCorrectlyWhenDeckEmpty()
        {
            var contextFactory = new TestDbContextFactory(nameof(
                DeleteDeckTest_ShouldDeleteCorrectlyWhenDeckEmpty));
            var context = contextFactory.CreateDbContext();
            var dataDestroyer = new DatabaseUserDataDestroyer(contextFactory);

            var user = CreateSampleUser(contextFactory);

            await dataDestroyer.DeleteDeck(user.Decks[2]);

            var userActivityInDb = context.DailyActivity.ToList();
            var deckActivitiesInDb = context.DeckActivity.ToList();
            var flashcardsInDb = context.Flashcards.ToList();
            var decksInDb = context.Decks.ToList();

            Assert.Equal(2, deckActivitiesInDb.Count);
            Assert.Equal(2, decksInDb.Count);
            Assert.Single(userActivityInDb);
            Assert.Equal(14, userActivityInDb[0].ReviewedFlashcardsCount);
            Assert.Equal(4.6, userActivityInDb[0].MinutesSpentLearning);
            Assert.Equal(3, flashcardsInDb.Count);
            Assert.Empty(context.Decks.Where(d => d.Name == "deckThree").ToList());
        }
    }
}
