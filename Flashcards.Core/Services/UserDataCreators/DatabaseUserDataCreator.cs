using Flashcards.Core.DBConnection;
using Flashcards.Core.Models;
using Flashcards.Core.Services.UserDataValidators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards.Core.Services.UserDataCreators
{
    public class DatabaseUserDataCreator : IUserDataCreator
    {
        private readonly IUserDbContextFactory _dbContextFactory;
        private readonly IUserDataValidator _dataValidator;

        public DatabaseUserDataCreator(IUserDbContextFactory dbContextFactory, IUserDataValidator dataValidator)
        {
            _dbContextFactory = dbContextFactory;
            _dataValidator = dataValidator;
        }

        public async Task SaveNewDailyActivity(DailyActivity activity)
        {
            using (UsersContext context = _dbContextFactory.CreateDbContext())
            {
                context.DailyActivity.Add(activity);
                await context.SaveChangesAsync();
            }
        }

        public async Task<bool> SaveNewDeck(Deck deck)
        {
            using (UsersContext context = _dbContextFactory.CreateDbContext())
            {
                bool isValid = await _dataValidator.ValidateDeckName(deck.Name, deck.UserId);

                if (!isValid) return false;

                context.Decks.Add(deck);
                await context.SaveChangesAsync();
                return true;
            }
        }

        public async Task SaveNewFlashcard(Flashcard flashcard)
        {
            using (UsersContext context = _dbContextFactory.CreateDbContext())
            {
                context.Flashcards.Add(flashcard);
                await context.SaveChangesAsync();
            }
        }

        public async Task SaveNewDeckActivityAsync(DeckActivity activity)
        {

        }
    }
}
