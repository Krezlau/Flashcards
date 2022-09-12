using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards.Core.Models
{
    public class Flashcard
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Back { get; set; }
        [Required]
        [MaxLength(100)]
        public string Front { get; set; }
        [Required]
        public int Level { get; set; }
        [Required]
        public DateTime NextReview { get; set; }
        [ForeignKey("Deck")]
        [Required]
        public int DeckId { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is null || !this.GetType().Equals(obj.GetType())) return false;

            Flashcard f = (Flashcard)obj;
            return f.Id == Id;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }
    }
}
