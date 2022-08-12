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
        private readonly IGetPasswordService _passwordService;
        private readonly UserDecksStore _userDecksStore;
        private readonly NavigationService<UserWelcomeViewModel> _userWelcomeService;
        private readonly NavigationService<LogInViewModel> _logInService;

        public string Username { get; set; }
        public string Email { get; set; }

        public ICommand RegisterCommand { get; set; }

        public ICommand GoBackCommand { get; set; }

        public RegistrationViewModel(IGetPasswordService passwordService, UserDecksStore userDecksStore, NavigationService<UserWelcomeViewModel> userWelcomeService, NavigationService<LogInViewModel> logInService)
        {
            _passwordService = passwordService;
            _userDecksStore = userDecksStore;
            _userWelcomeService = userWelcomeService;
            _logInService = logInService;
            RegisterCommand = new RelayCommand(OnRegisterClick);
            GoBackCommand = new RelayCommand(OnGoBackClick);
        }

        private void OnGoBackClick()
        {
            _logInService.Navigate();
        }

        private void OnRegisterClick()
        {
            throw new NotImplementedException();
        }
    }
}
