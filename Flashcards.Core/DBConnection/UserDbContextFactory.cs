using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards.Core.DBConnection
{
    public class UserDbContextFactory : IUserDbContextFactory
    {
        private readonly string _connectionString;

        public UserDbContextFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public UsersContext CreateDbContext()
        {
            DbContextOptions options = new DbContextOptionsBuilder().UseSqlServer(_connectionString).Options;

            return new UsersContext(options);
        }
    }
}
