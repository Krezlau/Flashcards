using Flashcards.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards.Core.Services.UserDataProviders
{
    public interface IUserDataProvider
    {
        Task<User> GetUserDecks(string _username);
        User LoadUserDecks(string _username);
    }
}
