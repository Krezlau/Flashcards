using Flashcards.Core.Services;
using Flashcards.Core.Stores;
using Flashcards.Core.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Flashcards.Core.HostBuilderExtensions
{
    public static class AddViewModelsBuilderExtension
    {
        public static IHostBuilder AddViewModels(this IHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices(services =>
            {
                services.AddTransient((s) => CreateAddNewDeckViewModel(s));
                services.AddSingleton<Func<AddNewDeckViewModel>>((s) => () => s.GetRequiredService<AddNewDeckViewModel>());
                services.AddSingleton<NavigationService<AddNewDeckViewModel>>();

                services.AddTransient<AddNewFlashcardViewModel>();
                services.AddSingleton<Func<AddNewFlashcardViewModel>>((s) => () => s.GetRequiredService<AddNewFlashcardViewModel>());
                services.AddSingleton<NavigationService<AddNewFlashcardViewModel>>();

                services.AddTransient((s) => CreateDeckPreviewViewModel(s));
                services.AddSingleton<Func<DeckPreviewViewModel>>((s) => () => s.GetRequiredService<DeckPreviewViewModel>());
                services.AddSingleton<NavigationService<DeckPreviewViewModel>>();

                services.AddTransient((s) => CreateHomeViewModel(s));
                services.AddSingleton<Func<HomeViewModel>>((s) => () => s.GetRequiredService<HomeViewModel>());
                services.AddSingleton<NavigationService<HomeViewModel>>();

                services.AddTransient<UserWelcomeViewModel>();
                services.AddSingleton<Func<UserWelcomeViewModel>>((s) => () => s.GetRequiredService<UserWelcomeViewModel>());
                services.AddSingleton<NavigationService<UserWelcomeViewModel>>();

                services.AddTransient<FlashcardManagementViewModel>();
                services.AddSingleton<Func<FlashcardManagementViewModel>>((s) => () => s.GetRequiredService<FlashcardManagementViewModel>());
                services.AddSingleton<NavigationService<FlashcardManagementViewModel>>();


                services.AddSingleton<MainViewModel>();
            });

            return hostBuilder;
        }

        private static AddNewDeckViewModel CreateAddNewDeckViewModel(IServiceProvider services)
        {
            return new AddNewDeckViewModel(services.GetRequiredService<NavigationService<HomeViewModel>>(), services.GetRequiredService<UserDecksStore>());
        }

        private static HomeViewModel CreateHomeViewModel(IServiceProvider services)
        {
            return new HomeViewModel(services.GetRequiredService<NavigationService<DeckPreviewViewModel>>(),
                                        services.GetRequiredService<NavigationService<AddNewDeckViewModel>>(),
                                        services.GetRequiredService<UserDecksStore>());
        }

        private static DeckPreviewViewModel CreateDeckPreviewViewModel(IServiceProvider services)
        {
            return new DeckPreviewViewModel(services.GetRequiredService<NavigationService<AddNewFlashcardViewModel>>(), services.GetRequiredService<UserDecksStore>(),
                                                services.GetRequiredService<NavigationService<UserWelcomeViewModel>>(), services.GetRequiredService<NavigationService<FlashcardManagementViewModel>>());
        }
    }
}
