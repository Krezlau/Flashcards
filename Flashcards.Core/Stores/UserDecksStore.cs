using Flashcards.Core.Models;
using Flashcards.Core.Services;
using Flashcards.Core.Services.UserDataChangers;
using Flashcards.Core.Services.UserDataCreators;
using Flashcards.Core.Services.UserDataDestroyers;
using Flashcards.Core.Services.UserDataProviders;
using Flashcards.Core.ViewModels;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards.Core.Stores
{
    public class UserDecksStore : ObservableObject
    {
        private readonly IUserDataProvider _dataProvider;
        private readonly IUserDataCreator _dataCreator;
        private readonly IUserDataDestroyer _dataDestroyer;
        private readonly IUserDataChanger _dataChanger;

        private readonly NavigationService _navigationStore;

        private readonly NavigationService<HomeViewModel> _navigationService;
        private readonly NavigationService<UserIconViewModel> _rightNavService;

        private User _user = new User() { Decks = new System.Collections.ObjectModel.ObservableCollection<Deck>() };
        public User User
        {
            get => _user;
            set => SetProperty(ref _user, value);
        }

        public bool IfTodayActivity { get; set; }

        private int _streak;
        public int Streak
        {
            get => _streak;
            set
            {
                _streak = value;
                StreakChangedEvent?.Invoke();
            }
        }

        public event Action EmailChangeRequest;

        public event Action StreakChangedEvent;

        public SelectionStore SelectionStore { get; }

        public UserDecksStore(IUserDataProvider dataProvider, IUserDataCreator dataCreator, IUserDataDestroyer dataDestroyer, IUserDataChanger dataChanger, SelectionStore selectionStore, NavigationService navigationStore, NavigationService<UserIconViewModel> rightNavService, NavigationService<HomeViewModel> navigationService)
        {
            _dataProvider = dataProvider;
            _dataCreator = dataCreator;
            _dataDestroyer = dataDestroyer;
            _dataChanger = dataChanger;
            SelectionStore = selectionStore;
            _navigationStore = navigationStore;
            _rightNavService = rightNavService;
            _navigationService = navigationService;
        }

        public void Initialize(User user)
        {
            User = _dataProvider.LoadUserDecks(user.Id);
            IfTodayActivity = User.IfLearnedToday(DateTime.Today);
            Streak = User.CalculateStreak(DateTime.Today);
            _navigationService.NavigateLeft();
            _rightNavService.NavigateRight();
        }

        public void EmailChangeRequestInvoke()
        {
            EmailChangeRequest?.Invoke();
        }

        public async Task<bool> ChangeUserEmail(string email)
        {
            bool outcome = await _dataChanger.ChangeUserEmailAsync(User, email);
            if (outcome == false) return false;
            User.Email = email;
            return true;
        }

        public async Task<bool> ChangeUserName(string username)
        {
            bool outcome = await _dataChanger.ChangeUserNameAsync(User, username);
            if (outcome == false) return false;
            User.Name = username;
            return true;
        }

        public void LogOutUser()
        {
            User = null;
            SelectionStore.SelectedDeck = null;
            SelectionStore.SelectedFlashcard = null;
            _navigationStore.LeftViewModel = null;
            _navigationStore.RightViewModel = null;
        }

        public async Task AlterFlashcard(string front, string back)
        {
            int deckIndex = SelectionStore.GetSelectedDeckIndex(User);
            int flashcardIndex = SelectionStore.GetSelectedFlashcardIndex();
            User.Decks[deckIndex].Flashcards[flashcardIndex].Front = front;
            User.Decks[deckIndex].Flashcards[flashcardIndex].Back = back;
            await _dataChanger.ChangeFlashcard(User.Decks[deckIndex].Flashcards[flashcardIndex]);
        }

        public async Task<bool> AlterDeck(string name)
        {
            int deckIndex = SelectionStore.GetSelectedDeckIndex(User);
            string oldName = User.Decks[deckIndex].Name;
            User.Decks[deckIndex].Name = name;
            bool outcome = await _dataChanger.ChangeDeck(User.Decks[deckIndex]);
            if (outcome == false)
            {
                User.Decks[deckIndex].Name = oldName;
                return false;
            }
            return true;
        }

        public async Task<bool> AddNewDeck(Deck deck)
        {
            bool outcome = await _dataCreator.SaveNewDeck(deck);
            if (outcome == false) return false;
            User.Decks.Add(deck);
            return true;
        }

        public async Task RemoveCurrentFlashcard()
        {
            int deckIndex = SelectionStore.GetSelectedDeckIndex(User);
            int flashcardIndex = SelectionStore.GetSelectedFlashcardIndex();
            await _dataDestroyer.DeleteFlashcard(SelectionStore.SelectedFlashcard);
            User.Decks[deckIndex].Flashcards.Remove(SelectionStore.SelectedFlashcard);
        }

        public async Task RemoveCurrentDeck()
        {
            await _dataDestroyer.DeleteDeck(SelectionStore.SelectedDeck);
            User.Decks.Remove(SelectionStore.SelectedDeck);
        }

        public async Task AddFlashcardToSelectedDeck(Flashcard flashcard)
        {
            User.Decks[SelectionStore.GetSelectedDeckIndex(User)].Flashcards.Add(flashcard);
            await _dataCreator.SaveNewFlashcard(flashcard);
        }

        public async Task FlashcardSetReview(Flashcard flashcard)
        {
            // probably should be called with list of flashcards to change instead of being called on every flashcard
            if (IfTodayActivity)
            {
                User.Activity[User.Activity.Count - 1].ReviewedFlashcardsCount++;
                await _dataChanger.ChangeActivityAsync(User.Activity[User.Activity.Count - 1]);
            }
            if (!IfTodayActivity)
            {
                IfTodayActivity = true;
                User.Activity.Add(new DailyActivity
                {
                    Day = DateTime.Today,
                    UserId = User.Id,
                    ReviewedFlashcardsCount = 1,
                });
                Streak++;
                await _dataCreator.SaveNewDailyActivity(User.Activity[User.Activity.Count - 1]);
            }
            flashcard.Level += 1;
            flashcard.NextReview = DateTime.Today.AddDays(flashcard.Level);
            await _dataChanger.ChangeFlashcard(flashcard);
        }

        public async Task FlashcardSetReviewFailed(Flashcard flashcard)
        {
            flashcard.Level = 0;
            await _dataChanger.ChangeFlashcard(flashcard);
        }
    }
}
