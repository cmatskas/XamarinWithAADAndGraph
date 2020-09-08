using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Identity.Client;

namespace AuthWithAAD
{
    public static class AuthService
    {
        public static async Task SignInAsync()
        {
            try
            {
                if (App.CurrentAccount == null)
                {
                    var accounts = await App.ClientApplication.GetAccountsAsync();
                    App.CurrentAccount = accounts.FirstOrDefault();
                }

                App.AuthResult = await App.ClientApplication
                    .AcquireTokenSilent(Constants.Scopes, App.CurrentAccount)
                    .ExecuteAsync();
            }
            catch (MsalUiRequiredException)
            {
                var interactiveRequest = App.ClientApplication.AcquireTokenInteractive(Constants.Scopes);

                if (App.AuthUIParent != null)
                {
                    interactiveRequest = interactiveRequest
                        .WithParentActivityOrWindow(App.AuthUIParent);
                }

                App.AuthResult = await interactiveRequest.ExecuteAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Authentication failed. See exception messsage for more details: " + ex.Message);
            }
        }

        public static async Task SignOut()
        {
            await App.ClientApplication.RemoveAsync(App.CurrentAccount);
            App.AuthResult = null;
            App.CurrentAccount = null;
        }
    }
}
