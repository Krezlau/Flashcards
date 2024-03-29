﻿using Flashcards.Core.DBConnection;
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
        private readonly IUserDbContextFactory _dbContextFactory;

        public DatabaseUserDataProvider(IUserDbContextFactory dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public async Task<User> LoadUserDecksAsync(int userId)
        {
            using (UsersContext context = _dbContextFactory.CreateDbContext())
            {
                User user = context.Users
                    .Where(b => b.Id == userId)
                    .First();

                using (var activityContext = _dbContextFactory.CreateDbContext())
                {
                    var activity = activityContext.DailyActivity
                        .Where(b => b.UserId == userId)
                        .OrderBy(a => a.Day)
                        .ToListAsync();

                    var decks = context.Decks
                        .Where(b => b.UserId == userId)
                        .ToListAsync();


                    user.Activity = new List<DailyActivity>(await activity);

                    user.Decks = new ObservableCollection<Deck>(await decks);
                }

                foreach (Deck d in user.Decks)
                {
                    d.Flashcards = new ObservableCollection<Flashcard>(await context.Flashcards
                        .Where(b => b.DeckId == d.Id)
                        .ToListAsync());

                    d.Activity = new List<DeckActivity>(await context.DeckActivity
                        .Where(b => b.DeckId == d.Id)
                        .OrderBy(b => b.Day)
                        .ToListAsync());
                }

                return user;
            }
        }

        public async Task<List<DailyActivity>> LoadUserActivityAsync(int userId)
        {
            using (UsersContext context = _dbContextFactory.CreateDbContext())
            {
                return await context.DailyActivity.Where(da => da.UserId == userId).ToListAsync();
            }
        }
    }
}
