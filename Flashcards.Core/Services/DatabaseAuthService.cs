using Flashcards.Core.DBConnection;
using Flashcards.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards.Core.Services
{
    public class DatabaseAuthService : IAuthenticationService
    {
        private readonly UserDbContextFactory _dbContextFactory;

        public DatabaseAuthService(UserDbContextFactory dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public async Task<bool> CreateAccountAsync(string username, string email, string password)
        {
            using (UsersContext context = _dbContextFactory.CreateDbContext())
            {
                if (context.Users.Where(a => a.Name == username).FirstOrDefault() is not null
                    || context.Users.Where(a => a.Email == email).FirstOrDefault() is not null)
                {
                    return false;
                }
                User user = new User
                {
                    Decks = new ObservableCollection<Deck>(),
                    Email = email,
                    Name = username,
                    PasswordHash = PasswordHasher.Hash(password)
                };

                context.Users.Add(user);
                await context.SaveChangesAsync();
                return true;
            }
        }

        public async Task<User> LoginUserAsync(string username, string password)
        {
            using (UsersContext context = _dbContextFactory.CreateDbContext())
            {
                User user = await context.Users.Where(a => a.Name == username).FirstOrDefaultAsync();

                if (user is not null && PasswordHasher.Verify(password, user.PasswordHash))
                {
                    return user;
                }
                return null;
            }
        }

        public async Task<bool> ChangeUserPasswordAsync(string newPassword, string oldPassword, string username)
        {
            using (UsersContext context = _dbContextFactory.CreateDbContext())
            {
                User dbUser = await LoginUserAsync(username, oldPassword);

                if (dbUser is null) return false;

                dbUser.PasswordHash = PasswordHasher.Hash(newPassword);

                context.Users.Update(dbUser);
                await context.SaveChangesAsync();
                return true;

            }
        }

        public async Task<bool> IfPasswordCorrect(string password, string username)
        {
            using (UsersContext context = _dbContextFactory.CreateDbContext())
            {
                User dbUser = await LoginUserAsync(username, password);

                if (dbUser is null) return false;

                return true;
            }
        }
    }
}
