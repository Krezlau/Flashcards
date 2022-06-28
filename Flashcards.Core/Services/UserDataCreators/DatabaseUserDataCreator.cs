using Flashcards.Core.DTOModels;
using Flashcards.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards.Core.Services.UserDataCreators
{
    public class DatabaseUserDataCreator : IUserDataCreator
    {
        private readonly UserDbContextFactory _dbContextFactory;

        public DatabaseUserDataCreator(UserDbContextFactory dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public async Task SaveNewDeck(Deck deck, string username)
        {
            using (UsersContext context = _dbContextFactory.CreateDbContext())
            {
                context.Decks.Add(ToDTOModelConverter.ToDeckDTO(deck, username));
                await context.SaveChangesAsync();
            }
        }

        public async Task SaveNewFlashcard(Flashcard flashcard, Deck deck, string username)
        {
            using (UsersContext context = _dbContextFactory.CreateDbContext())
            {
                context.Flashcards.Add(ToDTOModelConverter.ToFlashcardDTO(flashcard, deck, username));
                await context.SaveChangesAsync();
            }
        }
    }
}
