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
            Navigation.PushAsync(new CreateContestPage());
        }
    }
}