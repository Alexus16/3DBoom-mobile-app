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
    public partial class Page11 : ContentPage
    {
        Sensor lampOfThisPage;
        int __id;
        public Page11(int _id)
        {
            InitializeComponent();
            __id = _id;
            lampOfThisPage = DataBase.findSensorById(_id);
        }
        private async void _backButton_Clicked(object sender, EventArgs e)
        {
            _backButton.IsEnabled = false;
            await Navigation.PopModalAsync(false);
            _backButton.IsEnabled = true;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            name.Text = lampOfThisPage.Name;
            setViews();
        }

        private void setViews()
        {
            nameEditor.Text = lampOfThisPage.Name;
            name.Text = lampOfThisPage.Name;
        }

        private async void _refreshButton_Clicked(object sender, EventArgs e)
        {
            _refreshButton.IsEnabled = false;
            await DataBase.refreshData();
            lampOfThisPage = DataBase.findSensorById(__id);
            setViews();
            _refreshButton.IsEnabled = true;
        }

        private async void saveName_Clicked(object sender, EventArgs e)
        {
            saveName.IsEnabled = false;
            lampOfThisPage.Name = nameEditor.Text;
            await DataBase.updateData(lampOfThisPage.id, "Sensors", "Name", "'" + lampOfThisPage.Name + "'");
            setViews();
            saveName.IsEnabled = true;
        }
    }
}