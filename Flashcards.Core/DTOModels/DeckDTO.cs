using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards.Core.DTOModels
{
    public class DeckDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<FlashcardDTO> Flashcards { get; set; }
    }
}
