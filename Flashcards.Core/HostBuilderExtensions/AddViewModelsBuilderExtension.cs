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
                services.AddTransient<AddNewDeckViewModel>();
                services.AddSingleton<Func<AddNewDeckViewModel>>((s) => () => s.GetRequiredService<AddNewDeckViewModel>());
                services.AddSingleton<NavigationService<AddNewDeckViewModel>>();

                services.AddTransient<AlterFlashcardViewModel>();
                services.AddSingleton<Func<AlterFlashcardViewModel>>((s) => () => s.GetRequiredService<AlterFlashcardViewModel>());
                services.AddSingleton<NavigationService<AlterFlashcardViewModel>>();

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

                services.AddTransient<FrontLearnViewModel>();
                services.AddSingleton<Func<FrontLearnViewModel>>((s) => () => s.GetRequiredService<FrontLearnViewModel>());
                services.AddSingleton<NavigationService<FrontLearnViewModel>>();

                services.AddTransient<BackLearnViewModel>();
                services.AddSingleton<Func<BackLearnViewModel>>((s) => () => s.GetRequiredService<BackLearnViewModel>());
                services.AddSingleton<NavigationService<BackLearnViewModel>>();

                services.AddTransient<LogInViewModel>();
                services.AddSingleton<Func<LogInViewModel>>((s) => () => s.GetRequiredService<LogInViewModel>());
                services.AddSingleton<NavigationService<LogInViewModel>>();

                services.AddTransient<RegistrationViewModel>();
                services.AddSingleton<Func<RegistrationViewModel>>((s) => () => s.GetRequiredService<RegistrationViewModel>());
                services.AddSingleton<NavigationService<RegistrationViewModel>>();

                services.AddTransient<UserIconViewModel>();
                services.AddSingleton<Func<UserIconViewModel>>((s) => () => s.GetRequiredService<UserIconViewModel>());
                services.AddSingleton<NavigationService<UserIconViewModel>>();

                services.AddTransient<AccountManagementViewModel>();
                services.AddSingleton<Func<AccountManagementViewModel>>((s) => () => s.GetRequiredService<AccountManagementViewModel>());
                services.AddSingleton<NavigationService<AccountManagementViewModel>>();

                services.AddTransient<AccountInfoChangeViewModel>();
                services.AddSingleton<Func<AccountInfoChangeViewModel>>((s) => () => s.GetRequiredService<AccountInfoChangeViewModel>());
                services.AddSingleton<NavigationService<AccountInfoChangeViewModel>>();

                services.AddTransient<PasswordChangeViewModel>();
                services.AddSingleton<Func<PasswordChangeViewModel>>((s) => () => s.GetRequiredService<PasswordChangeViewModel>());
                services.AddSingleton<NavigationService<PasswordChangeViewModel>>();

                services.AddTransient<ActivityChartsViewModel>();
                services.AddSingleton<Func<ActivityChartsViewModel>>((s) => () => s.GetRequiredService<ActivityChartsViewModel>());
                services.AddSingleton<NavigationService<ActivityChartsViewModel>>();

                services.AddSingleton<MainViewModel>();
            });

            return hostBuilder;
        }



        private static HomeViewModel CreateHomeViewModel(IServiceProvider services)
        {
            return new HomeViewModel(services.GetRequiredService<NavigationService<DeckPreviewViewModel>>(),
                                        services.GetRequiredService<NavigationService<AddNewDeckViewModel>>(),
                                        services.GetRequiredService<UserDecksStore>());
        }

        private static DeckPreviewViewModel CreateDeckPreviewViewModel(IServiceProvider services)
        {
            return new DeckPreviewViewModel(services.GetRequiredService<UserDecksStore>(),
                                                services.GetRequiredService<NavigationService<UserWelcomeViewModel>>(),
                                                services.GetRequiredService<NavigationService<FlashcardManagementViewModel>>(),
                                                services.GetRequiredService<NavigationService<FrontLearnViewModel>>(),
                                                services.GetRequiredService<ReviewStore>(),
                                                services.GetRequiredService<NavigationService<AddNewDeckViewModel>>(),
                                                services.GetRequiredService<IDialogService>());
        }
    }
}
