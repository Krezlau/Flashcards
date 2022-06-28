using Flashcards.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards.Core.Services.UserDataCreators
{
    public interface IUserDataCreator
    {
        Task SaveNewDeck(Deck deck, string username);
        Task SaveNewFlashcard(Flashcard flashcard, Deck deck, string username);
    }
}
