using Flashcards.Core.Models;
using Flashcards.Core.Services;
using Flashcards.Core.Stores;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;
using System.Windows.Input;

namespace Flashcards.Core.ViewModels
{
    public class AddNewDeckViewModel : ObservableRecipient
    {
        private readonly NavigationService<HomeViewModel> _navigationService;
        private readonly UserDecksStore userDecksStore;

        public ICommand AddCommand { get; set; }

        private string deckName;

        public string DeckName
        {
            get => deckName;
            set => SetProperty(ref deckName, value);
        }

        public AddNewDeckViewModel(NavigationService<HomeViewModel> navigationService, UserDecksStore userDecksStore)
        {
            AddCommand = new RelayCommand(OnAddClick);
            _navigationService = navigationService;
            this.userDecksStore = userDecksStore;
        }

        private async void OnAddClick()
        {
            Deck deck = new Deck(DeckName);
            await userDecksStore.AddNewDeck(deck);
        }
    }
}
