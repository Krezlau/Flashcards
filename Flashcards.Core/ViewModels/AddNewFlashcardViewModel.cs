using Flashcards.Core.Services;
using Flashcards.Core.Stores;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards.Core.ViewModels
{
    public class AddNewFlashcardViewModel : ObservableObject
    {
        private readonly NavigationService<DeckPreviewViewModel> _navigationService;

        public AddNewFlashcardViewModel(NavigationStore navigationStore)
        {
           // _navigationService = new NavigationService<DeckPreviewViewModel>(navigationStore, () => new DeckPreviewViewModel(navigationStore,));
        }
    }
}
