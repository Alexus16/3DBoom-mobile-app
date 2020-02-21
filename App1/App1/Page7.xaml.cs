using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Threading;
using LibVLCSharp;
using LibVLCSharp.Shared;
using LibVLCSharp.Forms.Shared;
using VideoView = LibVLCSharp.Forms.Shared.VideoView;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App1
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Page7 : ContentPage
    {
        
        TcpClient client;
        string ipServer;
        public Page7()
        {
            InitializeComponent();
            _playButton.BackgroundColor = Color.White;
            _stopButton.BackgroundColor = Color.White;
            _pauseButton.BackgroundColor = Color.White;
            _homeButton.BackgroundColor = Color.White;
            _rebootButton.BackgroundColor = Color.White;
        }
        protected override void OnAppearing()
        {
            string[] octets = DataBase.__ip.Split('.');
            ipServer = octets[0] + "." + octets[1] + "." + octets[2] + ".220";
            webView1.HorizontalOptions = LayoutOptions.Center;
            webView1.Source = new UrlWebViewSource { Url = $"http://{ipServer}/" };
        }

        private async void _playButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                string str = "Files";
                byte[] data = Encoding.ASCII.GetBytes(str);
                client = new TcpClient();
                client.Connect(ipServer, 1234);
                StringBuilder stringBuilder = new StringBuilder();
                NetworkStream stream = client.GetStream();
                stream.Write(data, 0, data.Length);
                byte[] recData = new byte[64];
                while(!stream.DataAvailable);
                while(stream.DataAvailable)
                {
                    int readSize = stream.Read(recData, 0, recData.Length);
                    stringBuilder.Append(Encoding.ASCII.GetString(recData, 0, readSize));
                }
                string recStr = stringBuilder.ToString();
                string[] names = recStr.Split(';');
                string nameSelected = await DisplayActionSheet("File to print", "Cancel", null, names);
                if(nameSelected != "Cancel")
                {
                    byte[] data2 = Encoding.ASCII.GetBytes(nameSelected);
                    stream.Write(data2, 0, data2.Length);
                }
                stream.Close();
                client.Close();
            }
            catch { }
        }

        private void _stopButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                string str = "Stop";
                byte[] data = Encoding.ASCII.GetBytes(str);
                client = new TcpClient();
                client.Connect(ipServer, 1234);
                NetworkStream stream = client.GetStream();
                stream.Write(data, 0, data.Length);
                stream.Close();
                client.Close();
            }
            catch { }
        }

        private void _pauseButton_Clicked(object sender, EventArgs e)
        {
            try
            { 
            string str = "Pause";
            byte[] data = Encoding.ASCII.GetBytes(str);
            client = new TcpClient();
            client.Connect(ipServer, 1234);
            NetworkStream stream = client.GetStream();
            stream.Write(data, 0, data.Length);
            stream.Close();
            client.Close();
            }
            catch { }
        }
        private async void _backButton_Clicked(object sender, EventArgs e)
        {
            _backButton.IsEnabled = false;
            await Navigation.PopModalAsync(false);
            _backButton.IsEnabled = true;
        }
        private async void _refreshButton_Clicked(object sender, EventArgs e)
        {
        }

        private void _homeButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                string str = "Home";
                byte[] data = Encoding.ASCII.GetBytes(str);
                client = new TcpClient();
                client.Connect(ipServer, 1234);
                NetworkStream stream = client.GetStream();
                stream.Write(data, 0, data.Length);
                stream.Close();
                client.Close();
            }
            catch { }
        }

        private void _rebootButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                string str = "Reboot";
                byte[] data = Encoding.ASCII.GetBytes(str);
                client = new TcpClient();
                client.Connect(ipServer, 1234);
                NetworkStream stream = client.GetStream();
                stream.Write(data, 0, data.Length);
                stream.Close();
                client.Close();
            }
            catch { }
        }
        private void refreshViews()
        {
            try
            {
                string str = "Refresh";
                byte[] data = Encoding.ASCII.GetBytes(str);
                client = new TcpClient();
                client.Connect(ipServer, 1234);
                NetworkStream stream = client.GetStream();
                stream.Write(data, 0, data.Length);
                stream.Close();
                client.Close();
            }
            catch { }
        }
    }
}