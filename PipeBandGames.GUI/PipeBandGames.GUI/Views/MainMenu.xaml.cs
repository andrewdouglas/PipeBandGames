using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PipeBandGames.GUI.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MainMenu : ContentPage
	{
		public MainMenu ()
		{
			InitializeComponent ();
		}

        public void CreateContest_Clicked(object sender, EventArgs args)
        {
            DisplayAlert("Alert", "You have been alerted", "OK");
        }
    }
}