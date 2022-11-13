using Flashcards.Core.DBConnection;
using Flashcards.Core.Models;
using Flashcards.Core.Services;
using Flashcards.Core.Services.UserDataCreators;
using Flashcards.Core.Services.UserDataValidators;
using Flashcards.Core.Tests.DbConnection;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Flashcards.Core.Tests.Services.UserDataCreators
{
    public class DatabaseUserDataCreatorTests
    {
        private readonly Mock<IUserDataValidator> _dataValidatorMockTrue;
        private readonly Mock<IUserDataValidator> _dataValidatorMockFalse;
        public DatabaseUserDataCreatorTests()
        {
            _dataValidatorMockTrue = new Mock<IUserDataValidator>();
            _dataValidatorMockTrue.Setup(v => v.ValidateDeckName(It.IsAny<string>(), It.IsAny<int>())).Returns(Task.FromResult(true));
            _dataValidatorMockTrue.Setup(v => v.ValidateEmail(It.IsAny<string>())).Returns(Task.FromResult(true));
            _dataValidatorMockTrue.Setup(v => v.ValidateUsername(It.IsAny<string>())).Returns(Task.FromResult(true));

            _dataValidatorMockFalse = new Mock<IUserDataValidator>();
            _dataValidatorMockFalse.Setup(v => v.ValidateDeckName(It.IsAny<string>(), It.IsAny<int>())).Returns(Task.FromResult(false));
            _dataValidatorMockFalse.Setup(v => v.ValidateEmail(It.IsAny<string>())).Returns(Task.FromResult(false));
            _dataValidatorMockFalse.Setup(v => v.ValidateUsername(It.IsAny<string>())).Returns(Task.FromResult(false));
        }

        private User CreateSampleUser(UsersContext context)
        {
            var user = new User()
            {
                Name = "user",
                Email = "test@email",
                PasswordHash = PasswordHasher.Hash("useruser")
            };
            context.Users.Add(user);
            context.SaveChanges();
            return user;
        }

        [Fact]
        public async void SaveNewDailyActivityTest()
        {
            var _contextFactory = new TestDbContextFactory(nameof(SaveNewDailyActivityTest));
            var _dataCreator = new DatabaseUserDataCreator(_contextFactory, _dataValidatorMockTrue.Object);

            var context = _contextFactory.CreateDbContext();
            var user = CreateSampleUser(context);
            var da = new DailyActivity()
            {
                Day = DateTime.Parse("2022-12-31"),
                ReviewedFlashcardsCount = 1,
                UserId = user.Id
            };
            await _dataCreator.SaveNewDailyActivity(da);

            var dbRecord = context.DailyActivity.Single();
            Assert.Equal(da, dbRecord);
            Assert.Equal(da.Day, dbRecord.Day);
            Assert.Equal(da.ReviewedFlashcardsCount, dbRecord.ReviewedFlashcardsCount);
            Assert.Equal(da.UserId, dbRecord.UserId);

            _contextFactory.CleanUp(context);
        }

        [Fact]
        public async void SaveNewDeckTest()
        {
            var _contextFactory = new TestDbContextFactory(nameof(SaveNewDeckTest));
            var _dataCreator = new DatabaseUserDataCreator(_contextFactory, _dataValidatorMockTrue.Object);

            var context = _contextFactory.CreateDbContext();
            var user = CreateSampleUser(context);
            var deck = new Deck()
            {
                Name = "deck",
                UserId = user.Id
            };
            bool outcome = await _dataCreator.SaveNewDeck(deck);

            Assert.True(outcome);

            var dbRecord = context.Decks.Single();

            Assert.Equal(deck, dbRecord);
            Assert.Equal(deck.Name, dbRecord.Name);
            Assert.Equal(deck.UserId, dbRecord.UserId);

            _contextFactory.CleanUp(context);
        }

        [Fact]
        public async void SaveNewFlashcardTest()
        {
            var _contextFactory = new TestDbContextFactory(nameof(SaveNewFlashcardTest));
            var _dataCreator = new DatabaseUserDataCreator(_contextFactory, _dataValidatorMockTrue.Object);

            var context = _contextFactory.CreateDbContext();
            var user = CreateSampleUser(context);
            var deck = new Deck()
            {
                Name = "deck",
                UserId = user.Id
            };
            await _dataCreator.SaveNewDeck(deck);

            var flashcard = new Flashcard()
            {
                Back = "back",
                Front = "front",
                DeckId = deck.Id,
                Level = 0,
                NextReview = DateTime.Parse("2022-01-01")
            };

            await _dataCreator.SaveNewFlashcard(flashcard);

            var dbRecord = context.Flashcards.Single();

            Assert.Equal(flashcard, dbRecord);
            Assert.Equal(flashcard.Front, dbRecord.Front);
            Assert.Equal(flashcard.Back, dbRecord.Back);
            Assert.Equal(flashcard.DeckId, dbRecord.DeckId);
            Assert.Equal(flashcard.Level, dbRecord.Level);
            Assert.Equal(flashcard.NextReview, dbRecord.NextReview);

            _contextFactory.CleanUp(context);
        }

        [Fact]
        public async void ValidationOutcomeTest()
        {
            var _contextFactory = new TestDbContextFactory(nameof(ValidationOutcomeTest));
            var _dataCreator = new DatabaseUserDataCreator(_contextFactory, _dataValidatorMockFalse.Object);

            var context = _contextFactory.CreateDbContext();
            var user = CreateSampleUser(context);
            var deck = new Deck()
            {
                Name = "deck",
                UserId = user.Id
            };
            bool outcome = await _dataCreator.SaveNewDeck(deck);

            Assert.False(outcome);

            _contextFactory.CleanUp(context);
        }

        [Fact]
        public async void SaveNewDeckActivityAsyncTest()
        {
            var _contextFactory = new TestDbContextFactory(nameof(SaveNewDeckActivityAsyncTest));
            var _dataCreator = new DatabaseUserDataCreator(_contextFactory, _dataValidatorMockTrue.Object);

            var context = _contextFactory.CreateDbContext();
            var user = CreateSampleUser(context);
            var da = new DeckActivity()
            {
                Day = DateTime.Parse("2022-12-31"),
                ReviewedFlashcardsCount = 1,
                DeckId = 1,
                MinutesSpentLearning = 1.2
            };
            await _dataCreator.SaveNewDeckActivityAsync(da);

            var dbRecord = context.DeckActivity.Single();
            Assert.Equal(da, dbRecord);
            Assert.Equal(da.Day, dbRecord.Day);
            Assert.Equal(da.ReviewedFlashcardsCount, dbRecord.ReviewedFlashcardsCount);
            Assert.Equal(da.DeckId, dbRecord.DeckId);
            Assert.Equal(da.MinutesSpentLearning, dbRecord.MinutesSpentLearning);

            _contextFactory.CleanUp(context);
        }
    }
}
