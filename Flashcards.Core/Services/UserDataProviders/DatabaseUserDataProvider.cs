using Flashcards.Core.DTOModels;
using Flashcards.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public async Task<User> GetUserDecks(string _username)
        {
            using (UsersContext context = _dbContextFactory.CreateDbContext())
            {
                List<UserDTO> userDTOs = await context.Users
                    .Where(b => b.Name == _username)
                    .Include(d => d.Decks)
                    .ToListAsync();

                UserDTO userDTO;
                try
                {
                    userDTO = userDTOs.First();
                } catch(Exception e)
                {
                    return new User(_username);
                }

                return ToUser(userDTO);
            }
        }

        public User LoadUserDecks(string _username)
        {
            using (UsersContext context = _dbContextFactory.CreateDbContext())
            {
                List<UserDTO> userDTOs = context.Users
                    .Where(b => b.Name == _username)
                    .Include(d => d.Decks)
                    .ToList();
                    //.AsAsyncEnumerable();

                UserDTO userDTO;
                try
                {
                    userDTO = userDTOs.First();
                }
                catch (Exception e)
                {
                    return new User(_username);
                }

                return ToUser(userDTO);
            }
        }

        private User ToUser(UserDTO r)
        {
            if (r.Decks != null)
            {
                r.Decks.ToArray();
                List<Deck> dlist = new List<Deck>();
                foreach (DeckDTO d in r.Decks)
                {
                    dlist.Add(new Deck(d.Name));
                }
                return new User(r.Name, new ObservableCollection<Deck>(dlist));
            }
            return new User(r.Name, new ObservableCollection<Deck>());
        }

    }
}
