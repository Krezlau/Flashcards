using Flashcards.Core.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Flashcards.Core.Tests.Models
{
    public class DeckTests
    {
        
        public static IEnumerable<object[]> CollectToReviewData()
        {
            Deck deckOne = new Deck("deck1", 1);
            Deck deckTwo = new Deck("deck2", 1);
            Deck deckThree = new Deck("deck3", 1);
            Deck deckFour = new Deck("deck4", 1);

            var flist = new ObservableCollection<Flashcard>();
            for (int i = 0; i < 10; i++)
            {
                flist.Add(new Flashcard()
                {
                    NextReview = DateTime.Today.AddDays(i - 3),
                    Id = i + 1
                });
            }

            deckOne.Flashcards = flist;

            foreach (Flashcard f in flist.Where(f => f.Id % 2 == 0))
            {
                deckTwo.Flashcards.Add(f);
            }

            foreach (Flashcard f in flist.Where(f => f.Id > 5))
            {
                deckThree.Flashcards.Add(f);
            }

            foreach (Flashcard f in flist.Where(f => f.Id == 1))
            {
                deckFour.Flashcards.Add(f);
            }

            return new List<object[]>()
            {
                new object[] {deckOne, new List<Flashcard>(flist.Where(f => f.Id < 5))},
                new object[] {deckTwo, new List<Flashcard>(flist.Where(f => f.Id < 5 && f.Id % 2 == 0)) },
                new object[] {deckThree, new List<Flashcard>()},
                new object[] {deckFour, new List<Flashcard>(flist.Where(f => f.Id == 1))}
            };
        }

        [Fact]
        public void CollectToReview_WhenFlashcardsNullOrEmpty_ShouldReturnEmptyList()
        {
            Deck deckFlashcardsNull = new Deck()
            {
                Activity = new List<DeckActivity>(),
                Id = 1,
                Name = "deck",
                UserId = 1,
                Flashcards = null
            };

            Deck deckFlashcardsEmpty = new Deck()
            {
                Activity = new List<DeckActivity>(),
                Id = 1,
                Name = "deck",
                UserId = 1,
                Flashcards = new ObservableCollection<Flashcard>()
            };

            Assert.Equal(new List<Flashcard>(), deckFlashcardsNull.CollectToReview());
            Assert.Equal(new List<Flashcard>(), deckFlashcardsEmpty.CollectToReview());
        }

        [Theory]
        [MemberData(nameof(CollectToReviewData))]
        public void CollectToReview_WhenCorrectListOfFlashcards_ShouldCorrectlyCollectThem(Deck deck, List<Flashcard> expected)
        {
            Assert.Equal(expected, deck.CollectToReview());
        }

        [Fact]
        public void EqualsTest()
        {
            Deck primaryDeck = new Deck
            {
                Activity = new List<DeckActivity>(),
                Id = 1,
                Name = "deck",
                UserId = 1,
                Flashcards = new ObservableCollection<Flashcard>()
            };

            Deck deckNull = null;
            User notADeck = new User { Id = 1 };
            Deck deckWithDifferentId = new Deck() { Id = 10 };
            Deck deckWithTheSameId = new Deck() { Id = 1, Name = "different" };

            Assert.False(primaryDeck.Equals(deckNull));
            Assert.False(primaryDeck.Equals(notADeck));
            Assert.False(primaryDeck.Equals(deckWithDifferentId));
            Assert.True(primaryDeck.Equals(deckWithTheSameId));
            Assert.True(primaryDeck.Equals(primaryDeck));
        }

        [Fact]
        public void ConstructorTest()
        {
            Deck deck = new Deck("deck", 1);

            Assert.Equal("deck", deck.Name);
            Assert.Equal(1, deck.UserId);
            Assert.NotNull(deck.Flashcards);
            Assert.NotNull(deck.Activity);
            Assert.Empty(deck.Flashcards);
            Assert.Empty(deck.Activity);
        }

    }
}
