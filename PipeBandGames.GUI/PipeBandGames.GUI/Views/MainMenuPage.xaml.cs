using PipeBandGames.BusinessLayer.Interfaces;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PipeBandGames.GUI.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MainMenuPage : ContentPage
	{
		public MainMenuPage()
		{
			InitializeComponent();
		}

        public void CreateContest_Clicked(object sender, EventArgs args)
        {
            // TODO: Figure out better way to do DI
            IContestService contestService = App.ServiceProvider.GetService(typeof(IContestService)) as IContestService;
            var createContestPage = new CreateContestPage(contestService);
            Navigation.PushAsync(createContestPage);
        }
    }
}