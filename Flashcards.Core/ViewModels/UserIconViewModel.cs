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

        private readonly NavigationService<LogInViewModel> _logInNavService;
        private readonly NavigationService<AccountManagementViewModel> _accountManagementService;

        public string Username => _userDecksStore.User.Name;

        private string _streak;
        public string Streak
        {
            get => _streak;
            set => SetProperty(ref _streak, value);
        }
        public ICommand ManageCommand { get; set; }
        public ICommand LogOutCommand { get; set; }

        public UserIconViewModel(UserDecksStore userDecksStore, NavigationService<LogInViewModel> logInNavService, NavigationService<AccountManagementViewModel> accountManagementService)
        {
            _userDecksStore = userDecksStore;
            _logInNavService = logInNavService;

            ManageCommand = new RelayCommand(OnManageClick);
            LogOutCommand = new RelayCommand(OnLogOutClick);
            _accountManagementService = accountManagementService;

            Streak = $"{_userDecksStore.Streak}\nDAYS";
        }

        private void OnLogOutClick()
        {
            _logInNavService.Navigate();
            _userDecksStore.LogOutUser();

        }

        private void OnManageClick()
        {
            _accountManagementService.Navigate();
        }
    }
}
