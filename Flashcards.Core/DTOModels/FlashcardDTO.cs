using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards.Core.DTOModels
{
    public class FlashcardDTO
    {
        public int Id { get; set; }
        public string Back { get; set; }
        public string Front { get; set; }
        public int Level { get; set; }
        public DateTime NextReview { get; set; }
    }
}
