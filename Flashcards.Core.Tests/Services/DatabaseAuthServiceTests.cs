using Flashcards.Core.DBConnection;
using Flashcards.Core.Models;
using Flashcards.Core.Services;
using Flashcards.Core.Tests.DbConnection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Flashcards.Core.Tests.Services
{
    public class DatabaseAuthServiceTests
    {

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

        [Theory]
        [InlineData("user", "email@test")]
        [InlineData("user", "test@email")]
        [InlineData("notuser", "test@email")]
        public async void CreateAccountAsyncTest_WithEmailOrUsernameAlreadyTaken_ShouldReturnFalse(string username, string email)
        {
            var _contextFactory = new TestDbContextFactory(nameof(CreateAccountAsyncTest_WithEmailOrUsernameAlreadyTaken_ShouldReturnFalse));
            var _authService = new DatabaseAuthService(_contextFactory);
            var context = _contextFactory.CreateDbContext();
            var UserOne = CreateSampleUser(context);

            Assert.False(await _authService.CreateAccountAsync(username, email, "hash"));

            //checking if there is only one record
            var userTwo = context.Users.Single();

            _contextFactory.CleanUp(context);
        }

        [Theory]
        [InlineData("notuser", "email@test")]
        public async void CreateAccountAsyncTest_WithFreeEmailAndUsername_ShouldReturnTrue(string username, string email)
        {
            var _contextFactory = new TestDbContextFactory(nameof(CreateAccountAsyncTest_WithFreeEmailAndUsername_ShouldReturnTrue));
            var _authService = new DatabaseAuthService(_contextFactory);
            var context = _contextFactory.CreateDbContext();
            var UserOne = CreateSampleUser(context);

            Assert.True(await _authService.CreateAccountAsync(username, email, "hash"));

            //checking if there is only one record
            Assert.Throws<InvalidOperationException>(() => context.Users.Single());

            _contextFactory.CleanUp(context);
        }

        [Theory]
        [InlineData("UsEr", "useruser")]
        [InlineData("notuser", "useruser")]
        [InlineData("user", "USERUSER")]
        [InlineData("user", "password")]
        [InlineData("notuser", "password")]
        public async void LoginUserAsyncTest_WithIncorrectPasswordOrNonExistingUser_ShouldReturnNull(string username, string password)
        {
            var _contextFactory = new TestDbContextFactory(nameof(LoginUserAsyncTest_WithIncorrectPasswordOrNonExistingUser_ShouldReturnNull));
            var _authService = new DatabaseAuthService(_contextFactory);
            var context = _contextFactory.CreateDbContext();
            var user = CreateSampleUser(context);

            var loggedUser = await _authService.LoginUserAsync(username, password);

            Assert.Null(loggedUser);

            _contextFactory.CleanUp(context);
        }

        [Theory]
        [InlineData("user", "useruser")]
        public async void LoginUserAsyncTest_WithCorrectPasswordAndUsername_ShouldReturnUser(string username, string password)
        {
            var _contextFactory = new TestDbContextFactory(nameof(LoginUserAsyncTest_WithCorrectPasswordAndUsername_ShouldReturnUser));
            var _authService = new DatabaseAuthService(_contextFactory);
            var context = _contextFactory.CreateDbContext();
            var user = CreateSampleUser(context);

            var loggedUser = await _authService.LoginUserAsync(username, password);

            Assert.Equal(user, loggedUser);

            _contextFactory.CleanUp(context);
        }

        [Theory]
        [InlineData("USER", "lmaolmao", "password")]
        [InlineData("incorrectUser", "useruser", "password")]
        public async void ChangeUserPasswordAsyncTest_WithIncorrectPasswordOrUsername_ShouldReturnFalse(string username, string oldpassword, string newpassword)
        {
            var _contextFactory = new TestDbContextFactory(nameof(ChangeUserPasswordAsyncTest_WithIncorrectPasswordOrUsername_ShouldReturnFalse));
            var _authService = new DatabaseAuthService(_contextFactory);
            var context = _contextFactory.CreateDbContext();
            var user = CreateSampleUser(context);

            var outcome = await _authService.ChangeUserPasswordAsync(newpassword, oldpassword, username);

            Assert.False(outcome);

            // making sure there is only one user in db and password remains unchanged
            var userInDb = context.Users.Single();
            Assert.Equal(user, userInDb);

            _contextFactory.CleanUp(context);
        }

        [Theory]
        [InlineData("user", "useruser", "password")]
        public async void ChangeUserPasswordAsyncTest_WithCorrectData_ShouldReturnTrue(string username, string oldpassword, string newpassword)
        {
            var _contextFactory = new TestDbContextFactory(nameof(ChangeUserPasswordAsyncTest_WithCorrectData_ShouldReturnTrue));
            var _authService = new DatabaseAuthService(_contextFactory);
            using (var context = _contextFactory.CreateDbContext())
            {
                var user = CreateSampleUser(context);
            }

            var outcome = await _authService.ChangeUserPasswordAsync(newpassword, oldpassword, username);

            User loggedUser;
            using (var context = _contextFactory.CreateDbContext())
            {
                loggedUser = context.Users.Single();

                // cant understand why this is failing
                Assert.True(outcome);
                Assert.True(PasswordHasher.Verify(newpassword, loggedUser.PasswordHash));
                Assert.False(PasswordHasher.Verify(oldpassword, loggedUser.PasswordHash));

                _contextFactory.CleanUp(context);
            }
        }

        [Theory]
        [InlineData("user", "USERUSER")]
        [InlineData("user", "password")]
        [InlineData("UsEr", "useruser")]
        public async void IfPasswordCorrectTest_WithIncorrectPasswordOrUsername_ShouldReturnFalse(string username, string password)
        {
            var _contextFactory = new TestDbContextFactory(nameof(IfPasswordCorrectTest_WithIncorrectPasswordOrUsername_ShouldReturnFalse));
            var _authService = new DatabaseAuthService(_contextFactory);
            var context = _contextFactory.CreateDbContext();
            var user = CreateSampleUser(context);

            var outcome = await _authService.IfPasswordCorrect(password, username);

            Assert.False(outcome);

            _contextFactory.CleanUp(context);
        }

        [Theory]
        [InlineData("user", "useruser")]
        public async void IfPasswordCorrectTest_WithCorrectPasswordAndUsername_ShouldReturnTrue(string username, string password)
        {
            var _contextFactory = new TestDbContextFactory(nameof(IfPasswordCorrectTest_WithCorrectPasswordAndUsername_ShouldReturnTrue));
            var _authService = new DatabaseAuthService(_contextFactory);
            var context = _contextFactory.CreateDbContext();
            var user = CreateSampleUser(context);

            var outcome = await _authService.IfPasswordCorrect(password, username);

            Assert.True(outcome);

            _contextFactory.CleanUp(context);
        }
    }
}
