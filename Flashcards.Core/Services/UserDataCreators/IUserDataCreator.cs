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
        Task<bool> SaveNewDeck(Deck deck);
        Task SaveNewFlashcard(Flashcard flashcard);
        Task SaveNewDailyActivity(DailyActivity activity);
    }
}
