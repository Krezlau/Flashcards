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
        public UsersContext CreateDbContext()
        {
            DbContextOptions options = new DbContextOptionsBuilder().UseInMemoryDatabase("test").Options;

            return new UsersContext(options);
        }
    }
}
