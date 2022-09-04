using Flashcards.Core.DBConnection;
using Flashcards.Core.Models;
using Flashcards.Core.Services.UserDataChangers;
using Flashcards.Core.Services.UserDataValidators;
using Flashcards.Core.Tests.DbConnection;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Flashcards.Core.Tests.Services.UserDataChangers
{
    public class DatabaseUserDataChangerTests
    {
        private readonly DatabaseUserDataChanger _dataChanger;
        private readonly TestDbContextFactory _dbContextFactory;
        public DatabaseUserDataChangerTests()
        {
            var dataValidatorMock = new Mock<IUserDataValidator>();
            dataValidatorMock.Setup(v => v.ValidateDeckName(It.IsAny<string>(), It.IsAny<int>())).Returns(Task.FromResult(true));
            dataValidatorMock.Setup(v => v.ValidateEmail(It.IsAny<string>())).Returns(Task.FromResult(true));
            dataValidatorMock.Setup(v => v.ValidateUsername(It.IsAny<string>())).Returns(Task.FromResult(true));

            _dbContextFactory = new TestDbContextFactory();
            _dataChanger = new DatabaseUserDataChanger(_dbContextFactory, dataValidatorMock.Object);
        }

        public void CleanUp()
        {
            var context = _dbContextFactory.CreateDbContext();
            context.Flashcards.RemoveRange(context.Flashcards.ToList());
            context.Decks.RemoveRange(context.Decks.ToList());
            context.DailyActivity.RemoveRange(context.DailyActivity.ToList());
            context.Users.RemoveRange(context.Users.ToList());

            context.SaveChanges();
        }

        [Fact]
        public async void ChangeActivityAsyncTest()
        {
            var context = _dbContextFactory.CreateDbContext();
            var user = new User
            {
                Id = 1,
                Name = "test",
                Email = "test@test",
                PasswordHash = "testtesttest"
            };
            context.Users.Add(user);

            var dailyActivity = new DailyActivity
            {
                Day = DateTime.Parse("2022-07-30"),
                ReviewedFlashcardsCount = 1,
                User = user,
                UserId = user.Id
            };

            context.DailyActivity.Add(dailyActivity);
            context.SaveChanges();

            dailyActivity.Day = DateTime.Parse("2022-06-30");
            dailyActivity.ReviewedFlashcardsCount = 30;

            await _dataChanger.ChangeActivityAsync(dailyActivity);

            // single in order to make sure there aren't any dublicates
            var newActivity = context.DailyActivity.Single();

            Assert.Equal(DateTime.Parse("2022-06-30"), newActivity.Day);
            Assert.Equal(30, newActivity.ReviewedFlashcardsCount);

            CleanUp();
        }

        [Fact]
        public async void ChangeDeckTest()
        {
            var context = _dbContextFactory.CreateDbContext();
            var user = new User
            {
                Id = 1,
                Name = "test",
                Email = "test@test",
                PasswordHash = "testtesttest"
            };
            context.Users.Add(user);

            var deck = new Deck
            {
                Name = "deck",
                UserId = user.Id
            };

            context.Decks.Add(deck);
            context.SaveChanges();

            deck.Name = "changed deck";

            await _dataChanger.ChangeDeck(deck);

            // single in order to make sure there aren't any dublicates
            var newDeck = context.Decks.Single();

            Assert.Equal("changed deck", newDeck.Name);

            CleanUp();
        }

        [Fact]
        public async void ChangeFlashcardTest()
        {
            var context = _dbContextFactory.CreateDbContext();
            var user = new User
            {
                Id = 1,
                Name = "test",
                Email = "test@test",
                PasswordHash = "testtesttest"
            };
            context.Users.Add(user);

            var deck = new Deck
            {
                Name = "deck",
                UserId = user.Id
            };
            context.Decks.Add(deck);

            var flashcard = new Flashcard
            {
                Back = "back",
                Front = "front",
                DeckId = deck.Id,
                Level = 0,
                NextReview = DateTime.Parse("2022-01-01")
            };
            context.Flashcards.Add(flashcard);

            context.SaveChanges();

            flashcard.Front = "new front";
            flashcard.Back = "new back";
            flashcard.NextReview = DateTime.Parse("2021-02-02");
            flashcard.Level = 1;

            await _dataChanger.ChangeFlashcard(flashcard);

            // single in order to make sure there aren't any dublicates
            var newFlashcard = context.Flashcards.Single();

            Assert.Equal("new front", newFlashcard.Front);
            Assert.Equal("new back", newFlashcard.Back);
            Assert.Equal(DateTime.Parse("2021-02-02"), newFlashcard.NextReview);
            Assert.Equal(1, newFlashcard.Level);

            CleanUp();
        }
    }
}
