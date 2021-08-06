using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
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
                var colleagueData = await LoadGraphData();
                TransformGraphDataToDto(colleagueData);
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

        private async Task<string> LoadGraphData()
        {
            if (App.AuthResult == null)
            {
                await AuthService.SignInAsync();
            }

            var client = new HttpClient();
            var message = new HttpRequestMessage(HttpMethod.Get, "https://graph.microsoft.com/v1.0/me/people");
            message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", App.AuthResult.AccessToken);
            var response = await client.SendAsync(message).ConfigureAwait(false);
            var responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return responseString;
        }

        private void TransformGraphDataToDto(string data)
        {
            var extractedData = JObject.Parse(data);
            var colleagues = extractedData["value"];
            foreach (var colleague in colleagues)
            {
                var newColleague = new Colleague
                {
                    Name = colleague["displayName"].ToString(),
                    Department = colleague["department"].ToString(),
                    EmailAddress = colleague["scoredEmailAddresses"][0]["address"].ToString(),
                    Title = colleague["jobTitle"].ToString()
                };

                if(newColleague.Name.Equals("All Company", StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }
                Colleagues.Add(newColleague);
            }
        }
    }
}