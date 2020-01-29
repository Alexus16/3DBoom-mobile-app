using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Threading;
//using Npgsql;

namespace App1
{
    
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    
    public partial class MainPage : ContentPage
    {

        
        public MainPage()
        {    
            InitializeComponent();
            buttonLamp.BackgroundColor = Color.White;
            buttonSettings.BackgroundColor = Color.White;
            buttonSensors.BackgroundColor = Color.White;
            button3dPrinter.BackgroundColor = Color.White;
            buttonWindows.BackgroundColor = Color.White;
            buttonSwitch.BackgroundColor = Color.White;
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            status.Text = "Loading...";
            status.TextColor = Color.Black;
            buttonLamp.IsEnabled = false;
            buttonWindows.IsEnabled = false;
            buttonSensors.IsEnabled = false;
            button3dPrinter.IsEnabled = false;
            buttonSwitch.IsEnabled = false;
            //buttonSettings.IsEnabled = false;
            object __bin;
            if (!App.Current.Properties.TryGetValue("ipServer", out __bin) || !App.Current.Properties.TryGetValue("user", out __bin) || !App.Current.Properties.TryGetValue("password", out __bin))
            {
                await Navigation.PushModalAsync(new Page3(), false);
            }
            else
            {
                if (await DataBase.init())
                {
                    await DataBase.connect();
                    if (await DataBase.refreshData() && !DataBase.isConnected)
                    {
                        status.Text = "Server connected & data loaded";
                        status.TextColor = Color.Green;
                        buttonLamp.IsEnabled = true;
                        buttonWindows.IsEnabled = true;
                        buttonSensors.IsEnabled = true;
                        button3dPrinter.IsEnabled = true;
                        buttonSwitch.IsEnabled = true;
                        //buttonSettings.IsEnabled = true;
                    }
                    else
                    {
                        status.Text = "Error of loading data";
                        status.TextColor = Color.Red;
                    }
                }
                else
                {
                    status.Text = "Error of connection to server";
                    status.TextColor = Color.Red;
                }

            }
            
        }
        private void buttonLamp_Clicked(object sender, EventArgs e)
        {
            buttonLamp.IsEnabled = false;
            Navigation.PushModalAsync(new Page1(),false);
            buttonLamp.IsEnabled = true;
        }

        private void buttonSettings_Clicked(object sender, EventArgs e)
        {
            buttonSettings.IsEnabled = false;
            Navigation.PushModalAsync(new Page4(),false);
            buttonSettings.IsEnabled = true;
        }

        private void button3dPrinter_Clicked(object sender, EventArgs e)
        {
            button3dPrinter.IsEnabled = false;
            Navigation.PushModalAsync(new Page7(),false);
            button3dPrinter.IsEnabled = true;
        }

        private void buttonSensors_Clicked(object sender, EventArgs e)
        {
            buttonLamp.IsEnabled = false;
            Navigation.PushModalAsync(new Page8(),false);
            buttonLamp.IsEnabled = true;
        }

        private void buttonWindows_Clicked(object sender, EventArgs e)
        {
            buttonLamp.IsEnabled = false;
            Navigation.PushModalAsync(new Page9(),false);
            buttonLamp.IsEnabled = true;
        }

        private void buttonSwitch_Clicked(object sender, EventArgs e)
        {
            buttonLamp.IsEnabled = false;
            Navigation.PushModalAsync(new Page5(),false);
            buttonLamp.IsEnabled = true;
        }
    }
}