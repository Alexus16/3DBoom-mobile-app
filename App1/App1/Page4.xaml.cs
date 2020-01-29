using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App1
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Page4 : ContentPage
    {
        public Page4()
        {
            InitializeComponent();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            App.Current.Properties.Remove("ipServer");
            App.Current.Properties.Remove("user");
            App.Current.Properties.Remove("password");
        }
        private async void _backButton_Clicked(object sender, EventArgs e)
        {
            _backButton.IsEnabled = false;
            await Navigation.PopModalAsync(false);
            _backButton.IsEnabled = true;
        }
    }
}