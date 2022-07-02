using Flashcards.Core.DBConnection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards.Core.Models
{
    public class User
    {
        [Key]
        [Required]
        [MaxLength(50)]
        [MinLength(5)]
        public string Name { get; set; }
        public ObservableCollection<Deck> Decks { get; set; }
    }
}
