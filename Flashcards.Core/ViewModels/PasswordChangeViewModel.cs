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

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        public string OldPassword { get; set; }

        public ICommand ButtonCommand { get; set; }

        public ICommand GoBackCommand { get; set; }

        public PasswordChangeViewModel(NavigationService<AccountManagementViewModel> navigationService, UserDecksStore userDecksStore, IAuthenticationService authService)
        {
            _navigationService = navigationService;
            _userDecksStore = userDecksStore;

            ButtonCommand = new RelayCommand(OnChangePasswordClick);
            GoBackCommand = new RelayCommand(OnGoBackClick);
            _authService = authService;
        }

        private void OnGoBackClick()
        {
            _navigationService.Navigate();
        }


        private async void OnChangePasswordClick()
        {
            // validation todo
            await _authService.ChangeUserPasswordAsync(Password, OldPassword, _userDecksStore.User.Name);
            _navigationService.Navigate();
        }
    }
}
