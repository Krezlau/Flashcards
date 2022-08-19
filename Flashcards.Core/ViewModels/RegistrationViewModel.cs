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
    public class RegistrationViewModel : ObservableObject
    {
        private readonly IAuthenticationService _authService;
        private readonly IDialogService _dialogService;
        private readonly UserDecksStore _userDecksStore;
        private readonly NavigationService<UserWelcomeViewModel> _userWelcomeService;
        private readonly NavigationService<LogInViewModel> _logInService;

        public string Username { get; set; }
        public string Email { get; set; }

        public string Password { private get; set; }

        public string ConfirmPassword { private get; set; }

        public ICommand RegisterCommand { get; set; }

        public ICommand GoBackCommand { get; set; }

        public RegistrationViewModel(UserDecksStore userDecksStore, NavigationService<UserWelcomeViewModel> userWelcomeService, NavigationService<LogInViewModel> logInService, IAuthenticationService authService, IDialogService dialogService)
        {
            _userDecksStore = userDecksStore;
            _userWelcomeService = userWelcomeService;
            _logInService = logInService;
            RegisterCommand = new RelayCommand(OnRegisterClick);
            GoBackCommand = new RelayCommand(OnGoBackClick);
            _authService = authService;
            _dialogService = dialogService;
        }

        private void OnGoBackClick()
        {
            _logInService.Navigate();
        }

        private async void OnRegisterClick()
        {
            if (UserInputValidator.ValidateUsername(Username) == 0 &&
                UserInputValidator.ValidatePassword(Password) == 0 &&
                ConfirmPassword == Password &&
                UserInputValidator.IsValidEmail(Email))
            {
                bool ifSuccesfulRegistration = await _authService.CreateAccountAsync(Username, Email, Password);
                if (!ifSuccesfulRegistration)
                {
                    _dialogService.ShowMessageDialog("ERROR", "Failed to register. Either username or email is taken.");
                    return;

                }
                User user = await _authService.LoginUserAsync(Username, Password);
                _userDecksStore.Initialize(user);
                _userWelcomeService.Navigate();
                return;
            }

            // ERROR cases
            if (ConfirmPassword != Password)
            {
                _dialogService.ShowMessageDialog("ERROR", "Failed to register. Inserted passwords are different.");
                return;
            }
            if (UserInputValidator.ValidateUsername(Username) == 1)
            {
                _dialogService.ShowMessageDialog("ERROR", "Failed to register. Username is too short - must be at least 4 characters.");
                return;
            }
            if (UserInputValidator.ValidateUsername(Username) == 2)
            {
                _dialogService.ShowMessageDialog("ERROR", "Failed to register. Username is too long - must be no longer than 25 characters.");
                return;
            }
            if (UserInputValidator.ValidateUsername(Username) == 3)
            {
                _dialogService.ShowMessageDialog("ERROR", "Failed to register. Username can't consist of white space characters.");
            }
            if (UserInputValidator.ValidatePassword(Password) == 1)
            {
                _dialogService.ShowMessageDialog("ERROR", "Failed to register. Password is too short - must be at least 8 characters.");
                return;
            }
            if (UserInputValidator.ValidatePassword(Password) == 2)
            {
                _dialogService.ShowMessageDialog("ERROR", "Failed to register. Password is too long - must be no longer than 25 characters.");
                return;
            }
            if (UserInputValidator.ValidatePassword(Password) == 3)
            {
                _dialogService.ShowMessageDialog("ERROR", "Failed to register. Illegal characters in password.");
                return;
            }
            if (!UserInputValidator.IsValidEmail(Email))
            {
                _dialogService.ShowMessageDialog("ERROR", "Failed to register. Email not valid.");
                return;
            }
        }
    }
}
