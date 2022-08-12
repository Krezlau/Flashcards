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
    public class LogInViewModel : ObservableObject
    {
        private readonly IGetPasswordService _passwordService;
        private readonly IAuthenticationService _authService;
        private readonly IDialogService _dialogService;
        private readonly UserDecksStore _userDecksStore;
        private readonly NavigationService<UserWelcomeViewModel> _userWelcomeService;
        private readonly NavigationService<RegistrationViewModel> _registerService;

        public string Username { get; set; }

        public ICommand LogInCommand { get; set; }

        public ICommand RegisterCommand { get; set; }

        public LogInViewModel(IGetPasswordService passwordService, IAuthenticationService authService, UserDecksStore userDecksStore, NavigationService<UserWelcomeViewModel> userWelcomeService, IDialogService dialogService, NavigationService<RegistrationViewModel> registerService)
        {
            _passwordService = passwordService;
            _authService = authService;
            LogInCommand = new RelayCommand(OnLogInClick);
            RegisterCommand = new RelayCommand(OnRegisterClick);
            _userDecksStore = userDecksStore;
            _userWelcomeService = userWelcomeService;
            _dialogService = dialogService;
            _registerService = registerService;
        }

        private void OnRegisterClick()
        {
            _registerService.Navigate();
        }

        private async void OnLogInClick()
        {
            User user = await _authService.LoginUserAsync(Username, _passwordService.GetPassword());
            if (_authService.LoginUserAsync(Username, _passwordService.GetPassword()) is not null)
            {
                _userDecksStore.User = user;
                _userWelcomeService.Navigate();
                return;
            }
            _dialogService.ShowMessageDialog("ERROR", "Failed to log in. Check if your username and password are correct.");
        }
    }
}
