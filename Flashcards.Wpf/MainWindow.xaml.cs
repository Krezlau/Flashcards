using Flashcards.Core.ViewModels;
using Flashcards.WpfApp.Services;
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
        private readonly LoadingRingService _loadingService;
        public MainWindow(SnackbarService snackbarService,
                          MainViewModel mainViewModel,
                          DialogService dialogService,
                          LoadingRingService loadingService)
        {
            InitializeComponent();
            DataContext = mainViewModel;
            _snackbarService = snackbarService;
            _snackbarService.SetSnackbarControl(RootSnackbar);
            _dialogService = dialogService;
            _dialogService.SetDialogControl(RootDialog);
            _loadingService = loadingService;
            _loadingService.SetLoadingScreen(LoadingScreen);
        }
    }
}
