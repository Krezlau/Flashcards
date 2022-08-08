using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards.Core.Services
{
    public static class UserInputValidator
    {
        public const int DeckNameMaxLength = 25;
        public const int DeckNameMinLength = 3;
        public const int FlashcardTextMinLenght = 1;
        public const int FlashcardTextMaxLength = 100;

        /// <summary>
        /// For checking if inserted deck name is valid
        /// </summary>
        /// <param name="name">Name of the deck</param>
        /// <returns>
        /// <para>0 => name valid</para>
        /// <para>1 => name too short</para>
        /// <para>2 => name too long</para>
        /// </returns>
        public static int ValidateDeckName(string name)
        {
            if (name is null || name.Length < DeckNameMinLength)
            {
                return 1;
            }
            if (name.Length > DeckNameMaxLength)
            {
                return 2;
            }
            return 0;
        }

        /// <summary>
        /// For checking if inserted back/front text of a flashcard is valid
        /// </summary>
        /// <param name="text">Back/Front of a flashcard</param>
        /// <returns>
        /// 0 => text valid<br />
        /// 1 => text too short<br />
        /// 2 => text too long<br />
        /// </returns>
        public static int ValidateFlashcardTextField(string text)
        {
            if (text is null || text.Length < FlashcardTextMinLenght)
            {
                return 1;
            }
            if (text.Length > FlashcardTextMaxLength)
            {
                return 2;
            }
            return 0;
        }
    }
}
