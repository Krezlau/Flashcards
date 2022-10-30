using Flashcards.Core.DBConnection;
using Flashcards.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards.Core.Services.UserDataDestroyers
{
    public class DatabaseUserDataDestroyer : IUserDataDestroyer
    {
        private readonly IUserDbContextFactory _dbContextFactory;

        public DatabaseUserDataDestroyer(IUserDbContextFactory dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }
        public async Task DeleteDeck(Deck deck)
        {
            using (UsersContext context = _dbContextFactory.CreateDbContext())
            {
                context.Flashcards.RemoveRange(context.Flashcards.Where(f => f.DeckId == deck.Id));

                //migrating deckactivity to dailyactivity
                var activity = await context.DeckActivity.Where(da => da.DeckId == deck.Id).ToListAsync();
                var dailyActivity = new List<DailyActivity>(activity.Count);
                foreach (var a in activity)
                {
                    dailyActivity.Add(a.ToDailyActivity());
                }
                foreach (var a in dailyActivity)
                {
                    var inDb = context.DailyActivity.Where(da => da.Day == a.Day).First();
                    if (inDb is not null)
                    {
                        inDb.ReviewedFlashcardsCount += a.ReviewedFlashcardsCount;
                        inDb.MinutesSpentLearning += a.MinutesSpentLearning;
                        context.DailyActivity.Update(inDb);
                        continue;
                    }
                    if (inDb is null)
                    {
                        context.DailyActivity.Add(a);
                        continue;
                    }
                }
                await context.SaveChangesAsync();
                foreach (var a in activity)
                {
                    await DeleteDeckActivityAsync(a);
                }

                context.Decks.Remove(context.Decks.First(a => a.Id == deck.Id));
                await context.SaveChangesAsync();
            }
        }

        public async Task DeleteFlashcard(Flashcard flashcard)
        {
            using (UsersContext context = _dbContextFactory.CreateDbContext())
            {
                context.Flashcards.Remove(flashcard);
                await context.SaveChangesAsync();
            }
        }
        
        public async Task DeleteDeckActivityAsync(DeckActivity activity)
        {
            using (UsersContext context = _dbContextFactory.CreateDbContext())
            {
                context.DeckActivity.Remove(activity);
                await context.SaveChangesAsync();
            }
        }
    }
}
