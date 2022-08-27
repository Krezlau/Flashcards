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

        public string LabelText { get; set; }

        public string PreviousValueText { get; set; }

        public string UpperText { get; set; }

        public string UpperTextField { get; set; }

        public string Password { get; set; }

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
        }

        private async void OnChangeEmailClick()
        { 
            if (!UserInputValidator.IsValidEmail(UpperTextField))
            {
                _dialogService.ShowMessageDialog("ERROR", "Failed to change. Email not valid.");
                return;
            }

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
            if (UserInputValidator.ValidateUsername(UpperTextField) == 1)
            {
                _dialogService.ShowMessageDialog("ERROR", "Failed to change. New username is too short - must be at least 4 characters.");
                return;
            }
            if (UserInputValidator.ValidateUsername(UpperTextField) == 2)
            {
                _dialogService.ShowMessageDialog("ERROR", "Failed to change. New username is too long - must be no longer than 25 characters.");
                return;
            }
            if (UserInputValidator.ValidateUsername(UpperTextField) == 3)
            {
                _dialogService.ShowMessageDialog("ERROR", "Failed to change. New username can't consist of white space characters.");
                return;
            }

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
