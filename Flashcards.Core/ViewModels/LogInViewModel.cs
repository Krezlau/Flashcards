using Flashcards.Core.Models;
using Flashcards.Core.Services;
using Flashcards.Core.Stores;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Flashcards.Core.ViewModels
{
    public class LogInViewModel : ObservableObject
    {
        private readonly IAuthenticationService _authService;
        private readonly IDialogService _dialogService;
        private readonly ILoadingService _loadingService;
        private readonly UserDecksStore _userDecksStore;
        private readonly NavigationService<UserWelcomeViewModel> _userWelcomeService;
        private readonly NavigationService<RegistrationViewModel> _registerService;

        public string Username { get; set; }

        public string Password { private get; set; }

        public ICommand LogInCommand { get; set; }

        public ICommand EnterLogInCommand { get; set; }

        public ICommand RegisterCommand { get; set; }

        public LogInViewModel(IAuthenticationService authService,
                              UserDecksStore userDecksStore,
                              NavigationService<UserWelcomeViewModel> userWelcomeService,
                              IDialogService dialogService,
                              NavigationService<RegistrationViewModel> registerService,
                              ILoadingService loadingService)
        {
            _authService = authService;
            LogInCommand = new RelayCommand(OnLogInClick);
            RegisterCommand = new RelayCommand(OnRegisterClick);
            EnterLogInCommand = new RelayCommand(OnLogInClick);
            _userDecksStore = userDecksStore;
            _userWelcomeService = userWelcomeService;
            _dialogService = dialogService;
            _registerService = registerService;
            _loadingService = loadingService;
        }

        private void OnRegisterClick()
        {
            _registerService.Navigate();
        }

        private async void OnLogInClick()
        {
            _loadingService.Enable();
            User user = await _authService.LoginUserAsync(Username, Password);
            if (user is not null)
            {
                await _userDecksStore.Initialize(user);
                _userWelcomeService.Navigate();
                _loadingService.Disable();
                return;
            }
            _loadingService.Disable();
            _dialogService.ShowMessageDialog("ERROR", "Failed to log in. Check if your username and password are correct.");
        }
    }
}
