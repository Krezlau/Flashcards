using Flashcards.Core.DBConnection;
using Flashcards.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards.Core.Services.UserDataDestroyers
{
    public class DatabaseUserDataDestroyer : IUserDataDestroyer
    {
        private readonly UserDbContextFactory _dbContextFactory;

        public DatabaseUserDataDestroyer(UserDbContextFactory dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }
        public async Task DeleteDeck(Deck deck)
        {
            using (UsersContext context = _dbContextFactory.CreateDbContext())
            {
                context.Decks.Remove(context.Decks.Single(a => a.Id == deck.Id));
                await context.SaveChangesAsync();
            }
        }

        public async Task DeleteFlashcard(Flashcard flashcard)
        {
            using (UsersContext context = _dbContextFactory.CreateDbContext())
            {
                context.Flashcards.Remove(context.Flashcards.Single(a => a.Id == flashcard.Id));
                await context.SaveChangesAsync();
            }
        }
    }
}
