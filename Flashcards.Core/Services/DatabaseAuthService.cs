using Flashcards.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards.Core.Services
{
    public class DatabaseAuthService : IAuthenticationService
    {
        public Task<bool> CreateAccountAsync(string username, string email, string password)
        {
            throw new NotImplementedException();
        }

        public Task<User> LoginUserAsync(string username, string password)
        {
            throw new NotImplementedException();
        }
    }
}
