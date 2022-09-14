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
        private readonly IUserDbContextFactory _dbContextFactory;

        public DatabaseUserDataDestroyer(IUserDbContextFactory dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }
        public async Task DeleteDeck(Deck deck)
        {
            using (UsersContext context = _dbContextFactory.CreateDbContext())
            {
                context.Flashcards.RemoveRange(context.Flashcards.Where(f => f.DeckId == deck.Id));
                context.Decks.Remove(context.Decks.First(a => a.Id == deck.Id));
                await context.SaveChangesAsync();
            }
        }

        public async Task DeleteFlashcard(Flashcard flashcard)
        {
            using (UsersContext context = _dbContextFactory.CreateDbContext())
            {
                context.Flashcards.Remove(flashcard);
                await context.SaveChangesAsync();
            }
        }
    }
}
