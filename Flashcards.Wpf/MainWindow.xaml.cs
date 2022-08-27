using Flashcards.Core.ViewModels;
using System.Windows;
using Wpf.Ui.Controls;
using Wpf.Ui.Mvvm.Services;

namespace Flashcards.WpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : UiWindow
    {
        private readonly SnackbarService _snackbarService;
        private readonly DialogService _dialogService;
        public MainWindow(SnackbarService snackbarService, MainViewModel mainViewModel, DialogService dialogService)
        {
            InitializeComponent();
            DataContext = mainViewModel;
            _snackbarService = snackbarService;
            _snackbarService.SetSnackbarControl(RootSnackbar);
            _dialogService = dialogService;
            _dialogService.SetDialogControl(RootDialog);
        }
    }
}
