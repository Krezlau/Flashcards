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
        public DateTime StartTime { get; private set; }

        public bool IfSessionActive { get; set; } = false;

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
        /// <summary>
        /// end a session
        /// </summary>
        /// <returns>Number of minutes spent learning</returns>
        public double EndOfLearning()
        {
            ToReviewList = SelectedDeck.CollectToReview();
            Iterator = 0;
            IfSessionActive = false;
            var timespan_minutes = (DateTime.Now - StartTime).TotalMinutes;
            return timespan_minutes;
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

        public void StartSession()
        {
            StartTime = DateTime.Now;
            IfSessionActive = true;
        }
    }
}
