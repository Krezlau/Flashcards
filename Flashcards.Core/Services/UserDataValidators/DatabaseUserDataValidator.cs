using Flashcards.Core.DBConnection;
using Flashcards.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards.Core.Services.UserDataValidators
{
    public class DatabaseUserDataValidator : IUserDataValidator
    {
        private readonly UserDbContextFactory _contextFactory;

        public DatabaseUserDataValidator(UserDbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<bool> ValidateDeckName(string name, int userId)
        {
            using (UsersContext context = _contextFactory.CreateDbContext())
            {
                Deck deck = await context.Decks.
                    Where(d => d.Name == name && d.UserId == userId).
                    FirstOrDefaultAsync();

                return deck is null;
            }
        }

        public async Task<bool> ValidateEmail(string email)
        {
            using (UsersContext context = _contextFactory.CreateDbContext())
            {
                User user = await context.Users
                    .Where(u => u.Email == email)
                    .FirstOrDefaultAsync();

                return user is null;
            }
        }

        public async Task<bool> ValidateUsername(string username)
        {
            using (UsersContext context = _contextFactory.CreateDbContext())
            {
                User user = await context.Users
                    .Where(u => u.Name == username)
                    .FirstOrDefaultAsync();

                return user is null;
            }
        }
    }
}
