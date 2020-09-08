using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AuthWithAAD
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        async void SignIn_Clicked(System.Object sender, System.EventArgs e)
        {
            await AuthService.SignInAsync();
            await Navigation.PushAsync(new PeoplePage());
        }
    }
}
