using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards.Core.Models
{
    public class Deck : ObservableObject
    {
        public List<Flashcard> FlashcardList { get; set; }

        public int Size => FlashcardList.Count();

        private string name;
        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        public Deck(string _name, List<Flashcard> _flashcardList)
        {
            Name = _name;
            FlashcardList = _flashcardList;
        }

        public Deck(string _name)
        {
            Name = _name;
            FlashcardList = new List<Flashcard>();
        }
    }
}
