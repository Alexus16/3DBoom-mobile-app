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
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            Core.Initialize();
            ipServer = App.Current.Properties["ipServer"].ToString();
            LibVLC _libVLC = new LibVLC();
            MediaPlayer _mediaPlayer = new MediaPlayer(_libVLC) { EnableHardwareDecoding = true };

            VideoView _videoView = new VideoView() { MediaPlayer = _mediaPlayer };
            field.Content = _videoView;
            var media = new Media(_libVLC, $"http://{ipServer}:8081/", FromType.FromLocation);
            DisplayAlert("source", $"http://{ipServer}:8081/", "OK");
            _videoView.MediaPlayer.Play(media);
        }

        private void _upButton_Clicked(object sender, EventArgs e)
        {
            try
            { 
                string str = "up";
                byte[] data = Encoding.Unicode.GetBytes(str);
            client = new TcpClient();
                client.Connect(ipServer, 8082);
                NetworkStream stream = client.GetStream();
                stream.Write(data, 0, data.Length);
                stream.Close();
                client.Close();
            }
            catch { }
        }

        private void _enterButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                string str = "enter";
                byte[] data = Encoding.Unicode.GetBytes(str);
                client = new TcpClient();
                client.Connect(ipServer, 8082);
                NetworkStream stream = client.GetStream();
                stream.Write(data, 0, data.Length);
                stream.Close();
                client.Close();
            }
            catch { }
        }

        private void _downButton_Clicked(object sender, EventArgs e)
        {
            try
            { 
            string str = "down";
            byte[] data = Encoding.Unicode.GetBytes(str);
            client = new TcpClient();
            client.Connect(ipServer, 8082);
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
    }
}