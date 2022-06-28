using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards.Core.DTOModels
{
    public class DeckDTO
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        public List<FlashcardDTO> Flashcards { get; set; }
        [ForeignKey("User")]
        [Required]
        public string UserName { get; set; }
        public UserDTO User { get; set; }
    }
}
