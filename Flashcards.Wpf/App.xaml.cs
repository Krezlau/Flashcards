using Flashcards.Core.HostBuilderExtensions;
using Flashcards.Core.Models;
using Flashcards.Core.Services;
using Flashcards.Core.Stores;
using Flashcards.Core.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.ObjectModel;
using System.Windows;

namespace Flashcards.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly IHost _host;

        public App()
        {
            _host = Host.CreateDefaultBuilder()
                .AddViewModels()
                .ConfigureServices(services =>
                {
                    services.AddSingleton<NavigationStore>();

                    services.AddSingleton<UserDecksStore>();

                    services.AddSingleton(s => new MainWindow()
                    {
                        DataContext = s.GetRequiredService<MainViewModel>()
                    });
                })
                .Build();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            _host.Start();

            NavigationStore _navigationStore = _host.Services.GetRequiredService<NavigationStore>();
            _navigationStore.CurrentViewModel = _host.Services.GetRequiredService<AddNewDeckViewModel>();
            _navigationStore.LeftViewModel = _host.Services.GetRequiredService<HomeViewModel>();

            UserDecksStore _userDecksStore = _host.Services.GetRequiredService<UserDecksStore>();
            _userDecksStore.UserDecksModel = new UserDecksModel { DeckList = new ObservableCollection<Deck>() };
            _userDecksStore.UserDecksModel.DeckList.Add(new Deck("lmao"));
            _userDecksStore.UserDecksModel.DeckList.Add(new Deck("xd"));
            _userDecksStore.UserDecksModel.DeckList.Add(new Deck("fajny deck"));

            MainWindow = _host.Services.GetRequiredService<MainWindow>();
            MainWindow.Show();

            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _host.Dispose();

            base.OnExit(e);
        }
    }
}
