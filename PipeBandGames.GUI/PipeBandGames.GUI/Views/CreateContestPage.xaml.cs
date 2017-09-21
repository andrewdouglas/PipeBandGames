using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PipeBandGames.GUI.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CreateContestPage : ContentPage
	{
        public CreateContestPage()
		{
			InitializeComponent();

            // This class is responsible for bindings...the properties in this class are bound to XAML controls
            BindingContext = this;
		}

        // Bound to the Entry control for the contest name
        public string Name { get; set; }

        // Bound to the DatePicker for the contest date
        public DateTime Date { get; set; }

        // Bound to the DatePicker to force the user to select a date of today or something in the future
        public DateTime Today { get; } = DateTime.Today;

        protected override void OnAppearing()
        {
            base.OnAppearing();

            // Set focus to the Name field for convenience
            this.txtName.Focus();
        }

        protected void CreateContest_Clicked(object sender, EventArgs args)
        {
            // Crude form validation...this should be improved upon
            if (string.IsNullOrWhiteSpace(this.Name))
            {
                DisplayAlert("Alert", $"Please enter a name", "OK");
                this.txtName.Focus();
                return;
            }

            if (this.Date == DateTime.MinValue)
            {
                DisplayAlert(string.Empty, $"Please select a date", "OK");
                return;
            }

            // TODO: Create the contest
            DisplayAlert("Alert", $"Creating contest {this.Name} on {this.Date}", "OK");
        }
    }
}