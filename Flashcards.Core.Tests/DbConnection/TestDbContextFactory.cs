using Flashcards.Core.DBConnection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards.Core.Tests.DbConnection
{
    public class TestDbContextFactory : IUserDbContextFactory
    {
        private readonly string _name;

        public TestDbContextFactory(string name)
        {
            _name = name;
        }

        public UsersContext CreateDbContext()
        {
            DbContextOptions options = new DbContextOptionsBuilder().UseInMemoryDatabase($"{_name}_testDB").Options;

            return new UsersContext(options);
        }

        public void CleanUp(UsersContext context)
        {
            context.Flashcards.RemoveRange(context.Flashcards.ToList());
            context.Decks.RemoveRange(context.Decks.ToList());
            context.DailyActivity.RemoveRange(context.DailyActivity.ToList());
            context.Users.RemoveRange(context.Users.ToList());

            context.SaveChanges();
        }
    }
}
