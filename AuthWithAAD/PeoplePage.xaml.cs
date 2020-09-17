using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Graph;
using Xamarin.Forms;

namespace AuthWithAAD
{
    public partial class PeoplePage : ContentPage
    {
        public IList<Colleague> Colleagues { get; private set; }

        public PeoplePage()
        {
            InitializeComponent();
        }

        protected async override void OnAppearing()
        {
            var data = await LoadGraphData();
            Colleagues = TransformGraphDataToDto(data);
            base.OnAppearing();
        }

        private List<Colleague> TransformGraphDataToDto(IUserPeopleCollectionPage data)
        {
            var items = data.ToList();
            var colleagues = new List<Colleague>();
            foreach(var item in items)
            {
                var colleague = new Colleague
                {
                    Name = item?.GivenName,
                    Department = item?.Department,
                    EmailAddress = item?.ScoredEmailAddresses.FirstOrDefault().Address,
                    Title = item.JobTitle
                };
                colleagues.Add(colleague);
            }

            return colleagues;
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

        private async Task<IUserPeopleCollectionPage> LoadGraphData()
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

            return await graphServiceClient.Me.People.Request().GetAsync();
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
