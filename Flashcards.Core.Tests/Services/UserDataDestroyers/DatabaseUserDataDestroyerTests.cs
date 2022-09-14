using Flashcards.Core.Models;
using Flashcards.Core.Services.UserDataDestroyers;
using Flashcards.Core.Tests.DbConnection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Flashcards.Core.Tests.Services.UserDataDestroyers
{
    public class DatabaseUserDataDestroyerTests
    {
        [Fact]
        public async void DeleteTests()
        {
            var _contextFactory = new TestDbContextFactory(nameof(DeleteTests));
            var _dataDestroyer = new DatabaseUserDataDestroyer(_contextFactory);
            var context = _contextFactory.CreateDbContext();

            User user = new User()
            {
                Email = "test@test",
                Name = "user",
                PasswordHash = "hash"
            };
            context.Users.Add(user);
            context.SaveChanges();

            Deck deck = new Deck()
            {
                Name = "deck",
                UserId = user.Id
            };
            context.Decks.Add(deck);
            context.SaveChanges();

            Flashcard flashcard = new Flashcard()
            {
                Front = "test",
                Back = "test",
                DeckId = deck.Id,
                Level = 0,
                NextReview = DateTime.Parse("2022-03-23")
            };
            context.Flashcards.Add(flashcard);
            context.SaveChanges();

            await _dataDestroyer.DeleteFlashcard(flashcard);

            List<Flashcard> flashcardsInDataBaseAfterFlashcardDeletion = context.Flashcards.ToList();

            Flashcard flashcard2 = new Flashcard()
            {
                Front = "test",
                Back = "test",
                DeckId = deck.Id,
                Level = 0,
                NextReview = DateTime.Parse("2022-03-23")
            };
            context.Flashcards.Add(flashcard2);
            context.SaveChanges();

            await _dataDestroyer.DeleteDeck(deck);

            List<Flashcard> flashcardsInDataBaseAfterDeckDeletion = context.Flashcards.ToList();
            List<Deck> decksInDataBaseAfterDeckDeletion = context.Decks.ToList();

            Assert.Empty(decksInDataBaseAfterDeckDeletion);
            Assert.Empty(flashcardsInDataBaseAfterDeckDeletion);
            Assert.Empty(flashcardsInDataBaseAfterFlashcardDeletion);

            _contextFactory.CleanUp(context);
        }
    }
}
