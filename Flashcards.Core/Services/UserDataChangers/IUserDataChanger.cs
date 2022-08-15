using Flashcards.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards.Core.Services.UserDataChangers
{
    public interface IUserDataChanger
    {
        Task ChangeFlashcard(Flashcard flashcard);
        Task ChangeDeck(Deck deck);
        Task ChangeUserAsync(User user);
    }
}
