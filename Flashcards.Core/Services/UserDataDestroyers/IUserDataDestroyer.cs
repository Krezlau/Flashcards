using Flashcards.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards.Core.Services.UserDataDestroyers
{
    public interface IUserDataDestroyer
    {
        Task DeleteDeck(Deck deck);
        Task DeleteFlashcard(Flashcard flashcard);
        Task DeleteDeckActivityAsync(DeckActivity activity);
    }
}
