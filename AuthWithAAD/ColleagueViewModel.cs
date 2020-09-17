using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Graph;
using Xamarin.Forms;

namespace AuthWithAAD
{
    public class ColleagueViewModel : BaseViewModel
    {

        public ObservableCollection<Colleague> Colleagues { get; set; }
        public Command LoadItemsCommand { get; set; }

        public ColleagueViewModel()
        {
            Title = "My Colleagues";
            Colleagues = new ObservableCollection<Colleague>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

        }

        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                Colleagues.Clear();
                var data = await LoadGraphData();
                TransformGraphDataToDto(data);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task<IUserPeopleCollectionPage> LoadGraphData()
        {
            if (App.AuthResult == null)
            {
                await AuthService.SignInAsync();
            }

            var graphServiceClient = new GraphServiceClient(new DelegateAuthenticationProvider((requestMessage) =>
            {
                requestMessage
                    .Headers
                    .Authorization = new AuthenticationHeaderValue("Bearer", App.AuthResult.AccessToken);

                return Task.FromResult(0);
            }));

            return await graphServiceClient.Me.People.Request().GetAsync();
        }

        private void TransformGraphDataToDto(IUserPeopleCollectionPage data)
        {
            var items = data.ToList();

            foreach (var item in items)
            {
                var colleague = new Colleague
                {
                    Name = item?.GivenName,
                    Department = item?.Department,
                    EmailAddress = item?.ScoredEmailAddresses.FirstOrDefault().Address,
                    Title = item.JobTitle
                };
                Colleagues.Add(colleague);
            }
        }
    }
}