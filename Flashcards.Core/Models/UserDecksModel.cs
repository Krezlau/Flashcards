using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards.Core.Models
{
    public class UserDecksModel : ObservableObject
    {
        private ObservableCollection<Deck> deckList;

        public ObservableCollection<Deck> DeckList
        {
            get => deckList;
            set => SetProperty(ref deckList, value);
        }
    }
}
