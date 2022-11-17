using Flashcards.Core.DBConnection;
using Flashcards.Core.HostBuilderExtensions;
using Flashcards.Core.Models;
using Flashcards.Core.Services;
using Flashcards.Core.Services.UserDataChangers;
using Flashcards.Core.Services.UserDataCreators;
using Flashcards.Core.Services.UserDataDestroyers;
using Flashcards.Core.Services.UserDataProviders;
using Flashcards.Core.Services.UserDataValidators;
using Flashcards.Core.Stores;
using Flashcards.Core.ViewModels;
using Flashcards.WpfApp.Services;
using Flashcards.WpfApp.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace Flashcards.WpfApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IHost _host;
        private const string CONNECTION_STRING = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=FlashcardDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";


        public App()
        {
            _host = Host.CreateDefaultBuilder()
                .AddViewModels()
                .ConfigureServices(services =>
                {
                    services.AddDbContext<UsersContext>(options =>
                    {
                        options.UseSqlServer(CONNECTION_STRING);
                    });

                    services.AddSingleton<IUserDataProvider, DatabaseUserDataProvider>();
                    services.AddSingleton<IUserDataCreator, DatabaseUserDataCreator>();
                    services.AddSingleton<IUserDataDestroyer, DatabaseUserDataDestroyer>();
                    services.AddSingleton<IUserDataChanger, DatabaseUserDataChanger>();
                    services.AddSingleton<IAuthenticationService, DatabaseAuthService>();
                    services.AddSingleton<IUserDataValidator, DatabaseUserDataValidator>();
                    services.AddSingleton<IUserDbContextFactory>(new UserDbContextFactory(CONNECTION_STRING));

                    services.AddSingleton<NavigationStore>();

                    services.AddSingleton<SelectionStore>();

                    services.AddSingleton<UserDecksStore>();

                    services.AddSingleton<ReviewStore>();

                    services.AddSingleton<Wpf.Ui.Mvvm.Services.SnackbarService>();

                    services.AddSingleton<Wpf.Ui.Mvvm.Services.DialogService>();

                    services.AddSingleton<LoadingRingService>();

                    services.AddSingleton<ILoadingService, WpfLoadingService>();

                    services.AddSingleton<IDialogService, WpfDialogService>();

                    services.AddSingleton<MainWindow>();
                })
                .Build();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            _host.Start();

            NavigationStore _navigationStore = _host.Services.GetRequiredService<NavigationStore>();
            _navigationStore.CurrentViewModel = _host.Services.GetRequiredService<LogInViewModel>();

            
            MainWindow = _host.Services.GetRequiredService<MainWindow>();
            MainWindow.Show();

            base.OnStartup(e);
        }

        protected async override void OnExit(ExitEventArgs e)
        {
            ReviewStore _reviewStore = _host.Services.GetRequiredService<ReviewStore>();
            if (_reviewStore.IfSessionActive)
            {
                UserDecksStore _userDecksStore = _host.Services.GetRequiredService<UserDecksStore>();
                await _userDecksStore.SaveSessionTime(_reviewStore.EndOfLearning());
            }

            _host.Dispose();

            base.OnExit(e);
        }
    }
}
