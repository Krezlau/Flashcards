using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Flashcards.Core.DTOModels
{
    public class UserDTO
    {
        [Key]
        [Required]
        [MaxLength(50)]
        [MinLength(5)]
        public string Name { get; set; }
        public List<DeckDTO> Decks { get; set; }
    }
}
