using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards.Core.DBConnection
{
    public interface IUserDbContextFactory
    {
        UsersContext CreateDbContext();
    }
}
