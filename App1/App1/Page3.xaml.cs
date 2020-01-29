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
    public partial class Page3 : ContentPage
    {
        public Page3()
        {
            InitializeComponent();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            string str1 = ip1.Text + "." + ip2.Text + "." + ip3.Text + "." + ip4.Text;
            string str2 = user.Text;
            string str3 = password.Text;
            App.Current.Properties["ipServer"] = str1;
            App.Current.Properties["user"] = str2;
            App.Current.Properties["password"] = str3;
            Navigation.PopModalAsync(false);
        }
    }
}