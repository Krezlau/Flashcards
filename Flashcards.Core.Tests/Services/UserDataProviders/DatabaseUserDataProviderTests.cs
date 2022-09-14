using Flashcards.Core.DBConnection;
using Flashcards.Core.Models;
using Flashcards.Core.Services.UserDataProviders;
using Flashcards.Core.Tests.DbConnection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Flashcards.Core.Tests.Services.UserDataProviders
{
    public class DatabaseUserDataProviderTests
    {
        private List<User> PrepareData(UsersContext context)
        {
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
                Day = DateTime.Parse("2022-04-30"),
                ReviewedFlashcardsCount = 1,
                UserId = userTwo.Id
            };
            context.DailyActivity.Add(activityTwo);
            context.SaveChanges();

            return context.Users.ToList();
        }

        [Fact]
        public async void LoadUserDecksAsyncTest()
        {
            var _contextFactory = new TestDbContextFactory(nameof(LoadUserDecksAsyncTest));
            var _dataProvider = new DatabaseUserDataProvider(_contextFactory);

            var context = _contextFactory.CreateDbContext();
            var users = PrepareData(context);

            var UserOne = await _dataProvider.LoadUserDecksAsync(users[0].Id);
            var UserThree = await _dataProvider.LoadUserDecksAsync(users[2].Id);

            Assert.Equal(users[0], UserOne);
            Assert.Equal(2, UserOne.Decks.Count);
            Assert.Single(UserOne.Activity);
            Assert.Empty(UserThree.Decks);
            Assert.Empty(UserThree.Activity);
            Assert.Equal(users[2], UserThree);

            _contextFactory.CleanUp(context);
        }
    }
}
