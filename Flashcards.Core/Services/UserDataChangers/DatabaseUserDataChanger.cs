using Flashcards.Core.DBConnection;
using Flashcards.Core.Models;
using Flashcards.Core.Services.UserDataValidators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards.Core.Services.UserDataChangers
{
    public class DatabaseUserDataChanger : IUserDataChanger
    {
        private readonly IUserDbContextFactory _dbContextFactory;
        private readonly IUserDataValidator _dataValidator;

        public DatabaseUserDataChanger(IUserDbContextFactory dbContextFactory, IUserDataValidator dataValidator)
        {
            _dbContextFactory = dbContextFactory;
            _dataValidator = dataValidator;
        }

        public async Task ChangeActivityAsync(DailyActivity activity)
        {
            using (UsersContext context = _dbContextFactory.CreateDbContext())
            {
                context.DailyActivity.Update(activity);
                await context.SaveChangesAsync();
            }
        }

        public async Task<bool> ChangeDeck(Deck deck, string newName)
        {
            using (UsersContext context = _dbContextFactory.CreateDbContext())
            {
                bool isValid = await _dataValidator.ValidateDeckName(newName, deck.UserId);

                if (!isValid) return false;

                deck.Name = newName;
                context.Decks.Update(deck);
                await context.SaveChangesAsync();
                return true;
            }
        }

        public async Task ChangeFlashcard(Flashcard flashcard)
        {
            using (UsersContext context = _dbContextFactory.CreateDbContext())
            {
                context.Flashcards.Update(flashcard);
                await context.SaveChangesAsync();
            }
        }

        public async Task<bool> ChangeUserEmailAsync(User user, string email)
        {
            using (UsersContext context = _dbContextFactory.CreateDbContext())
            {
                bool isEmailValid = await _dataValidator.ValidateEmail(email);

                if (!isEmailValid) return false;

                user.Email = email.ToLower();
                context.Users.Update(user);
                await context.SaveChangesAsync();
                return true;
            }
        }

        public async Task<bool> ChangeUserNameAsync(User user, string username)
        {
            using (UsersContext context = _dbContextFactory.CreateDbContext())
            {
                bool isUsernameValid = await _dataValidator.ValidateUsername(username);

                if (!isUsernameValid) return false;

                user.Name = username;
                context.Users.Update(user);
                await context.SaveChangesAsync();
                return true;
            }
        }

        public async Task ChangeDeckActivityAsync(DeckActivity activity)
        {
            using (UsersContext context = _dbContextFactory.CreateDbContext())
            {
                context.DeckActivity.Update(activity);
                await context.SaveChangesAsync();
            }
        }
    }
}
