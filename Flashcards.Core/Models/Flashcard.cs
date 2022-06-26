using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards.Core.Models
{
    public class Flashcard
    {
        public string Front { get; set; }
        public string Back { get; set; }
        public int Level { get; set; }
        public DateTime NextReview { get; set; }

        public Flashcard(string front, string back)
        {
            Front = front;
            Back = back;
            Level = 0;
            NextReview = DateTime.Today;
        }
    }
}
