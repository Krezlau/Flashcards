using Flashcards.Core.DBConnection;
using Flashcards.Core.Services;
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
            var daysRegisteredList = new List<DateTime>();

            if (Activity is not null && Activity.Count != 0)
            {
                daysRegisteredList.Add(Activity[0].Day);

                for (int j = 1; j < Activity.Count; j++)
                {
                    daysRegisteredList.Add(Activity[j].Day);
                }
            }

            if (Decks is null) Decks = new ObservableCollection<Deck>();
            foreach (Deck deck in Decks)
            {
                if (deck.Activity is null || deck.Activity.Count == 0) continue;
                foreach (DeckActivity activity in deck.Activity)
                {
                    int index = daysRegisteredList.BinarySearch(activity.Day);
                    if (index < 0)
                    {
                        daysRegisteredList.Insert(~index, activity.Day);
                    }
                }
            }

            if (daysRegisteredList.Count == 0) return 0;

            int i = daysRegisteredList.Count - 1;
            int sum = 0;
            daysRegisteredList.OrderBy(d => d);
            while (i >= 0 && date == daysRegisteredList[i--].Date)
            {
                date = date.AddDays(-1);
                sum++;
            }

            return sum;
        }

        public bool IfLearnedToday(DateTime date)
        {
            if (this.Activity is not null && 
                this.Activity.Count > 0 &&
                this.Activity[^1].Day == date) { return true; }

            if (Decks is null) Decks = new ObservableCollection<Deck>();
            foreach (Deck d in Decks)
            {
                if (d.Activity is not null &&
                d.Activity.Count > 0 &&
                d.Activity[^1].Day == date) { return true; }
            }
            return false;
        }

        public override bool Equals(object obj)
        {
            if (obj is null || !this.GetType().Equals(obj.GetType())) return false;

            User u = (User)obj;
            return Id == u.Id;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }
    }
}
