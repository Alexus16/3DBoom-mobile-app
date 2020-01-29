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
    public partial class Page8 : ContentPage
    {
        public Page8()
        {
            InitializeComponent();
        }

        private async void _backButton_Clicked(object sender, EventArgs e)
        {
            _backButton.IsEnabled = false;
            await Navigation.PopModalAsync(false);
            _backButton.IsEnabled = true;
        }

        private async void ImageButton_Clicked(object sender, EventArgs e)
        {
            ImageButton tempButton = sender as ImageButton;
            Grid tempGrid = tempButton.Parent as Grid;
            Label tempLabel = tempGrid.Children[1] as Label;
            int temp = Convert.ToInt32(tempLabel.Text.Split(':')[1]);
            await Navigation.PushModalAsync(new Page2(temp), false);
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            mainField.Content = new TableView();
            TableRoot tempRoot = new TableRoot();
            TableSection tempSection = new TableSection();
            ViewCell tempCell;
            for (int i = 0; i < DataBase.sensors.Count; i++)
            {
                string tempText = DataBase.sensors[i].Name;
                string tempId = DataBase.sensors[i].id.ToString();
                int tempValue = DataBase.sensors[i].Value;
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
                Label tempLabel = new Label() { Text = Convert.ToString(tempValue), FontSize = 20, HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center };
                grid.Children.Add(tempLabel, 3, 0);
                ImageButton tempButton = new ImageButton() { Source = "mySettings_small.png", BackgroundColor = Color.White, HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center };
                tempButton.Clicked += TempButton_Clicked;
                grid.Children.Add(tempButton, 2, 0);
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

        
        private async void TempButton_Clicked(object sender, EventArgs e)
        {
            ImageButton tempButton = sender as ImageButton;
            Grid tempGrid = tempButton.Parent as Grid;
            Label tempLabel = tempGrid.Children[1] as Label;
            int temp = Convert.ToInt32(tempLabel.Text.Split(':')[1]);
            await Navigation.PushModalAsync(new Page11(temp), false);
        }
        private async void _refreshButton_Clicked(object sender, EventArgs e)
        {
            _refreshButton.IsEnabled = false;
            await DataBase.refreshData();
            mainField.Content = new TableView();
            TableRoot tempRoot = new TableRoot();
            TableSection tempSection = new TableSection();
            ViewCell tempCell;
            for (int i = 0; i < DataBase.sensors.Count; i++)
            {
                string tempText = DataBase.sensors[i].Name;
                string tempId = DataBase.sensors[i].id.ToString();
                int tempValue = DataBase.sensors[i].Value;
                Grid grid = new Grid
                {
                    ColumnDefinitions =
                    {
                        new ColumnDefinition{Width = GridLength.Star },
                        new ColumnDefinition{Width = GridLength.Auto },
                        new ColumnDefinition{Width = 60 }
                    }
                };
                grid.Children.Add(new Label { HorizontalOptions = LayoutOptions.Start, VerticalOptions = LayoutOptions.Center, Text = tempText, FontSize = 25 }, 0, 0);
                grid.Children.Add(new Label { HorizontalOptions = LayoutOptions.Center, VerticalOptions = LayoutOptions.Center, Text = "id:" + tempId, FontSize = 10, TextColor = Color.Gray }, 1, 0);
                Label tempLabel = new Label() { Text = Convert.ToString(tempValue), FontSize = 20, VerticalOptions=LayoutOptions.Center, HorizontalOptions=LayoutOptions.Center };
                grid.Children.Add(tempLabel, 2, 0);
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