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
        Task DeleteDeck(Deck deck, string username);
        Task DeleteFlashcard(Flashcard flashcard, Deck deck, string username);
    }
}
