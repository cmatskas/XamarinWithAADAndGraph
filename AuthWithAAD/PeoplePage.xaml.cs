using System;
using Xamarin.Forms;

namespace AuthWithAAD
{
    public partial class PeoplePage : ContentPage
    {
        ColleagueViewModel viewModel;
        public PeoplePage()
        {
            InitializeComponent();
            BindingContext = viewModel = new ColleagueViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Colleagues.Count == 0)
                viewModel.IsBusy = true;
        }

        public async void OnLogoutClicked(object sender, EventArgs e)
        {
            ToolbarItem item = (ToolbarItem)sender;
            if (item.Text == "Logout")
            {
                await AuthService.SignOut();

                Navigation.InsertPageBefore(new MainPage(), this);
                await Navigation.PopAsync();
            }
        }

    }

    public class Colleague
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public string Department { get; set; }
        public string EmailAddress { get; set; }
    }
}
