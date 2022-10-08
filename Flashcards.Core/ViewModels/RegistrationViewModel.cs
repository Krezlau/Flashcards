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
        private readonly NavigationService<LogInViewModel> _logInService;

        private string _usernameErrorText;
        public string UsernameErrorText
        {
            get => _usernameErrorText;
            set => SetProperty(ref _usernameErrorText, value);
        }

        private string _emailErrorText;
        public string EmailErrorText
        {
            get => _emailErrorText;
            set => SetProperty(ref _emailErrorText, value);
        }

        private string _passwordErrorText;
        public string PasswordErrorText
        {
            get => _passwordErrorText;
            set => SetProperty(ref _passwordErrorText, value);
        }

        private string _confirmPasswordErrorText;
        public string ConfirmPasswordErrorText
        {
            get => _confirmPasswordErrorText;
            set => SetProperty(ref _confirmPasswordErrorText, value);
        }

        private string _username;
        public string Username
        {
            get => _username;
            set
            {
                if (UserInputValidator.ValidateUsername(value) == 1)
                {
                    UsernameErrorText = "Username too short - must be at least 4 characters.";
                    _username = value;
                    return;
                }
                if (UserInputValidator.ValidateUsername(value) == 2)
                {
                    UsernameErrorText = "Username too long - must be no longer than 25 characters.";
                    _username = value;
                    return;
                }
                if (UserInputValidator.ValidateUsername(value) == 3)
                {
                    UsernameErrorText = "Username can't consist of white space characters.";
                    _username = value;
                    return;
                }
                UsernameErrorText = "";
                _username = value;
            }
        }

        private string _email;
        public string Email
        {
            get => _email;
            set
            {
                if (!UserInputValidator.IsValidEmail(value))
                {
                    EmailErrorText = "Email not valid.";
                    _email = value;
                    return;
                }
                _email = value;
                EmailErrorText = "";
            }
        }

        private string _password;
        public string Password
        {
            private get => _password;
            set
            {
                if (UserInputValidator.ValidatePassword(value) == 1)
                {
                    PasswordErrorText = "Password too short - must be at least 8 characters.";
                    _password = value;
                    return;
                }
                if (UserInputValidator.ValidatePassword(value) == 2)
                {
                    PasswordErrorText = "Password too long - must be no longer than 25 characters.";
                    _password = value;
                    return;
                }
                if (UserInputValidator.ValidatePassword(value) == 3 || UserInputValidator.ValidatePassword(value) == 4)
                {
                    PasswordErrorText = "Illegal characters in password.";
                    _password = value;
                    return;
                }
                _password = value;
                PasswordErrorText = "";
            }
        }

        private string _confirmPassword;
        public string ConfirmPassword
        {
            private get => _confirmPassword;
            set
            {
                if (value != Password)
                {
                    ConfirmPasswordErrorText = "Passwords do not match";
                    _confirmPassword = value;
                    return;
                }
                ConfirmPasswordErrorText = "";
                _confirmPassword = value;
            }
        }

        public ICommand RegisterCommand { get; set; }

        public ICommand GoBackCommand { get; set; }

        public RegistrationViewModel(UserDecksStore userDecksStore, NavigationService<LogInViewModel> logInService, IAuthenticationService authService, IDialogService dialogService)
        {
            _userDecksStore = userDecksStore;
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
                _dialogService.ShowSnackbarMessage("Account created", "You can now log in.");
                _logInService.Navigate();
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
                return;
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
            if (UserInputValidator.ValidatePassword(Password) == 3 || UserInputValidator.ValidatePassword(Password) == 4)
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
