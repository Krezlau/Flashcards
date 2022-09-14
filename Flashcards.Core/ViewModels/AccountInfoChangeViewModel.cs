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
    public class AccountInfoChangeViewModel : ObservableObject
    {
        private readonly UserDecksStore _userDecksStore;
        private readonly NavigationService<AccountManagementViewModel> _navigationService;
        private readonly IAuthenticationService _authService;
        private readonly IDialogService _dialogService;

        private bool ifEmailChange = false;

        public string LabelText { get; set; }

        public string PreviousValueText { get; set; }

        public string UpperText { get; set; }

        private string _upperTextField;
        public string UpperTextField
        {
            get => _upperTextField;
            set
            {
                if (ifEmailChange)
                {
                    if (!UserInputValidator.IsValidEmail(value))
                    {
                        ErrorText = "Email not valid.";
                        _upperTextField = value;
                        return;
                    }
                    _upperTextField = value;
                    ErrorText = "";
                    return;
                }
                if (!ifEmailChange)
                {
                    if (UserInputValidator.ValidateUsername(value) == 0)
                    {
                        ErrorText = "";
                        _upperTextField = value;
                        return;
                    }
                    if (UserInputValidator.ValidateUsername(value) == 1)
                    {
                        ErrorText =  "Username is too short - must be at least 4 characters.";
                        _upperTextField = value;
                        return;
                    }
                    if (UserInputValidator.ValidateUsername(value) == 2)
                    {
                        ErrorText = "Username is too long - must be no longer than 25 characters.";
                        _upperTextField = value;
                        return;
                    }
                    if (UserInputValidator.ValidateUsername(value) == 3)
                    {
                        ErrorText = "Username can't consist of white space characters.";
                        _upperTextField = value;
                        return;
                    }
                    _upperTextField = value;
                }
            }
        }

        public string Password { get; set; }

        public string UpperTextTrim => UpperText.Replace(':', ' ');

        private string _errorText;
        public string ErrorText
        {
            get => _errorText;
            set => SetProperty(ref _errorText, value);
        }

        public ICommand ButtonCommand { get; set; }

        public ICommand GoBackCommand { get; set; }

        public AccountInfoChangeViewModel(NavigationService<AccountManagementViewModel> navigationService, UserDecksStore userDecksStore, IAuthenticationService authService, IDialogService dialogService)
        {
            _navigationService = navigationService;
            _userDecksStore = userDecksStore;

            LabelText = "Change username";
            PreviousValueText = "Current username: " + _userDecksStore.User.Name;
            UpperText = "New username: ";
            ButtonCommand = new RelayCommand(OnChangeUsernameClick);
            GoBackCommand = new RelayCommand(OnGoBackClick);

            _userDecksStore.EmailChangeRequest += PrepareForEmailChange;
            _authService = authService;
            _dialogService = dialogService;
        }

        private void OnGoBackClick()
        {
            _navigationService.Navigate();
        }

        private void PrepareForEmailChange()
        {
            LabelText = "Change email";
            PreviousValueText = "Current email: " + _userDecksStore.User.Email;
            UpperText = "New email: ";
            ButtonCommand = new RelayCommand(OnChangeEmailClick);
            ifEmailChange = true;
        }

        private async void OnChangeEmailClick()
        { 
            bool ifPasswordCorrect = await _authService.IfPasswordCorrect(Password, _userDecksStore.User.Name);
            if (!ifPasswordCorrect)
            {
                _dialogService.ShowMessageDialog("ERROR", "Password is not correct.");
                return;
            }

            bool outcome = await _userDecksStore.ChangeUserEmail(UpperTextField);
            if (outcome == false)
            {
                _dialogService.ShowMessageDialog("ERROR", "Failed to change. Email already taken.");
                return;
            }
            _userDecksStore.User.Email = UpperTextField;
            _dialogService.ShowSnackbarMessage("SUCCESS", "Email changed.");
            _navigationService.Navigate();
        }

        private async void OnChangeUsernameClick()
        {
            bool ifPasswordCorrect = await _authService.IfPasswordCorrect(Password, _userDecksStore.User.Name);
            if (!ifPasswordCorrect)
            {
                _dialogService.ShowMessageDialog("ERROR", "Password is not correct.");
                return;
            }

            bool outcome = await _userDecksStore.ChangeUserName(UpperTextField);
            if (outcome == false)
            {
                _dialogService.ShowMessageDialog("ERROR", "Failed to change. Username already taken.");
                return;
            }
            _userDecksStore.User.Name = UpperTextField;
            _dialogService.ShowSnackbarMessage("SUCCESS", "Username changed.");
            _navigationService.Navigate();
        }
    }
}
