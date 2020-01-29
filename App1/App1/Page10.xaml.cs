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
    public partial class Page10 : ContentPage
    {
        Window windowOfThisPage;
        int __id;
        public Page10(int _id)
        {
            InitializeComponent();
            __id = _id;
            windowOfThisPage = DataBase.findWindowById(_id);
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
            name.Text = windowOfThisPage.Name;
            Val.IsEnabled = !windowOfThisPage.isAuto;
            Val.Value = windowOfThisPage.curValueOfMotor;
            autoSwitch.IsToggled = windowOfThisPage.isAuto;
            minVal.Value = windowOfThisPage.minValueOfMotor;
            maxVal.Value = windowOfThisPage.maxValueOfMotor;
            setViews();
            refreshTable();
        }

        private async void autoSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            windowOfThisPage.isAuto = autoSwitch.IsToggled;
            setViews();
            await DataBase.updateData(windowOfThisPage.id, "Windows", "Auto", autoSwitch.IsToggled ? "true" : "false");
        }

        private void setViews()
        {
            nameEditor.Text = windowOfThisPage.Name;
            name.Text = windowOfThisPage.Name;
            maxVal.Value = windowOfThisPage.maxValueOfMotor;
            minVal.Value = windowOfThisPage.minValueOfMotor;
            Val.IsEnabled = !windowOfThisPage.isAuto;
            Val.Value = windowOfThisPage.curValueOfMotor;
            autoSwitch.IsToggled = windowOfThisPage.isAuto;
            minValL.Text = minVal.Value.ToString();
            maxValL.Text = maxVal.Value.ToString();
            valL.Text = Val.Value.ToString();
        }

        private async void buttonMinus_Clicked(object sender, EventArgs e)
        {
            buttonMinus.IsEnabled = false;
            List<string> names = new List<string>();
            for (int i = 0; i < windowOfThisPage.conf.items.Count; i++)
            {
                if (!windowOfThisPage.conf.items[i].isDaughter)
                { names.Add(windowOfThisPage.conf.items[i].getName() + ":" + windowOfThisPage.conf.items[i].id); }
            }
            string[] namesArr = names.ToArray();
            string action = await DisplayActionSheet("Item to delete", "Cancel", null, namesArr);
            if (action != "Cancel")
            {
                string deleteType = await DisplayActionSheet("Delete daughter items?", "Cancel", null, "Yes", "No");

                int tempId = Convert.ToInt32(action.Split(':')[1]);
                if (deleteType.Contains("Yes"))
                {
                    windowOfThisPage.conf.deleteItemWithDaughtersWithID(tempId);
                }
                else if (deleteType.Contains("No"))
                {
                    windowOfThisPage.conf.deleteItemWithID(tempId);
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
            for (int i = 0; i < windowOfThisPage.conf.items.Count; i++)
            {
                if (!windowOfThisPage.conf.items[i].isDaughter)
                {
                    ViewCell tempCell = new ViewCell
                    {
                        View = new Label
                        {
                            Text = windowOfThisPage.conf.items[i].getName(),
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
            for (int i = 0; i < windowOfThisPage.conf.items.Count; i++)
            {
                if (!windowOfThisPage.conf.items[i].isDaughter)
                { names.Add(windowOfThisPage.conf.items[i].getName() + ":" + windowOfThisPage.conf.items[i].id); }
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
                    windowOfThisPage.conf.addItemSensor(tempId);
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
                    windowOfThisPage.conf.addItemConst(tempValue);
            }
            else if (action == "Link")
            {
                string[] actions = { "And", "Or", "Not", "+", "-", "/", "*", "=", ">", "<", ">=", "<=" };
                string typeLink = await DisplayActionSheet("Type of link", "Cancel", null, actions);
                int numberLink = 0;
                for (int i = 0; i < actions.Length; i++)
                {
                    if (actions[i] == typeLink)
                    {
                        numberLink = i;
                        break;
                    }
                }
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
                        windowOfThisPage.conf.addItemLink(tempId, numberLink);
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
                            windowOfThisPage.conf.addItemLink(tempId1, tempId2, numberLink);
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
            for (int i = 0; i < windowOfThisPage.conf.items.Count; i++)
            {
                if (!windowOfThisPage.conf.items[i].isDaughter)
                { names.Add(windowOfThisPage.conf.items[i].getName() + ":" + windowOfThisPage.conf.items[i].id); }
            }
            string[] namesArr = names.ToArray();
            string selectedItem = await DisplayActionSheet("Select result item", "Cancel", null, namesArr);
            if (selectedItem != "Cancel")
            {
                int tempId = Convert.ToInt32(selectedItem.Split(':')[1]);
                windowOfThisPage.conf.setResultId(tempId);
                await DataBase.updateData(windowOfThisPage.id, "Windows", "Conf", windowOfThisPage.conf.getConfString());
            }
            saveButton.IsEnabled = true;
        }

        private async void _refreshButton_Clicked(object sender, EventArgs e)
        {
            _refreshButton.IsEnabled = false;
            await DataBase.refreshData();
            windowOfThisPage = DataBase.findWindowById(__id);
            refreshTable();
            setViews();
            _refreshButton.IsEnabled = true;
        }

        private async void minVal_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            minVal.IsEnabled = false;
            windowOfThisPage.minValueOfMotor = Convert.ToInt32(minVal.Value);
            await DataBase.updateData(windowOfThisPage.id, "Windows", "minValue", windowOfThisPage.minValueOfMotor);
            setViews();
            minVal.IsEnabled = true;
        }

        private async void maxVal_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            maxVal.IsEnabled = false;
            windowOfThisPage.maxValueOfMotor = Convert.ToInt32(maxVal.Value);
            await DataBase.updateData(windowOfThisPage.id, "Windows", "maxValue", windowOfThisPage.maxValueOfMotor);
            setViews();
            maxVal.IsEnabled = true;
        }

        private async void Val_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            Val.IsEnabled = false;
            windowOfThisPage.curValueOfMotor = Convert.ToInt32(Val.Value);
            await DataBase.updateData(windowOfThisPage.id, "Windows", "Value", windowOfThisPage.curValueOfMotor);
            setViews();
            Val.IsEnabled = true;
        }
        private async void saveName_Clicked(object sender, EventArgs e)
        {
            saveName.IsEnabled = false;
            windowOfThisPage.Name = nameEditor.Text;
            await DataBase.updateData(windowOfThisPage.id, "Windows", "Name", "'" + windowOfThisPage.Name + "'");
            setViews();
            refreshTable();
            saveName.IsEnabled = true;
        }
    }
}