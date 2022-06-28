using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards.Core.DTOModels
{
    public class FlashcardDTO
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
        public DeckDTO Deck { get; set; }
    }
}
