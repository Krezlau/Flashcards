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
    public class UserIconViewModel : ObservableObject
    {
        private readonly UserDecksStore _userDecksStore;
        private readonly IDialogService _dialogService;

        private readonly NavigationService<LogInViewModel> _logInNavService;
        private readonly NavigationService<AccountManagementViewModel> _accountManagementService;

        public string Username => _userDecksStore.User.Name;

        private string _streak;
        public string Streak
        {
            get => _streak;
            set => SetProperty(ref _streak, value);
        }

        private string _imageSource;
        public string ImageSource
        {
            get => _imageSource;
            set => SetProperty(ref _imageSource, value);
        }

        public ICommand ManageCommand { get; set; }
        public ICommand LogOutCommand { get; set; }

        public UserIconViewModel(UserDecksStore userDecksStore, NavigationService<LogInViewModel> logInNavService, NavigationService<AccountManagementViewModel> accountManagementService, IDialogService dialogService)
        {
            _userDecksStore = userDecksStore;
            _logInNavService = logInNavService;

            ManageCommand = new RelayCommand(OnManageClick);
            LogOutCommand = new RelayCommand(OnLogOutClick);
            _accountManagementService = accountManagementService;

            _userDecksStore.StreakChangedEvent += OnStreakChanged;

            // this isn't really correct 
            // will need to change through something like asset manager
            ImageSource = "\\Assets\\fire_gray.png";
            if (_userDecksStore.IfTodayActivity)
            {
                ImageSource = "\\Assets\\fire.png";
            }

            Streak = $"{_userDecksStore.Streak}\nDAYS";
            _dialogService = dialogService;
        }

        private void OnStreakChanged()
        {
            ImageSource = "\\Assets\\fire.png";
            Streak = $"{_userDecksStore.Streak}\nDAYS";
        }

        private void OnLogOutClick()
        {
            _dialogService.ShowSnackbarMessage("Logged out", "Successfully logged out.");
            _logInNavService.Navigate();
            _userDecksStore.LogOutUser();

        }

        private void OnManageClick()
        {
            _accountManagementService.Navigate();
        }
    }
}
