using Flashcards.Core.Models;
using Flashcards.Core.Stores;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Flashcards.Core.Tests.Stores
{
    public class ReviewStoreTests
    {
        private Deck PrepareDeck()
        {
            List<Flashcard> flist = new List<Flashcard>(10);
            for (int i = 0; i < 10; i++)
            {
                flist.Add(new Flashcard()
                {
                    Id = i,
                    Back = $"back{i}",
                    Front = $"front{i}",
                    DeckId = 1,
                    Level = 0,
                    NextReview = DateTime.Today.AddDays(i % 2)
                });
            }

            return new Deck()
            {
                Flashcards = new ObservableCollection<Flashcard>(flist),
                Id = 1,
                Name = "deck",
                UserId = 0
            };
        }

        [Fact]
        public void SelectedDeckSetterTest()
        {
            var store = new ReviewStore();
            store.Iterator = 10;
            var deck = PrepareDeck();
            store.SelectedDeck = deck;
            Assert.Equal(0, store.Iterator);
            Assert.Equal(deck, store.SelectedDeck);
            Assert.Equal(5, store.ToReviewList.Count);
            foreach (Flashcard f in store.ToReviewList)
            {
                Assert.Equal(DateTime.Today, f.NextReview);
            }
        }

        [Fact]
        public void EndOfLearningTest()
        {
            var store = new ReviewStore();
            store.Iterator = 10;
            var deck = PrepareDeck();
            store.SelectedDeck = deck;
            deck.Flashcards.RemoveAt(9);
            deck.Flashcards.RemoveAt(8);
            deck.Flashcards.RemoveAt(7);
            deck.Flashcards.RemoveAt(6);
            store.EndOfLearning();
            Assert.Equal(0, store.Iterator);
            Assert.Equal(deck, store.SelectedDeck);
            Assert.Equal(3, store.ToReviewList.Count);
        }

        [Fact]
        public void AgainTest()
        {
            var store = new ReviewStore();
            store.Iterator = 10;
            var deck = PrepareDeck();
            store.SelectedDeck = deck;
            store.Again();
            Assert.Equal(6, store.ToReviewList.Count);
            Assert.Equal(deck.Flashcards[0], store.ToReviewList[^1]);
            Assert.Equal(1, store.Iterator);
        }

        [Fact]
        public void GoodTest()
        {
            var store = new ReviewStore();
            store.Good();
            Assert.Equal(1, store.Iterator);
        }
    }
}
