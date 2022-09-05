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
        private readonly DatabaseUserDataChanger _validDataChanger;
        private readonly DatabaseUserDataChanger _invalidDataChanger;
        private readonly TestDbContextFactory _dbContextFactory;
        public DatabaseUserDataChangerTests()
        {
            var dataValidatorMockTrue = new Mock<IUserDataValidator>();
            dataValidatorMockTrue.Setup(v => v.ValidateDeckName(It.IsAny<string>(), It.IsAny<int>())).Returns(Task.FromResult(true));
            dataValidatorMockTrue.Setup(v => v.ValidateEmail(It.IsAny<string>())).Returns(Task.FromResult(true));
            dataValidatorMockTrue.Setup(v => v.ValidateUsername(It.IsAny<string>())).Returns(Task.FromResult(true));

            var dataValidatorMockFalse = new Mock<IUserDataValidator>();
            dataValidatorMockFalse.Setup(v => v.ValidateDeckName(It.IsAny<string>(), It.IsAny<int>())).Returns(Task.FromResult(false));
            dataValidatorMockFalse.Setup(v => v.ValidateEmail(It.IsAny<string>())).Returns(Task.FromResult(false));
            dataValidatorMockFalse.Setup(v => v.ValidateUsername(It.IsAny<string>())).Returns(Task.FromResult(false));

            _dbContextFactory = new TestDbContextFactory();
            _validDataChanger = new DatabaseUserDataChanger(_dbContextFactory, dataValidatorMockTrue.Object);
            _invalidDataChanger = new DatabaseUserDataChanger(_dbContextFactory, dataValidatorMockFalse.Object);
        }

        internal void CleanUp()
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

            await _validDataChanger.ChangeActivityAsync(dailyActivity);

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

            await _validDataChanger.ChangeDeck(deck, "changed deck");

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

            await _validDataChanger.ChangeFlashcard(flashcard);

            // single in order to make sure there aren't any dublicates
            var newFlashcard = context.Flashcards.Single();

            Assert.Equal("new front", newFlashcard.Front);
            Assert.Equal("new back", newFlashcard.Back);
            Assert.Equal(DateTime.Parse("2021-02-02"), newFlashcard.NextReview);
            Assert.Equal(1, newFlashcard.Level);

            CleanUp();
        }

        [Fact]
        public async void ChangeUserEmailAsyncTest()
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
            context.SaveChanges();

            string newEmail = "newemail@email";

            await _validDataChanger.ChangeUserEmailAsync(user, newEmail);

            // single in order to make sure there aren't any dublicates
            var newUser = context.Users.Single();
            Assert.Equal(newEmail, newUser.Email);

            CleanUp();
        }

        [Fact]
        public async void ChangeUserNameAsyncTest()
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
            context.SaveChanges();

            string newUsername = "newusername";

            await _validDataChanger.ChangeUserNameAsync(user, newUsername);

            // single in order to make sure there aren't any dublicates
            var newUser = context.Users.Single();
            Assert.Equal(newUsername, newUser.Name);

            CleanUp();
        }

        [Fact]
        public async void ValidationTest()
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

            string newName = "changed";
            string newEmail = "changed@email";

            bool deckChange = await _invalidDataChanger.ChangeDeck(deck, newName);
            bool emailChange = await _invalidDataChanger.ChangeUserEmailAsync(user, newEmail);
            bool usernameChange = await _invalidDataChanger.ChangeUserNameAsync(user, newName);

            Assert.False(deckChange);
            Assert.False(emailChange);
            Assert.False(usernameChange);

            var newDeck = context.Decks.Single();
            var newUser = context.Users.Single();

            Assert.Equal("test", newUser.Name);
            Assert.Equal("test@test", newUser.Email);
            Assert.Equal("deck", newDeck.Name);

            CleanUp();
        }
    }
}
