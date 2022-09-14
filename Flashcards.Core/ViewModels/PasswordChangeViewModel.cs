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
    public class PasswordChangeViewModel : ObservableObject
    {
        private readonly UserDecksStore _userDecksStore;
        private readonly NavigationService<AccountManagementViewModel> _navigationService;
        private readonly IAuthenticationService _authService;
        private readonly IDialogService _dialogService;

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        public string OldPassword { get; set; }

        public ICommand ButtonCommand { get; set; }

        public ICommand GoBackCommand { get; set; }

        public PasswordChangeViewModel(NavigationService<AccountManagementViewModel> navigationService, UserDecksStore userDecksStore, IAuthenticationService authService, IDialogService dialogService)
        {
            _navigationService = navigationService;
            _userDecksStore = userDecksStore;

            ButtonCommand = new RelayCommand(OnChangePasswordClick);
            GoBackCommand = new RelayCommand(OnGoBackClick);
            _authService = authService;
            _dialogService = dialogService;
        }

        private void OnGoBackClick()
        {
            _navigationService.Navigate();
        }


        private async void OnChangePasswordClick()
        {
            if (UserInputValidator.ValidatePassword(Password) == 1)
            {
                _dialogService.ShowMessageDialog("ERROR", "Failed to change. Password is too short - must be at least 8 characters.");
                return;
            }
            if (UserInputValidator.ValidatePassword(Password) == 2)
            {
                _dialogService.ShowMessageDialog("ERROR", "Failed to change. Password is too long - must be no longer than 25 characters.");
                return;
            }
            if (UserInputValidator.ValidatePassword(Password) == 3 || UserInputValidator.ValidatePassword(Password) == 4)
            {
                _dialogService.ShowMessageDialog("ERROR", "Failed to change. Illegal characters in password.");
                return;
            }
            bool ifPasswordCorrect = await _authService.IfPasswordCorrect(OldPassword, _userDecksStore.User.Name);
            if (!ifPasswordCorrect)
            {
                _dialogService.ShowMessageDialog("ERROR", "Password is not correct.");
                return;
            }
            await _authService.ChangeUserPasswordAsync(Password, OldPassword, _userDecksStore.User.Name);
            _dialogService.ShowSnackbarMessage("SUCCESS", "Password changed.");
            _navigationService.Navigate();
        }
    }
}
