using Flashcards.Core.DBConnection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards.Core.Models
{
    public class User
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        [MinLength(5)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; }

        [Required]
        [Column(TypeName = "VARCHAR")]
        [MaxLength(128)]
        public string PasswordHash { get; set; }
        public ObservableCollection<Deck> Decks { get; set; }

        public List<DailyActivity> Activity { get; set; }

        public int CalculateStreak(DateTime date)
        {
            int sum = 0;

            if (Activity.Count == 0) return 0;

            if (Activity[Activity.Count - 1].Day.Day == date.Day)
            {
                sum += 1;
            }

            while (sum < Activity.Count && Activity[Activity.Count - sum - 1].Day.Day == date.AddDays(-1).Day)
            {
                sum++;
                date = date.AddDays(-1);
            }

            return sum;
        }

        public bool IfLearnedToday(DateTime date)
        {
            if (Activity.Count == 0) return false;
            if (Activity[Activity.Count-1].Day.Day == date.Day) return true;
            return false;
        }
    }
}
