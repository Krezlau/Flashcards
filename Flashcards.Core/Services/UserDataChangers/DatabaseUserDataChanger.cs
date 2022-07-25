using Flashcards.Core.DBConnection;
using Flashcards.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards.Core.Services.UserDataChangers
{
    public class DatabaseUserDataChanger : IUserDataChanger
    {
        private readonly UserDbContextFactory _dbContextFactory;

        public DatabaseUserDataChanger(UserDbContextFactory dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }
        public async Task ChangeDeck(Deck deck)
        {
            using (UsersContext context = _dbContextFactory.CreateDbContext())
            {
                context.Decks.Update(deck);
                await context.SaveChangesAsync();
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
    }
}
