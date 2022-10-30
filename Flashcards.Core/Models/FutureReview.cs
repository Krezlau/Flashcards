using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards.Core.Models
{
    public class FutureReview
    {
        public int Count { get; set; }

        public DateTime Day { get; set; }

        public FutureReview(DateTime day, int count)
        {
            Day = day;
            Count = count;
        }
    }
}
