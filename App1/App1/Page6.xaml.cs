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
    public partial class Page6 : ContentPage
    {
        Executor executorOfThisPage;
        int __id;
        public Page6(int _id)
        {
            InitializeComponent();
            __id = _id;
            executorOfThisPage = DataBase.findExecutorById(_id);
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
            name.Text = executorOfThisPage.Name;
            enableSwitch.IsEnabled = !executorOfThisPage.isAuto;
            enableSwitch.IsToggled = Convert.ToBoolean(executorOfThisPage.curValue);
            autoSwitch.IsToggled = executorOfThisPage.isAuto;
            setViews();
            refreshTable();
        }

        private async void autoSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            executorOfThisPage.isAuto = autoSwitch.IsToggled;
            setViews();
            await DataBase.updateData(executorOfThisPage.id, "Executors", "Auto", autoSwitch.IsToggled ? "true" : "false");

        }

        private async void enableSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            executorOfThisPage.curValue = enableSwitch.IsToggled ? 1 : 0;
            setViews();
            await DataBase.updateData(executorOfThisPage.id, "Executors", "Value", enableSwitch.IsToggled ? "1" : "0");

        }
        private void setViews()
        {
            nameEditor.Text = executorOfThisPage.Name;
            name.Text = executorOfThisPage.Name;
            enableSwitch.IsEnabled = !executorOfThisPage.isAuto;
            enableSwitch.IsToggled = Convert.ToBoolean(executorOfThisPage.curValue);
            autoSwitch.IsToggled = executorOfThisPage.isAuto;
        }

        private async void buttonMinus_Clicked(object sender, EventArgs e)
        {
            buttonMinus.IsEnabled = false;
            List<string> names = new List<string>();
            for (int i = 0; i < executorOfThisPage.conf.items.Count; i++)
            {
                if (!executorOfThisPage.conf.items[i].isDaughter)
                { names.Add(executorOfThisPage.conf.items[i].getName() + ":" + executorOfThisPage.conf.items[i].id); }
            }
            string[] namesArr = names.ToArray();
            string action = await DisplayActionSheet("Item to delete", "Cancel", null, namesArr);
            if (action != "Cancel")
            {
                string deleteType = await DisplayActionSheet("Delete daughter items?", "Cancel", null, "Yes", "No");

                int tempId = Convert.ToInt32(action.Split(':')[1]);
                if (deleteType.Contains("Yes"))
                {
                    executorOfThisPage.conf.deleteItemWithDaughtersWithID(tempId);
                }
                else if (deleteType.Contains("No"))
                {
                    executorOfThisPage.conf.deleteItemWithID(tempId);
                }
                else { }

            }
            refreshTable();
            buttonMinus.IsEnabled = true;
        }
        private void refreshTable()
        {
            TableView table = configuratorTable;
            TableRoot tempRoot = new TableRoot();
            TableSection tempSection = new TableSection();
            for (int i = 0; i < executorOfThisPage.conf.items.Count; i++)
            {
                if (!executorOfThisPage.conf.items[i].isDaughter)
                {
                    ViewCell tempCell = new ViewCell
                    {
                        View = new Label
                        {
                            Text = executorOfThisPage.conf.items[i].getName(),
                            VerticalOptions = LayoutOptions.Center,
                            HorizontalOptions = LayoutOptions.Center,
                            FontSize = 20
                        }
                    };
                    tempSection.Add(tempCell);
                }
            }
            tempRoot.Add(tempSection);
            table.Root = tempRoot;
        }

        private async void buttonPlus_Clicked(object sender, EventArgs e)
        {
            buttonPlus.IsEnabled = false;
            List<string> names = new List<string>();
            for (int i = 0; i < executorOfThisPage.conf.items.Count; i++)
            {
                if (!executorOfThisPage.conf.items[i].isDaughter)
                { names.Add(executorOfThisPage.conf.items[i].getName() + ":" + executorOfThisPage.conf.items[i].id); }
            }
            string[] namesArr = names.ToArray();
            string action = await DisplayActionSheet("What type of element", "Cancel", null, "Sensor", "Constant", "Link");
            if (action == "Sensor")
            {
                string tempName;
                List<string> namesSensor = new List<string>();
                for (int i = 0; i < DataBase.sensors.Count; i++)
                {
                    namesSensor.Add(DataBase.sensors[i].Name + ":" + DataBase.sensors[i].id);
                }
                string[] namesArrSensor = namesSensor.ToArray();
                tempName = await DisplayActionSheet("What sensor", "Cancel", null, namesArrSensor);
                if (tempName != "Cancel")
                {
                    int tempId = Convert.ToInt32(tempName.Split(':')[1]);
                    executorOfThisPage.conf.addItemSensor(tempId);
                }
            }
            else if (action == "Constant")
            {
                string tempStr;
                int tempValue = -1;
                do
                {
                    tempStr = await DisplayPromptAsync("Value of constant", "Enter value of constant", "OK", "Cancel");
                }
                while (tempStr != null && !int.TryParse(tempStr, out tempValue));
                if (tempStr != null)
                    executorOfThisPage.conf.addItemConst(tempValue);
            }
            else if (action == "Link")
            {
                string[] actions = { "And", "Or", "Not", "+", "-", "/", "*", "=", ">", "<", ">=", "<=" };
                string typeLink = await DisplayActionSheet("Type of link", "Cancel", null, actions);
                int numberLink = -1;
                for (int i = 0; i < actions.Length; i++)
                {
                    if (actions[i] == typeLink)
                    {
                        numberLink = i;
                        break;
                    }
                }
                if (numberLink != -1)
                {
                    int valueOfElements;

                    if (numberLink == 2)
                        valueOfElements = 1;
                    else
                        valueOfElements = 2;

                    if (valueOfElements == 1)
                    {
                        string element = await DisplayActionSheet("Item", "Cancel", null, namesArr);
                        if (element != "Cancel")
                        {
                            int tempId = Convert.ToInt32(element.Split(':')[1]);
                            executorOfThisPage.conf.addItemLink(tempId, numberLink);
                        }
                    }
                    else
                    {
                        string element1 = await DisplayActionSheet("First item", "Cancel", null, namesArr);
                        if (element1 != "Cancel")
                        {
                            int tempId1 = Convert.ToInt32(element1.Split(':')[1]);
                            string element2 = await DisplayActionSheet("Second item", "Cancel", null, namesArr);
                            if (element2 != "Cancel")
                            {
                                int tempId2 = Convert.ToInt32(element2.Split(':')[1]);
                                executorOfThisPage.conf.addItemLink(tempId1, tempId2, numberLink);
                            }
                        }
                    }
                }
            }
            refreshTable();
            buttonPlus.IsEnabled = true;
        }

        private async void saveButton_Clicked(object sender, EventArgs e)
        {
            saveButton.IsEnabled = false;
            List<string> names = new List<string>();
            for (int i = 0; i < executorOfThisPage.conf.items.Count; i++)
            {
                if (!executorOfThisPage.conf.items[i].isDaughter)
                { names.Add(executorOfThisPage.conf.items[i].getName() + ":" + executorOfThisPage.conf.items[i].id); }
            }
            string[] namesArr = names.ToArray();
            string selectedItem = await DisplayActionSheet("Select result item", "Cancel", null, namesArr);
            if (selectedItem != "Cancel")
            {
                int tempId = Convert.ToInt32(selectedItem.Split(':')[1]);
                executorOfThisPage.conf.setResultId(tempId);
                await DataBase.updateData(executorOfThisPage.id, "Executors", "Conf", executorOfThisPage.conf.getConfString());
            }
            saveButton.IsEnabled = true;
        }

        private async void _refreshButton_Clicked(object sender, EventArgs e)
        {
            _refreshButton.IsEnabled = false;
            await DataBase.refreshData();
            executorOfThisPage = DataBase.findExecutorById(__id);
            refreshTable();
            setViews();
            _refreshButton.IsEnabled = true;
        }
        private async void saveName_Clicked(object sender, EventArgs e)
        {
            saveName.IsEnabled = false;
            executorOfThisPage.Name = nameEditor.Text;
            await DataBase.updateData(executorOfThisPage.id, "Executors", "Name", "'" + executorOfThisPage.Name + "'");
            setViews();
            refreshTable();
            saveName.IsEnabled = true;
        }
    }
}