using Flashcards.Core.Models;
using Flashcards.Core.Services.UserDataValidators;
using Flashcards.Core.Tests.DbConnection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Flashcards.Core.Tests.Services.UserDataValidators
{
    public class DatabaseUserDataValidatorTests
    {
        [Theory]
        [InlineData("deck", "deck")]
        public async void ValidateDeckNameTest_WithConflictingName_ShouldReturnFalse(string nameOne, string nameTwo)
        {
            var _contextFactory = new TestDbContextFactory(nameof(ValidateDeckNameTest_WithConflictingName_ShouldReturnFalse));
            var _validator = new DatabaseUserDataValidator(_contextFactory);
            var context = _contextFactory.CreateDbContext();
            var deck = new Deck()
            {
                Name = nameOne,
                UserId = 1
            };
            context.Decks.Add(deck);
            context.SaveChanges();

            var secondDeck = new Deck()
            {
                Name = nameTwo,
                UserId = 1
            };

            Assert.False(await _validator.ValidateDeckName(nameTwo, 1));

            _contextFactory.CleanUp(context);
        }

        [Theory]
        [InlineData("deck", "deck")]
        public async void ValidateDeckNameTest_WithConflictingNamesButDifferentUsers_ShouldReturnTrue(string nameOne, string nameTwo)
        {
            var _contextFactory = new TestDbContextFactory(nameof(ValidateDeckNameTest_WithConflictingNamesButDifferentUsers_ShouldReturnTrue));
            var _validator = new DatabaseUserDataValidator(_contextFactory);
            var context = _contextFactory.CreateDbContext();
            var userOne = new User()
            {
                Name = "user1",
                Email = "email@email",
                PasswordHash = "hash"
            };
            context.Users.Add(userOne);

            var userTwo = new User()
            {
                Name = "user2",
                Email = "email@email",
                PasswordHash = "hash"
            };
            context.Users.Add(userTwo);

            var deck = new Deck()
            {
                Name = nameOne,
                UserId = userOne.Id
            };
            context.Decks.Add(deck);
            context.SaveChanges();

            var secondDeck = new Deck()
            {
                Name = nameTwo,
                UserId = userTwo.Id
            };

            Assert.True(await _validator.ValidateDeckName(secondDeck.Name, secondDeck.UserId));

            _contextFactory.CleanUp(context);
        }

        [Theory]
        [InlineData("deck1", "deck2")]
        public async void ValidateDeckNameTest_WithDifferentNames_ShouldReturnTrue(string nameOne, string nameTwo)
        {
            var _contextFactory = new TestDbContextFactory(nameof(ValidateDeckNameTest_WithDifferentNames_ShouldReturnTrue));
            var _validator = new DatabaseUserDataValidator(_contextFactory);
            var context = _contextFactory.CreateDbContext();
            var deck = new Deck()
            {
                Name = nameOne,
                UserId = 1
            };
            context.Decks.Add(deck);
            context.SaveChanges();

            var secondDeck = new Deck()
            {
                Name = nameTwo,
                UserId = 1
            };

            Assert.True(await _validator.ValidateDeckName(nameTwo, 1));

            _contextFactory.CleanUp(context);
        }

        [Theory]
        [InlineData("test", "test")]
        public async void ValidateUserNameTest_WithConflictingNames_ShouldReturnFalse(string nameOne, string nameTwo)
        {
            var _contextFactory = new TestDbContextFactory(nameof(ValidateUserNameTest_WithConflictingNames_ShouldReturnFalse));
            var _validator = new DatabaseUserDataValidator(_contextFactory);
            var context = _contextFactory.CreateDbContext();
            var userOne = new User()
            {
                Name = nameOne,
                Email = "email",
                PasswordHash = "hash"
            };
            context.Users.Add(userOne);
            context.SaveChanges();

            Assert.False(await _validator.ValidateUsername(nameTwo));

            _contextFactory.CleanUp(context);
        }

        [Theory]
        [InlineData("test1", "test2")]
        public async void ValidateUserNameTest_WithDifferentNames_ShouldReturnTrue(string nameOne, string nameTwo)
        {
            var _contextFactory = new TestDbContextFactory(nameof(ValidateUserNameTest_WithDifferentNames_ShouldReturnTrue));
            var _validator = new DatabaseUserDataValidator(_contextFactory);
            var context = _contextFactory.CreateDbContext();
            var userOne = new User()
            {
                Name = nameOne,
                Email = "email",
                PasswordHash = "hash"
            };
            context.Users.Add(userOne);
            context.SaveChanges();

            Assert.True(await _validator.ValidateUsername(nameTwo));

            _contextFactory.CleanUp(context);
        }

        [Theory]
        [InlineData("email@email", "email@email")]
        public async void ValidateEmailTest_WithConflictingEmails_ShouldReturnFalse(string emailOne, string emailTwo)
        {
            var _contextFactory = new TestDbContextFactory(nameof(ValidateEmailTest_WithConflictingEmails_ShouldReturnFalse));
            var _validator = new DatabaseUserDataValidator(_contextFactory);
            var context = _contextFactory.CreateDbContext();
            var userOne = new User()
            {
                Name = "name",
                Email = emailOne,
                PasswordHash = "hash"
            };
            context.Users.Add(userOne);
            context.SaveChanges();

            Assert.False(await _validator.ValidateEmail(emailTwo));

            _contextFactory.CleanUp(context);
        }

        [Theory]
        [InlineData("email1@email", "email2@email")]
        public async void ValidateEmailTest_WithDifferentEmails_ShouldReturnTrue(string emailOne, string emailTwo)
        {
            var _contextFactory = new TestDbContextFactory(nameof(ValidateEmailTest_WithDifferentEmails_ShouldReturnTrue));
            var _validator = new DatabaseUserDataValidator(_contextFactory);
            var context = _contextFactory.CreateDbContext();
            var userOne = new User()
            {
                Name = "name",
                Email = emailOne,
                PasswordHash = "hash"
            };
            context.Users.Add(userOne);
            context.SaveChanges();

            Assert.True(await _validator.ValidateEmail(emailTwo));

            _contextFactory.CleanUp(context);
        }
    }
}
