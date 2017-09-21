using PipeBandGames.GUI.Views;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace PipeBandGames.GUI
{
	public partial class App : Application
	{
        public App()
		{
            InitializeDatabase();

			InitializeComponent();

			SetMainPage();
		}

		public static void SetMainPage()
		{
            Current.MainPage = new NavigationPage(new MainMenuPage());
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

        private static void InitializeDatabase()
        {
            ////using (var db = new PipeBandGamesContext())
            ////{
            ////    // This works on initial run, but throws exception on second run...why?
            ////    //db.Database.Migrate();

            ////    // Creates the DB if it doesn't exist, but doesn't do anything if it does.  Does NOT apply migrations.
            ////    db.Database.EnsureCreated();

            ////    // This works!
            ////    // db.Persons.Add(new Person { FirstName = "Bob", LastName = "Marley", Created = DateTime.Now, CreatedBy = 1 });
            ////    // db.SaveChanges();
            ////}
        }
    }
}
