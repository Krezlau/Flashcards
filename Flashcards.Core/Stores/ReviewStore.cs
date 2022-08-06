using Flashcards.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards.Core.Stores
{
    public class ReviewStore
    {
        public List<Flashcard> ToReviewList { get; set; }

        public int Iterator { get; set; }

        private Deck _selectedDeck;
        public Deck SelectedDeck
        {
            get => _selectedDeck;
            set
            {
                _selectedDeck = value;
                ToReviewList = value.CollectToReview();
                Iterator = 0;
            }
        }

        public void EndOfLearning()
        {
            ToReviewList = SelectedDeck.CollectToReview();
            Iterator = 0;
        }

        public void Again()
        {
            ToReviewList.Add(ToReviewList[Iterator]);
            Iterator++;
        }

        public void Good()
        {
            Iterator++;
        }
    }
}
