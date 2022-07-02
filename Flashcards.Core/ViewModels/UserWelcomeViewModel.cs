using Flashcards.Core.Stores;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards.Core.ViewModels
{
    public class UserWelcomeViewModel : ObservableObject
    {
        private readonly UserDecksStore _userDecksStore;

        public string Username => "Welcome " + _userDecksStore.UserDecksModel.Username + "!";

        public UserWelcomeViewModel(UserDecksStore userDecksStore)
        {
            _userDecksStore = userDecksStore;
        }
    }
}
