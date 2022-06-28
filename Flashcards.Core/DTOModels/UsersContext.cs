using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards.Core.DTOModels
{
    public class UsersContext : DbContext
    {
        public UsersContext(DbContextOptions options) : base(options) { }
        public UsersContext() { }
        public DbSet<UserDTO> Users { get; set; }
        public DbSet<DeckDTO> Decks { get; set; }
        public DbSet<FlashcardDTO> Flashcards { get; set; }
    }
}
