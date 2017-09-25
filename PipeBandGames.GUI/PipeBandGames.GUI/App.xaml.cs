using Microsoft.Extensions.DependencyInjection;
using PipeBandGames.BusinessLayer.Interfaces;
using PipeBandGames.BusinessLayer.Services;
using PipeBandGames.DataLayer;
using PipeBandGames.GUI.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace PipeBandGames.GUI
{
    public partial class App : Application
	{
        private static IServiceProvider serviceProvider;

        public static IServiceProvider ServiceProvider
        {
            get { return serviceProvider; }
        }

        public App()
		{
            // From: https://stackoverflow.com/questions/30257710/using-startup-class-in-asp-net5-console-application
            // Set up dependency injection
            var services = new ServiceCollection();
            this.ConfigureServices(services);
            serviceProvider = services.BuildServiceProvider();

            InitializeDatabase();

			InitializeComponent();

			SetMainPage();
		}

		public void SetMainPage()
		{
            Current.MainPage = new NavigationPage(ServiceProvider.GetService<MainMenuPage>());
            ////Current.MainPage = new TabbedPage
            ////{
            ////    Children =
            ////    {
            ////        new NavigationPage(new ItemsPage())
            ////        {
            ////            Title = "Browse",
            ////            Icon = new FileImageSource { File = @"C:\Programming\PipeBandGames\PipeBandGames\PipeBandGames.GUI\PipeBandGames.GUI.iOS\Resources\tab_feed.png" } // Device.OnPlatform<string>("tab_feed.png",null,null)
            ////        },
            ////        new NavigationPage(new AboutPage())
            ////        {
            ////            Title = "About",
            ////            Icon = "tab_about.png" // Device.OnPlatform<string>("tab_about.png",null,null)
            ////        },
            ////    }
            ////};
        }

        private void InitializeDatabase()
        {
            using (var db = ServiceProvider.GetService<IPipeBandGamesContext>() as PipeBandGamesContext) // new PipeBandGamesContext())
            {
                // This works on initial run, but throws exception on second run...why?
                //db.Database.Migrate();

                // Creates the DB if it doesn't exist, but doesn't do anything if it does.  Does NOT apply migrations.
                db.Database.EnsureCreated();

                // This works!
                // db.Persons.Add(new Person { FirstName = "Bob", LastName = "Marley", Created = DateTime.Now, CreatedBy = 1 });
                // db.SaveChanges();
            }
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<PipeBandGamesContext>();

            services.AddTransient<MainMenuPage>();
            services.AddTransient<CreateContestPage>();

            services.AddTransient<IPipeBandGamesContext, PipeBandGamesContext>();
            services.AddTransient<IContestService, ContestService>();
        }
    }
}
