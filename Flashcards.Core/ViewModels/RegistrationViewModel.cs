﻿using Flashcards.Core.Models;
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

        // need to fix those awaits probably
        private async void OnRegisterClick()
        {
            if (Password == ConfirmPassword)
            {
                bool ifSuccesfulRegistration = await _authService.CreateAccountAsync(Username, Email, Password);
                if (ifSuccesfulRegistration)
                {
                    User user = await _authService.LoginUserAsync(Username, Password);
                    _userDecksStore.Initialize(user);
                    _userWelcomeService.Navigate();
                    return;
                }
                _dialogService.ShowMessageDialog("ERROR", "Failed to register. Either username or email is taken.");
                return;
            }
            _dialogService.ShowMessageDialog("ERROR", "Password and confirmed password are different!");
        }
    }
}