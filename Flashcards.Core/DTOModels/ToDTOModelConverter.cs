using Flashcards.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards.Core.DTOModels
{
    public class ToDTOModelConverter
    {
        public static UserDTO ToUserDTO(User user)
        {
            return new UserDTO()
            {
                Name = user.Username
            };
        }

        public static DeckDTO ToDeckDTO(Deck deck, string username)
        {
            return new DeckDTO()
            {
                Name = deck.Name,
                UserName = username
            };
        }

        public static FlashcardDTO ToFlashcardDTO(Flashcard flashcard, Deck deck, string username)
        {
            return new FlashcardDTO()
            {
                Back = flashcard.Back,
                Front = flashcard.Front,
                Level = flashcard.Level,
                NextReview = flashcard.NextReview,
                Deck = ToDeckDTO(deck, username)
            };
        }
    }
}
