using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards.Core.Models
{
    public class DailyActivity
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public DateTime Day { get; set; }

        [Required]
        public int ReviewedFlashcardsCount { get; set; }

        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }

        public virtual User User { get; set; }
    }
}
