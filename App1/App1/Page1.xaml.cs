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
    public partial class Page1 : ContentPage
    {
        public Page1()
        {
            InitializeComponent();
        }

        private async void _backButton_Clicked(object sender, EventArgs e)
        {
             await Navigation.PopModalAsync(false);
        }

        private async void ImageButton_Clicked(object sender, EventArgs e)
        {
            ImageButton tempButton = sender as ImageButton;
            Grid tempGrid = tempButton.Parent as Grid;
            Label tempLabel = tempGrid.Children[1] as Label;
            int temp = Convert.ToInt32(tempLabel.Text.Split(':')[1]);
            await Navigation.PushModalAsync(new Page2(temp),false);
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            mainField.Content = new TableView();
            TableRoot tempRoot = new TableRoot();
            TableSection tempSection = new TableSection();
            ViewCell tempCell;
            for(int i = 0; i < DataBase.lamps.Count; i++)
            {
                string tempText = DataBase.lamps[i].Name;
                string tempId = DataBase.lamps[i].id.ToString();
                bool tempIsAuto = DataBase.lamps[i].isAuto;
                bool tempValue = Convert.ToBoolean(DataBase.lamps[i].curValue);
                Grid grid = new Grid
                {
                    ColumnDefinitions =
                    {
                        new ColumnDefinition{Width = GridLength.Star },
                        new ColumnDefinition{Width = GridLength.Auto },
                        new ColumnDefinition{Width = 40 },
                        new ColumnDefinition{Width = 60 }
                    }
                };
                grid.Children.Add(new Label { HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Center, Text = tempText, FontSize=25 },0,0);
                grid.Children.Add(new Label { HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center, Text = "id:"+tempId, FontSize = 10, TextColor=Color.Gray }, 1, 0);
                ImageButton tempImgButton = new ImageButton { HorizontalOptions = LayoutOptions.Center, BackgroundColor=Color.White, VerticalOptions = LayoutOptions.Center, Source = "mySettings_small.png" };
                tempImgButton.Clicked += ImageButton_Clicked;
                grid.Children.Add(tempImgButton, 2, 0);
                Switch tempSwitch = new Switch { HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center, IsEnabled = !tempIsAuto, IsToggled = tempValue };
                tempSwitch.Toggled += TempSwitch_Toggled;
                grid.Children.Add(tempSwitch, 3, 0);
                tempCell = new ViewCell
                {
                    View = grid
                };
                tempSection.Add(tempCell);
            }
            tempRoot.Add(tempSection);
            TableView tempView = mainField.Content as TableView;
            tempView.Root = tempRoot;
        }

        private async void TempSwitch_Toggled(object sender, ToggledEventArgs e)
        {
           // throw new NotImplementedException();
            Switch tempSwitch = sender as Switch;
            Grid tempGrid = tempSwitch.Parent as Grid;
            Label tempLabel = tempGrid.Children[1] as Label;
            int temp = Convert.ToInt32(tempLabel.Text.Split(':')[1]);
            bool newTempVal = tempSwitch.IsToggled;
            DataBase.findLampById(temp).curValue = newTempVal ? 1 : 0;
            await DataBase.updateData(temp, "Lamps", "Value", newTempVal?"1":"0");
        }

        private async void _refreshButton_Clicked(object sender, EventArgs e)
        {
            _refreshButton.IsEnabled = false;
            await DataBase.refreshData();
            mainField.Content = new TableView();
            TableRoot tempRoot = new TableRoot();
            TableSection tempSection = new TableSection();
            ViewCell tempCell;
            for (int i = 0; i < DataBase.lamps.Count; i++)
            {
                string tempText = DataBase.lamps[i].Name;
                string tempId = DataBase.lamps[i].id.ToString();
                bool tempIsAuto = DataBase.lamps[i].isAuto;
                bool tempValue = Convert.ToBoolean(DataBase.lamps[i].curValue);
                Grid grid = new Grid
                {
                    ColumnDefinitions =
                    {
                        new ColumnDefinition{Width = GridLength.Star },
                        new ColumnDefinition{Width = GridLength.Auto },
                        new ColumnDefinition{Width = 40 },
                        new ColumnDefinition{Width = 60 }
                    }
                };
                grid.Children.Add(new Label { HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Center, Text = tempText, FontSize = 25 }, 0, 0);
                grid.Children.Add(new Label { HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center, Text = "id:" + tempId, FontSize = 10, TextColor = Color.Gray }, 1, 0);
                ImageButton tempImgButton = new ImageButton { HorizontalOptions = LayoutOptions.Center, BackgroundColor = Color.White, VerticalOptions = LayoutOptions.Center, Source = "mySettings_small.png" };
                tempImgButton.Clicked += ImageButton_Clicked;
                grid.Children.Add(tempImgButton, 2, 0);
                Switch tempSwitch = new Switch { HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center, IsEnabled = !tempIsAuto, IsToggled = tempValue };
                tempSwitch.Toggled += TempSwitch_Toggled;
                grid.Children.Add(tempSwitch, 3, 0);
                tempCell = new ViewCell
                {
                    View = grid
                };
                tempSection.Add(tempCell);
            }
            tempRoot.Add(tempSection);
            TableView tempView = mainField.Content as TableView;
            tempView.Root = tempRoot;
            _refreshButton.IsEnabled = true;
        }
    }
}