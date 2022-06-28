using Flashcards.Core.DTOModels;
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
        public async Task DeleteDeck(Deck deck, string username)
        {
            using (UsersContext context = _dbContextFactory.CreateDbContext())
            {
                context.Decks.Remove(context.Decks.Single(a => a.UserName == username && a.Name == deck.Name));
                await context.SaveChangesAsync();
            }
        }

        public async Task DeleteFlashcard(Flashcard flashcard, Deck deck, string username)
        {
            using (UsersContext context = _dbContextFactory.CreateDbContext())
            {
                context.Flashcards.Remove(ToDTOModelConverter.ToFlashcardDTO(flashcard, deck, username));
                await context.SaveChangesAsync();
            }
        }
    }
}
