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

        public string LabelText { get; set; }

        public string PreviousValueText { get; set; }

        public string UpperText { get; set; }

        public string UpperTextField { get; set; }

        public string Password { get; set; }

        public ICommand ButtonCommand { get; set; }

        public ICommand GoBackCommand { get; set; }

        public AccountInfoChangeViewModel(NavigationService<AccountManagementViewModel> navigationService, UserDecksStore userDecksStore)
        {
            _navigationService = navigationService;
            _userDecksStore = userDecksStore;

            LabelText = "Change username";
            PreviousValueText = "Current username: " + _userDecksStore.User.Name;
            UpperText = "New username: ";
            ButtonCommand = new RelayCommand(OnChangeUsernameClick);
            GoBackCommand = new RelayCommand(OnGoBackClick);

            _userDecksStore.EmailChangeRequest += PrepareForEmailChange;
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
            // validation todo
            _userDecksStore.User.Email = UpperTextField;
            await _userDecksStore.UserChange();
            _navigationService.Navigate();
        }

        private async void OnChangeUsernameClick()
        {
            // validation todo
            _userDecksStore.User.Name = UpperTextField;
            await _userDecksStore.UserChange();
            _navigationService.Navigate();
        }
    }
}
