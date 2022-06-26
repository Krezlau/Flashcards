using System.Collections.Generic;

namespace Flashcards.Core.DTOModels
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public List<DeckDTO> Decks { get; set; }
    }
}
