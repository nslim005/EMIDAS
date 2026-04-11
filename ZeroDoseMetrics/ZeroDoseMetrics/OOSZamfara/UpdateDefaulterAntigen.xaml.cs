using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging.Abstractions;
using SQLite;
using Xamarin.Forms;
using ZeroDoseMetrics.Model;

namespace ZeroDoseMetrics.OOSZamfara
{	
	public partial class UpdateDefaulterAntigen : ContentPage
	{
        public DefaulterList helperList;

		public UpdateDefaulterAntigen (DefaulterList list)
		{
			InitializeComponent ();
            this.helperList = list;

            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                int id = list.Id;

                conn.CreateTable<DefaulterList>();
                var record = conn.Table<DefaulterList>().Where(x => x.Id == id).FirstOrDefault();
                ChildNameEntry.Text = record.ChildName;
                CaregiverEntry.Text = record.CaregiverName;
                ChildIDEntry.Text = record.ChildID;

            }
        }

        void AgePicker_SelectedIndexChanged(System.Object sender, System.EventArgs e)
        {
            if (AgePickerCurrent.SelectedItem == null)
                //return; // or show a message
                DisplayAlert("ERROR", "SELECT CHILD CURRENT AGE", "OK");
            string selectedCurrentAge = AgePickerCurrent.SelectedItem.ToString();
            if (selectedCurrentAge == "Birth - 5 weeks")
            {
                OPVStackL.IsVisible = true;
                HepBStack.IsVisible = true;
                BCGStack.IsVisible = true;

                //set others false
                PentaStack.IsVisible = false;
                PCVStack.IsVisible = false;
                RotaStack.IsVisible = false;
                IPVStack.IsVisible = false;
                MeaslesStack.IsVisible = false;
                YFStack.IsVisible = false;
                MenAStack.IsVisible = false;
            }
            else if (selectedCurrentAge == "6 weeks – 9 weeks")
            {
                OPVStackL.IsVisible = true;
                HepBStack.IsVisible = true;
                BCGStack.IsVisible = true;
                PentaStack.IsVisible = true;
                PCVStack.IsVisible = true;
                RotaStack.IsVisible = true;
                IPVStack.IsVisible = true;

                // set false
                MeaslesStack.IsVisible = false;
                YFStack.IsVisible = false;
                MenAStack.IsVisible = false;

            }
            else if (selectedCurrentAge == "10 weeks – 13 weeks")
            {
                OPVStackL.IsVisible = true;
                HepBStack.IsVisible = true;
                BCGStack.IsVisible = true;
                PentaStack.IsVisible = true;
                PCVStack.IsVisible = true;
                RotaStack.IsVisible = true;
                IPVStack.IsVisible = true;

                // set false
                MeaslesStack.IsVisible = false;
                YFStack.IsVisible = false;
                MenAStack.IsVisible = false;
            }
            else if (selectedCurrentAge == "14 weeks – 8 months")
            {
                OPVStackL.IsVisible = true;
                HepBStack.IsVisible = true;
                BCGStack.IsVisible = true;
                PentaStack.IsVisible = true;
                PCVStack.IsVisible = true;
                RotaStack.IsVisible = true;
                IPVStack.IsVisible = true;

                // set false
                MeaslesStack.IsVisible = false;
                YFStack.IsVisible = false;
                MenAStack.IsVisible = false;
            }
            else if (selectedCurrentAge == "9 months – 11 months")
            {
                OPVStackL.IsVisible = true;
                HepBStack.IsVisible = true;
                BCGStack.IsVisible = true;
                PentaStack.IsVisible = true;
                PCVStack.IsVisible = true;
                RotaStack.IsVisible = true;
                IPVStack.IsVisible = true;
                MeaslesStack.IsVisible = true;
                YFStack.IsVisible = true;
                MenAStack.IsVisible = true;
            }
            else if (selectedCurrentAge == "12 months – 14 months")
            {
                OPVStackL.IsVisible = true;
                HepBStack.IsVisible = true;
                BCGStack.IsVisible = true;
                PentaStack.IsVisible = true;
                PCVStack.IsVisible = true;
                RotaStack.IsVisible = true;
                IPVStack.IsVisible = true;
                MeaslesStack.IsVisible = true;
                YFStack.IsVisible = true;
                MenAStack.IsVisible = true;
            }
            else if (selectedCurrentAge == "15 months – 23 months")
            {
                OPVStackL.IsVisible = true;
                BCGStack.IsVisible = true;
                PentaStack.IsVisible = true;
                PCVStack.IsVisible = true;
                RotaStack.IsVisible = true;
                IPVStack.IsVisible = true;
                MeaslesStack.IsVisible = true;
                YFStack.IsVisible = true;
                MenAStack.IsVisible = true;

                //set false
                HepBStack.IsVisible = false;
            }
        }

        private string AllAdministeredAntigens()
        {
            bool isHepBChecked = HepB0.IsChecked;
            bool isBCGChecked = BCG.IsChecked;
            bool isYFChecked = YF.IsChecked;
            bool isMENAChecked = MENA.IsChecked;

            string selectedOptions = string.Empty;


            if (isHepBChecked) selectedOptions += "HepB | ";
            if (isBCGChecked) selectedOptions += "BCG | ";
            if (isYFChecked) selectedOptions += "YF | ";
            if (isMENAChecked) selectedOptions += "MENA | ";
            if (PENTATypes.SelectedIndex != -1) { selectedOptions += PENTATypes.SelectedItem.ToString() + " |"; };
            if (MeaslesTypes.SelectedIndex != -1) { selectedOptions += MeaslesTypes.SelectedItem.ToString() + " |"; };
            if (PCVTypes.SelectedIndex != -1) { selectedOptions += PCVTypes.SelectedItem.ToString() + " |"; };
            if (ROTATypes.SelectedIndex != -1) { selectedOptions += ROTATypes.SelectedItem.ToString() + " |"; };
            if (IPVTypes.SelectedIndex != -1) { selectedOptions += IPVTypes.SelectedItem.ToString() + " |"; };
            if (OPVTypes.SelectedIndex != -1) { selectedOptions += OPVTypes.SelectedItem.ToString() + " |"; };


            return selectedOptions;
        }

        void RespondentEnty_SelectedIndexChanged(System.Object sender, System.EventArgs e)
        {
        }

        void Submit_Clicked(System.Object sender, System.EventArgs e)
        {

            

            string administeredAntigens = AllAdministeredAntigens();
            string age = AgePickerCurrent.SelectedIndex.ToString();

            if (administeredAntigens == "" || string.IsNullOrEmpty(age))
            {

                DisplayAlert("ERROR", "SELECT ANTIGEN BEFORE YOU PROCEED", "OK");
            }

            else
            {

                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {

                    conn.CreateTable<DefaulterList>();
                    DefaulterList record = conn.Table<DefaulterList>().Where(x => x.Id.Equals(helperList.Id)).FirstOrDefault();
                    record.AntigensReceived = record.AntigensReceived +""+AllAdministeredAntigens().Trim();
                    record.AgeCategory = AgePickerCurrent.SelectedItem.ToString().Trim();
                    record.Completed = 11;

                    //if (record.AntigensReceived.Contains("BCG") ||
                    //    record.AntigensReceived.Contains("HepB 0") ||
                    //    record.AntigensReceived.Contains("OPV") ||
                    //    record.AntigensReceived.Contains("PENTA") ||
                    //    record.AntigensReceived.Contains("PCV") ||
                    //    record.AntigensReceived.Contains("ROTA") ||
                    //    record.AntigensReceived.Contains("IPV") ||
                    //    record.AntigensReceived.Contains("") ||
                    //    record.AntigensReceived.Contains("") ||
                    //    record.AntigensReceived.Contains("") ||
                    //    record.AntigensReceived.Contains("") ||
                    //    record.AntigensReceived.Contains("") ||
                    //    record.AntigensReceived.Contains("") ||
                    //    record.AntigensReceived.Contains("") ||
                    //    record.AntigensReceived.Contains("") ||
                    //    record.AntigensReceived.Contains("") ||
                    //    record.AntigensReceived.Contains("") ||)

                    //{
                    //    record.Completed = 12;
                    //}
                    //else
                    //{
                    //    record.Completed = 11;
                    //}

                    int rows = conn.Update(record);

                    if (rows > 0)
                    {
                        DisplayAlert("Success", "Vaccination Record Saved", "OK");
                        //Navigation.PushAsync(new ChildrenLineListPage(helper));
                        Navigation.PopAsync();
                    }
                    else
                    {
                        DisplayAlert("Failure", "Error Saving Vaccination Record", "OK");
                        Navigation.PushAsync(new UpdateDefaulterAntigen(record));
                    }
                }
            }

            //vcalidation
        }


        void PENTA_CheckedChanged(System.Object sender, Xamarin.Forms.CheckedChangedEventArgs e)
        {
            var item = sender as CheckBox;

            if (item.IsChecked)
            {
                PENTATypes.SelectedIndex = -1;
                PENTATypes.IsVisible = true;
            }
            if (!item.IsChecked)
            {
                PENTATypes.SelectedIndex = -1;
                PENTATypes.IsVisible = false;
            }

        }

        void Measles_CheckedChanged(System.Object sender, Xamarin.Forms.CheckedChangedEventArgs e)
        {
            var item = sender as CheckBox;

            if (item.IsChecked)
            {
                MeaslesTypes.SelectedIndex = -1;
                MeaslesTypes.IsVisible = true;
            }
            if (!item.IsChecked)
            {
                MeaslesTypes.SelectedIndex = -1;
                MeaslesTypes.IsVisible = false;
            }
        }

        void PCV_CheckedChanged(System.Object sender, Xamarin.Forms.CheckedChangedEventArgs e)
        {
            var item = sender as CheckBox;

            if (item.IsChecked)
            {
                PCVTypes.SelectedIndex = -1;
                PCVTypes.IsVisible = true;
            }
            if (!item.IsChecked)
            {
                PCVTypes.SelectedIndex = -1;
                PCVTypes.IsVisible = false;
            }
        }

        void IPV_CheckedChanged(System.Object sender, Xamarin.Forms.CheckedChangedEventArgs e)
        {
            var item = sender as CheckBox;

            if (item.IsChecked)
            {
                IPVTypes.SelectedIndex = -1;
                IPVTypes.IsVisible = true;
            }
            if (!item.IsChecked)
            {
                IPVTypes.SelectedIndex = -1;
                IPVTypes.IsVisible = false;
            }
        }

        void OPV_CheckedChanged(System.Object sender, Xamarin.Forms.CheckedChangedEventArgs e)
        {
            var item = sender as CheckBox;

            if (item.IsChecked)
            {
                OPVTypes.SelectedIndex = -1;
                OPVTypes.IsVisible = true;
            }
            if (!item.IsChecked)
            {
                OPVTypes.SelectedIndex = -1;
                OPVTypes.IsVisible = false;
            }
        }

        void ROTA_CheckedChanged(System.Object sender, Xamarin.Forms.CheckedChangedEventArgs e)
        {
            var item = sender as CheckBox;

            if (item.IsChecked)
            {
                ROTATypes.SelectedIndex = -1;
                ROTATypes.IsVisible = true;
            }
            if (!item.IsChecked)
            {
                ROTATypes.SelectedIndex = -1;
                ROTATypes.IsVisible = false;
            }
        }

        void IPV_CheckedChanged_1(System.Object sender, Xamarin.Forms.CheckedChangedEventArgs e)
        {
            var item = sender as CheckBox;

            if (item.IsChecked)
            {
                IPVTypes.SelectedIndex = -1;
                IPVTypes.IsVisible = true;
            }
            if (!item.IsChecked)
            {
                IPVTypes.SelectedIndex = -1;
                IPVTypes.IsVisible = false;
            }
        }

        void Measles_CheckedChanged_1(System.Object sender, Xamarin.Forms.CheckedChangedEventArgs e)
        {
            var item = sender as CheckBox;

            if (item.IsChecked)
            {
                MeaslesTypes.SelectedIndex = -1;
                MeaslesTypes.IsVisible = true;
            }
            if (!item.IsChecked)
            {
                MeaslesTypes.SelectedIndex = -1;
                MeaslesTypes.IsVisible = false;
            }
        }


        void setStacks()
        {
            if (AgePickerCurrent.SelectedItem == null)
                //return; // or show a message
                DisplayAlert("ERROR", "SELECT CHILD CURRENT AGE", "OK");
            string selectedCurrentAge = AgePickerCurrent.SelectedItem.ToString();
            if (selectedCurrentAge == "Birth - 5 weeks")
            {
                OPVStackL.IsVisible = true;
                HepBStack.IsVisible = true;
                BCGStack.IsVisible = true;

                //set others false
                PentaStack.IsVisible = false;
                PCVStack.IsVisible = false;
                RotaStack.IsVisible = false;
                IPVStack.IsVisible = false;
                MeaslesStack.IsVisible = false;
                YFStack.IsVisible = false;
                MenAStack.IsVisible = false;


            }
            else if (selectedCurrentAge == "6 weeks – 9 weeks")
            {
                OPVStackL.IsVisible = true;
                HepBStack.IsVisible = true;
                BCGStack.IsVisible = true;
                PentaStack.IsVisible = true;
                PCVStack.IsVisible = true;
                RotaStack.IsVisible = true;
                IPVStack.IsVisible = true;

                // set false
                MeaslesStack.IsVisible = false;
                YFStack.IsVisible = false;
                MenAStack.IsVisible = false;

            }
            else if (selectedCurrentAge == "10 weeks – 13 weeks")
            {
                OPVStackL.IsVisible = true;
                HepBStack.IsVisible = true;
                BCGStack.IsVisible = true;
                PentaStack.IsVisible = true;
                PCVStack.IsVisible = true;
                RotaStack.IsVisible = true;
                IPVStack.IsVisible = true;

                // set false
                MeaslesStack.IsVisible = false;
                YFStack.IsVisible = false;
                MenAStack.IsVisible = false;
            }
            else if (selectedCurrentAge == "14 weeks – 8 months")
            {
                OPVStackL.IsVisible = true;
                HepBStack.IsVisible = true;
                BCGStack.IsVisible = true;
                PentaStack.IsVisible = true;
                PCVStack.IsVisible = true;
                RotaStack.IsVisible = true;
                IPVStack.IsVisible = true;

                // set false
                MeaslesStack.IsVisible = false;
                YFStack.IsVisible = false;
                MenAStack.IsVisible = false;
            }
            else if (selectedCurrentAge == "9 months – 11 months")
            {
                OPVStackL.IsVisible = true;
                HepBStack.IsVisible = true;
                BCGStack.IsVisible = true;
                PentaStack.IsVisible = true;
                PCVStack.IsVisible = true;
                RotaStack.IsVisible = true;
                IPVStack.IsVisible = true;
                MeaslesStack.IsVisible = true;
                YFStack.IsVisible = true;
                MenAStack.IsVisible = true;
            }
            else if (selectedCurrentAge == "12 months – 14 months")
            {
                OPVStackL.IsVisible = true;
                HepBStack.IsVisible = true;
                BCGStack.IsVisible = true;
                PentaStack.IsVisible = true;
                PCVStack.IsVisible = true;
                RotaStack.IsVisible = true;
                IPVStack.IsVisible = true;
                MeaslesStack.IsVisible = true;
                YFStack.IsVisible = true;
                MenAStack.IsVisible = true;
            }
            else if (selectedCurrentAge == "15 months – 23 months")
            {
                OPVStackL.IsVisible = true;
                BCGStack.IsVisible = true;
                PentaStack.IsVisible = true;
                PCVStack.IsVisible = true;
                RotaStack.IsVisible = true;
                IPVStack.IsVisible = true;
                MeaslesStack.IsVisible = true;
                YFStack.IsVisible = true;
                MenAStack.IsVisible = true;

                //set false
                HepBStack.IsVisible = false;
            }
        }

    }
}

