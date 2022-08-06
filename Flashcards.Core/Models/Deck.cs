using Microsoft.Toolkit.Mvvm.ComponentModel;
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
    public class Deck : ObservableObject
    {
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }
        public ObservableCollection<Flashcard> Flashcards { get; set; }
        [ForeignKey("User")]
        [Required]
        public string UserName { get; set; }
        public int Size => Flashcards.Count;

        public Deck(string name, string username)
        {
            UserName = username;
            Name = name;
            Flashcards = new ObservableCollection<Flashcard>();
        }

        public Deck() { }

        private string _name;

        public List<Flashcard> CollectToReview()
        {
            List<Flashcard> toReview = new();
            foreach (Flashcard f in Flashcards)
            {
                if (f.NextReview <= DateTime.Today)
                {
                    toReview.Add(f);
                }
            }
            return toReview;
        }
    }
}
