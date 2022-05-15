using Flashcards.Core.Commands;
using Flashcards.Core.Messages;
using Flashcards.Core.Services;
using Flashcards.Core.Stores;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Flashcards.Core.ViewModels
{
    public class AddNewDeckViewModel : ObservableRecipient
    {
        private readonly NavigationService<HomeViewModel> _navigationService;

        public ICommand AddCommand { get; set; }

        private string deckName;

        [Required]
        [MinLength(3)]
        public string DeckName
        {
            get => deckName;
            set => SetProperty(ref deckName, value);
        }

        public AddNewDeckViewModel(NavigationStore navigationStore)
        {
            AddCommand = new RelayCommand(OnAddClick);
            _navigationService = new NavigationService<HomeViewModel>(
                navigationStore, () => new HomeViewModel(navigationStore));
        }

        private void OnAddClick()
        {
            Messenger.Send(new NewDeckMessage(DeckName));
            _navigationService.Navigate();
        }
    }
}
