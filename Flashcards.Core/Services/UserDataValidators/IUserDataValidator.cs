using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards.Core.Services.UserDataValidators
{
    public interface IUserDataValidator
    {
        Task<bool> ValidateUsername(string username);
        Task<bool> ValidateEmail(string email);
        Task<bool> ValidateDeckName(string name, int userId);
    }
}
