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
        Task<bool> ChangeDeck(Deck deck);
        Task<bool> ChangeUserEmailAsync(User user, string email);
        Task<bool> ChangeUserNameAsync(User user, string username);
        Task ChangeActivityAsync(DailyActivity activity);
    }
}
