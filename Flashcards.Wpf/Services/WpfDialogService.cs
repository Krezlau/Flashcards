using Flashcards.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Wpf.Ui.Mvvm.Services;

namespace Flashcards.WpfApp.Services
{
    public class WpfDialogService : IDialogService
    {
        private readonly DialogService _dialogService;
        private readonly SnackbarService _snackbarService;

        public WpfDialogService(DialogService dialogService, SnackbarService snackbarService)
        {
            _dialogService = dialogService;
            _snackbarService = snackbarService;
        }

        public void ShowMessageDialog(string title, string message)
        {
            _dialogService.GetDialogControl().Footer = new FooterControl(_dialogService.GetDialogControl());
            _dialogService.GetDialogControl().Show(title, message);
        }

        public void ShowSnackbarMessage(string title, string message)
        {
            _snackbarService.Show(title, message);
        }
    }
}
