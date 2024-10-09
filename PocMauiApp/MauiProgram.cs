using Microsoft.Extensions.Logging;
using Mopups.Hosting;
using PocMauiApp.Database;
using PocMauiApp.Model;
using PocMauiApp.View;
using PocMauiApp.ViewModel;

namespace PocMauiApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureMopups()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif
            var dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Blog3.db");

            builder.Services.AddSingleton(new DatabaseService(dbPath));
            builder.Services.AddTransient<BlogEntryViewModel>();
            builder.Services.AddTransient<BlogListViewModel>();
            builder.Services.AddTransient<AddPublisherViewModel>();
            builder.Services.AddTransient<PublisherViewModel>();
            builder.Services.AddTransient<StatusViewModel>();
            builder.Services.AddTransient<EditStatusViewModel>();
            builder.Services.AddTransient<BlogEntryPage>();
            builder.Services.AddTransient<BlogListPage>();
            builder.Services.AddTransient<PublisherPage>();
            builder.Services.AddTransient<AddPublisherPage>();
            builder.Services.AddTransient<StatusPage>();
            builder.Services.AddTransient<EditStatusPage>();

            return builder.Build();
        }
    }
}
