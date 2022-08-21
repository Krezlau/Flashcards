using Flashcards.Core.DBConnection;
using Flashcards.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards.Core.Services.UserDataProviders
{
    public class DatabaseUserDataProvider : IUserDataProvider
    {
        private readonly UserDbContextFactory _dbContextFactory;

        public DatabaseUserDataProvider(UserDbContextFactory dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        //public async Task<ObservableUser> GetUserDecks(string _username)
        //{
        //    using (UsersContext context = _dbContextFactory.CreateDbContext())
        //    {
        //        List<UserDTO> userDTOs = await context.Users
        //            .Where(b => b.Name == _username)
        //            .Include(d => d.Decks)
        //            .ToListAsync();

        //        UserDTO userDTO;
        //        try
        //        {
        //            userDTO = userDTOs.First();
        //        } catch(Exception e)
        //        {
        //            return new ObservableUser(_username);
        //        }

        //        return ToUser(userDTO);
        //    }
        //}

        public User LoadUserDecks(int userId)
        {
            using (UsersContext context = _dbContextFactory.CreateDbContext())
            {
                User user = context.Users
                    .Where(b => b.Id == userId)
                    .First();

                if (user == null)
                {
                    return new User();
                }

                user.Activity = new List<DailyActivity>(context.DailyActivity
                    .Where(b => b.UserId == userId)
                    .OrderBy(a => a.Day)
                    .ToList());

                user.Decks = new ObservableCollection<Deck>(context.Decks
                    .Where(b => b.UserId == userId)
                    .ToList());

                foreach (Deck d in user.Decks)
                {
                    d.Flashcards = new ObservableCollection<Flashcard>(context.Flashcards
                        .Where(b => b.DeckId == d.Id)
                        .ToList());
                }

                return user;
            }
        }
    }
}
