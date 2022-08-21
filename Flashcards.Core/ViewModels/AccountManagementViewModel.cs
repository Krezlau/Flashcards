using Flashcards.Core.Models;
using Flashcards.Core.Services;
using Flashcards.Core.Stores;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Flashcards.Core.ViewModels
{
    public class AccountManagementViewModel : ObservableObject
    {
        private readonly UserDecksStore _userDecksStore;
        private readonly NavigationService<AccountInfoChangeViewModel> _navigationService;
        private readonly NavigationService<PasswordChangeViewModel> _passwordChangeService;
        private readonly NavigationService<ActivityChartsViewModel> _chartsNavService;

        public string Username => _userDecksStore.User.Name;

        public string Email { get; set; }

        public string UsernameText { get; set; }

        public string StreakTileText { get; set; }

        public string DeckTileText1 { get; set; }

        public string DeckTileText2 { get; set; }

        public string ActivityTileText { get; set; }

        public ICommand ChangeUsernameCommand { get; set; }

        public ICommand ChangeEmailCommand { get; set; }

        public ICommand ChangePasswordCommand { get; set; }

        public ICommand ActivityChartsCommand { get; set; }

        public AccountManagementViewModel(UserDecksStore userDecksStore, NavigationService<AccountInfoChangeViewModel> navigationService, NavigationService<PasswordChangeViewModel> passwordChangeService, NavigationService<ActivityChartsViewModel> chartsNavService)
        {
            _userDecksStore = userDecksStore;
            _navigationService = navigationService;

            Deck largestDeck = FindLargestDeck();
            int flashcardCount = CountFlashcards();

            StreakTileText = $"You have been learning for {_userDecksStore.Streak} days in a row. \n Keep going!"; //todo
            if (largestDeck is not null)
            {
                DeckTileText2 = "The biggest deck is " + largestDeck.Name + " - with " + largestDeck.Size + " flashcards inside.";
            }
            DeckTileText1 = "There are " + _userDecksStore.User.Decks.Count + " decks on your account.";
            ActivityTileText = "We store data regarding your activity. Check your activity data here.";

            UsernameText = "Username: " + _userDecksStore.User.Name;
            Email = "Email: " + _userDecksStore.User.Email;

            ChangeUsernameCommand = new RelayCommand(OnChangeUsernameClick);
            ChangeEmailCommand = new RelayCommand(OnChangeEmailClick);
            ChangePasswordCommand = new RelayCommand(OnChangePasswordClick);
            ActivityChartsCommand = new RelayCommand(OnActivityClick);
            _passwordChangeService = passwordChangeService;
            _chartsNavService = chartsNavService;
        }

        private void OnActivityClick()
        {
            _chartsNavService.Navigate();
        }

        private void OnChangeUsernameClick()
        {
            _navigationService.Navigate();
        }

        private void OnChangeEmailClick()
        {
            _navigationService.Navigate();
            _userDecksStore.EmailChangeRequestInvoke();
        }

        private void OnChangePasswordClick()
        {
            _passwordChangeService.Navigate();
        }

        private Deck FindLargestDeck()
        {
            int max = -1;
            Deck maxDeck = null;
            foreach (Deck d in _userDecksStore.User.Decks)
            {
                if (d.Flashcards.Count > max)
                {
                    max = d.Flashcards.Count;
                    maxDeck = d;
                }
            }
            return maxDeck;
        }

        private int CountFlashcards()
        {
            int sum = 0;
            foreach (Deck d in _userDecksStore.User.Decks)
            {
                sum += d.Flashcards.Count;
            }
            return sum;
        }

    }
}
