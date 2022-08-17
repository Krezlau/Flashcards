using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards.Core.Services
{
    public static class UserInputValidator
    {
        public const int DeckNameMaxLength = 25;
        public const int DeckNameMinLength = 3;

        public const int FlashcardTextMinLength= 1;
        public const int FlashcardTextMaxLength = 100;

        public const int UsernameMaxLength = 25;
        public const int UsernameMinLength = 4;

        public const int PasswordMinLength = 8;
        public const int PasswordMaxLength = 25;

        // need to check somehow if email is valid
        // need to validate that characters in password are ascii

        /// <summary>
        /// Checks if inserted deck name is valid
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
        /// Checks if inserted back/front text of a flashcard is valid
        /// </summary>
        /// <param name="text">Back/Front of a flashcard</param>
        /// <returns>
        /// 0 => text valid<br />
        /// 1 => text too short<br />
        /// 2 => text too long<br />
        /// </returns>
        public static int ValidateFlashcardTextField(string text)
        {
            if (text is null || text.Length < FlashcardTextMinLength)
            {
                return 1;
            }
            if (text.Length > FlashcardTextMaxLength)
            {
                return 2;
            }
            return 0;
        }

        /// <summary>
        /// Checks if inserted username is valid
        /// </summary>
        /// <param name="username">inserted username</param>
        /// <returns>
        /// 0 => username valid<br />
        /// 1 => username too short<br />
        /// 2 => username too long<br />
        /// 3 => username consists of white space characters <br />
        /// </returns>
        public static int ValidateUsername(string username)
        {
            if (username is null || username.Length < UsernameMinLength) return 1;
            if (username.Length > UsernameMaxLength) return 2;
            if (username.Trim() != username) return 3;
            return 0;
        }

        /// <summary>
        /// Checks if inserted password is valid
        /// </summary>
        /// <param name="password">inserted password</param>
        /// <returns>
        /// 0 => password valid<br />
        /// 1 => password too short<br />
        /// 2 => password too long<br />
        /// 3 => password consists of not allowed character<br />
        /// </returns>
        public static int ValidatePassword(string password)
        {
            if (password is null || password.Length < PasswordMinLength) return 1;
            if (password.Length > PasswordMaxLength) return 2;
            if (IfContainsUnicodeCharacter(password)) return 3;
            return 0;
        }

        private static bool IfContainsUnicodeCharacter(string input)
        {
            return input.Any(c => c > 255);
        }

        public static bool IsValidEmail(string email)
        {
            if (email is null) return false;
            return new EmailAddressAttribute().IsValid(email);
        }
    }
}
