using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Graph;
using Microsoft.Identity.Client;
using Xamarin.Forms;

namespace AuthWithAAD
{
    public partial class PeoplePage : ContentPage
    {
        public PeoplePage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            await LoadGraphData();
            base.OnAppearing();
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

        private async Task LoadGraphData()
        {
            if (App.AuthResult == null)
            {
                await AuthService.SignInAsync();
            }

            var graphServiceClient = new GraphServiceClient(new DelegateAuthenticationProvider((requestMessage) => {
                requestMessage
                    .Headers
                    .Authorization = new AuthenticationHeaderValue("Bearer", App.AuthResult.AccessToken);

                return Task.FromResult(0);
            }));

            var data = await graphServiceClient.Me.People.Request().GetAsync();

        }
    }
}
