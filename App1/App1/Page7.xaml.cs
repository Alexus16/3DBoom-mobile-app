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
            ipServer = "192.168.10.220";
            webView1.Source = "http://192.168.10.220/";
            webView1.HorizontalOptions = LayoutOptions.Center;
        }

        private void _upButton_Clicked(object sender, EventArgs e)
        {
            try
            { 
                string str = "Left";
                byte[] data = Encoding.Unicode.GetBytes(str);
                client = new TcpClient();
                client.Connect(ipServer, 1234);
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
                string str = "Click";
                byte[] data = Encoding.Unicode.GetBytes(str);
                client = new TcpClient();
                client.Connect(ipServer, 1234);
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
            string str = "Right";
            byte[] data = Encoding.Unicode.GetBytes(str);
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
    }
}