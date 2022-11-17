using Flashcards.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Flashcards.Core.Tests.Models
{
    public class FlashcardTests
    {
        [Fact]
        public void EqualsTest()
        {
            Flashcard primaryFlashcard = new Flashcard()
            {
                Back = "back",
                Front = "front",
                Id = 1,
                Level = 0,
                DeckId = 1,
                NextReview = DateTime.Parse("2022-12-12")
            };

            Flashcard flashcardNull = null;
            Deck notAFlashcard = new Deck("deck", 1);
            Flashcard flashcardWithDifferentId = new Flashcard { Id = 10 };
            Flashcard flashcardWithTheSameId = new Flashcard { Id = 1 };

            Assert.False(primaryFlashcard.Equals(flashcardNull));
            Assert.False(primaryFlashcard.Equals(notAFlashcard));
            Assert.False(primaryFlashcard.Equals(flashcardWithDifferentId));
            Assert.True(primaryFlashcard.Equals(flashcardWithTheSameId));
            Assert.True(primaryFlashcard.Equals(primaryFlashcard));
        } 
    }
}
