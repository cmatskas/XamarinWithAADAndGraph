using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Identity.Client;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AuthWithAAD
{
    public partial class App : Application
    {
        public static object AuthUIParent = null;

        public static IPublicClientApplication ClientApplication;
        public static AuthenticationResult AuthResult;
        public static IAccount CurrentAccount;

        public App()
        {
            InitializeComponent();

            ClientApplication = PublicClientApplicationBuilder.Create(Constants.ClientId)
                                          .WithRedirectUri(Constants.RedirectUri)
                                          .WithAuthority($"https://login.microsoftonline.com/{Constants.TenantId}")
                                          .WithIosKeychainSecurityGroup(Constants.iOSKeyChainSecurityGroup)
                                          .Build();
            /*if (AuthResult != null)
            {
                MainPage = new NavigationPage(new PeoplePage());
            }
            else
            {*/
                MainPage = new NavigationPage(new MainPage());
            //}
        }
      
        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
