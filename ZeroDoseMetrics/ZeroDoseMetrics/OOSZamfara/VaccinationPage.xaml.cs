using System;
using System.Collections.Generic;
using System.Globalization;
using SQLite;
using Xamarin.Essentials;
using Xamarin.Forms;
using ZeroDoseMetrics.Model;

namespace ZeroDoseMetrics.OOSZamfara
{	
	public partial class VaccinationPage : ContentPage
	{
        public Login helper { get; set; }

		public VaccinationPage (OOSList list, Login helper)
		{
			InitializeComponent ();
            GetChildRecordTally();
            this.helper = helper; 


            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                int id = list.Id;

                conn.CreateTable<OOSList>();
                var linelist = conn.Table<OOSList>().Where(x => x.Id == id).FirstOrDefault();
                //ChildrenLineList.ItemsSource = linelists;

                string time = DateTime.Now.ToString("hh:mm tt");
                string date = DateTime.Now.ToString("dddd, dd MMMM yyyy");

                vaccinatorNameEntry.Text = helper.InterviewerName;
                phoneNoEntry.Text = helper.PhoneNo;
                dateEntry.Text = date;
                timeEntry.Text = time;
                teamCodeEntry.Text = helper.TeamCode;
                lgaEntry.Text = linelist.LGA;
                catchmentAreaHFEntry.Text = linelist.CatchmentAreaHF;
                settlementNameEnty.Text = linelist.SettlementName;
                wardEntry.Text = linelist.Ward;
                childNameEnty.Text = linelist.ChildName;
                //childAgeEnty.Text = linelist.Age.ToString();
                childAgeEnty.Text = linelist.CurrentAge;
                GenderEnty.Text = linelist.Gender;
                childIDEnty.Text = linelist.ChildID.ToString();
                AntigensReceivedEnty.Text = linelist.OldAntigensReceived;
                houseHoldNameEntry.Text = linelist.HouseHoldHeadName;
                houseHoldContactEntry.Text = linelist.CaregiverNumber;
                careGiverNameEntry.Text = linelist.CaregiverName;

                //if (linelist.AntigensReceived == "")
                //{
                    
                //    HasReceivedAntigenLbl.IsVisible = true;
                //    RadioButtonGroupReceivedRIPreviously.IsVisible = true;
                   
                //}
                
            }


        }

        void save_Clicked(System.Object sender, System.EventArgs e)
        {

        }

        void PreviouslyEnumerated_CheckedChanged(System.Object sender, Xamarin.Forms.CheckedChangedEventArgs e)
        {

        }

        void DayPicker_SelectedIndexChanged(System.Object sender, System.EventArgs e)
        {
            //var picker = sender as Picker;
            //string selectedValue = string.Empty;
            //foreach (var view in RadioButtonGroupAEFIType.Children)
            //{
            //    if (view is Picker picker1 && picker1.IsSet)
            //    {
            //        // Retrieve the selected value
            //        selectedValue = radioButton.Content.ToString();

            //    }

            //}
            //return selectedValue;
        }

        void PreviouslyReceivedRI_CheckedChanged1(System.Object sender, Xamarin.Forms.CheckedChangedEventArgs e)
        {
            //var picker = sender as Picker;
            //string selectedValue = string.Empty;
            //foreach (var view in RadioButtonGroupAEFIType.Children)
            //{
            //    if (view is Picker picker1 && picker1.IsSet)
            //    {
            //        // Retrieve the selected value
            //        selectedValue = radioButton.Content.ToString();

            //    }

            //}
            //return selectedValue;
            string getIsTally = GetChildRecordTally()?.Trim();
            if(getIsTally == null)
            {
                DisplayAlert("ERROR", "PROVIDE ANSWER TO Q20", "OK");
            }
            else
            {
                if(getIsTally == "Yes")
                {
                    followupQ1.IsVisible = false;
                    HepBStack1.IsVisible = false;
                    BCGStack1.IsVisible = false;
                    PentaStack1.IsVisible = false;
                    PCVStack1.IsVisible = false;
                    RotaStack1.IsVisible = false;
                    IPVStack1.IsVisible = false;
                    MeaslesStack1.IsVisible = false;
                    YFStack1.IsVisible = false;
                    MenAStack1.IsVisible = false;
                }
                else
                {
                    // get the list of Antigens
                    followupQ1.IsVisible = true;
                    OPVStackL1.IsVisible = true;
                    HepBStack1.IsVisible = true;
                    BCGStack1.IsVisible = true;
                    PentaStack1.IsVisible = true;
                    PCVStack1.IsVisible = true;
                    RotaStack1.IsVisible = true;
                    IPVStack1.IsVisible = true;
                    MeaslesStack1.IsVisible = true;
                    YFStack1.IsVisible = true;
                    MenAStack1.IsVisible = true;

                }

            }


        }

        void AgePickerCurrent_SelectedIndexChanged(System.Object sender, System.EventArgs e)
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
                else if (selectedCurrentAge == "15 months – 23 months" || selectedCurrentAge == "24 months – 59 months")
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


        void Button_Submit(System.Object sender, System.EventArgs e)
        {
            //Begin validation

            //string previously = GetPreviouslyEnumerated();
            string selectedAge = AgePickerCurrent.SelectedIndex.ToString();
            //string receivedRI = GetPreviouslyReceivedRI();
            string recordTally = GetChildRecordTally();
            string administeredA = AllAdministeredAntigens();
            string administeredFromChildHealthCard = AllAdministeredAntigens1();
            string aefi = GetAEFI();
            string aefiType = GetAEFIType();
            string dueForNextAntigens = DueForNextAntigen.SelectedIndex.ToString();
            string vaccinationSupervisorName = vaccinatorSupervisorNameEntry.Text;

            if (followupQ.IsVisible && (administeredA == "" || aefi == ""))
            {
    
             DisplayAlert("ERROR", "ANSWER FOLLOW-UP QUESTIONS.", "OK");

            }
            else if(vaccinationSupervisorName == string.Empty)
            {
                DisplayAlert("ERROR", "FILL Vaccinator Supervisor Name", "OK");
            }
            else if (recordTally == "No" && administeredFromChildHealthCard == "")
            {
                DisplayAlert("ERROR", "FILL ALL ANTIGENS AS SEEN ON THE CHILD HEALTH CARD", "OK");
            }
            else if (aefi == "Yes" && aefiType == "")
            {
                DisplayAlert("ERROR", "SELECT AEFI TYPE", "OK");
            }
            else if (selectedAge == "-1")
            {
                DisplayAlert("ERROR", "SELECT CHILD CURRENT AGE", "OK");
            }
            else if (dueForNextAntigens == "-1")
            {
                DisplayAlert("ERROR", "ANSWER Q21", "OK");
            }
            else if (string.IsNullOrEmpty(childNameEnty.Text) || LocationLabel.Text == "Fetch location..." || LocationLabel.Text == "Location permission denied.")
            {
                DisplayAlert("ERROR", "ENSURE GEO-CORDINATE AND CHILD NAME ARE FILLED, ALSO CHECK LOCATION PERMISSION", "OK");
            }
            
            else
            {
                string childId = childIDEnty.Text;
                string phoneNo = helper.PhoneNo;

                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<OOSList>();
                    OOSList record = conn.Table<OOSList>().Where(x => x.ChildID.Equals(childId)).FirstOrDefault();

                    conn.CreateTable<Login>();
                    Login user = conn.Table<Login>().Where(x => x.PhoneNo.Equals(phoneNo)).FirstOrDefault();
                    if (string.IsNullOrEmpty(user.PhoneNo))
                    {
                        DisplayAlert("ERROR", "YOUR PHONE NO HAS CHANGED, CONTACT ADMIN", "OK");
                    }
                    else
                    {
                        string geocord = LocationLabel.Text;
                        string lat = geocord.Split(',')[0];
                        string lat_ = lat.Split(':')[1];
                        double xx = Double.Parse(lat_.Trim(), CultureInfo.InvariantCulture);

                        string longi = geocord.Split(',')[1];
                        string longi_ = longi.Split(':')[1];
                        double yy = Double.Parse(longi_.Trim(), CultureInfo.InvariantCulture);

                        string time = DateTime.Now.ToString("hh:mm tt");
                        string date = DateTime.Now.ToString("dddd, dd MMMM yyyy");

                        record.VaccinatorName = vaccinatorNameEntry.Text.Trim();
                        record.VaccinatorNumber = phoneNoEntry.Text.Trim();
                        //record.Date = dateEntry.Text.Trim();
                        //record.Time = timeEntry.Text.Trim();
                        record.VaccDate += date + " | ";
                        record.VaccTime += time + " | ";
                        record.TeamCode = teamCodeEntry.Text.Trim();
                        record.LGA = lgaEntry.Text.Trim();
                        record.CatchmentAreaHF = catchmentAreaHFEntry.Text.Trim();
                        record.SettlementName = settlementNameEnty.Text.Trim();
                        record.Ward = wardEntry.Text.Trim();
                        record.ChildName = childNameEnty.Text.Trim();
                        record.Age = childAgeEnty.Text.Trim();
                        record.Gender = GenderEnty.Text.Trim();
                        record.ChildID = childIDEnty.Text.Trim();
                        record.Latitude = xx; //latitude
                        record.Longitude = yy;//longitude
                        record.CurrentAge = AgePickerCurrent.SelectedItem.ToString().Trim();
                        //record.HasReceivedAntigen = GetPreviouslyReceivedRI().Trim();
                        record.IsRecordTally = GetChildRecordTally().Trim();
                        record.AntigensFromChildHealthCard = AllAdministeredAntigens1().Trim();
                        record.AEFI = GetAEFI().Trim();
                        record.AEFIType = GetAEFIType().Trim();
                        record.Completed = 1;
                        helper.Settlement = settlementNameEnty.Text.Trim();
                        helper.HealthFacility = catchmentAreaHFEntry.Text.Trim();
                        record.DueForNextAntigen = DueForNextAntigen.SelectedItem.ToString().Trim(); // used to hold due for next antigen
                        record.OldAntigensReceived = AntigensReceivedEnty.Text;
                        record.AntigensReceived += AllAdministeredAntigens().Trim()+" | ";
                        record.VaccinatorSupName = vaccinationSupervisorName.Trim();
                        //helper.LGA = user.LGA.Trim();
                        //helper.Ward = user.Ward.Trim();
                        //helper.UserId = user.TeamCode.Trim();


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
                            Navigation.PushAsync(new ChildrenLineListPage(helper));
                        }
                    }

                   

                }

                //End Validation


            }
        }


        private string GetAEFI()
        {
            // Loop through each RadioButton in the RadioButtonGroup
            string selectedValue = string.Empty;

            foreach (var view in RadioButtonGroupAEFI.Children)
            {
                if (view is RadioButton radioButton && radioButton.IsChecked)
                {
                    // Retrieve the selected value
                    selectedValue = radioButton.Content.ToString();

                }

            }
            return selectedValue;
        }

        private string GetAEFIType()
        {
            // Loop through each RadioButton in the RadioButtonGroup
            string selectedValue = string.Empty;

            foreach (var view in RadioButtonGroupAEFIType.Children)
            {
                if (view is RadioButton radioButton && radioButton.IsChecked)
                {
                    // Retrieve the selected value
                    selectedValue = radioButton.Content.ToString();

                }

            }
            return selectedValue;
        }

        //private string GetPreviouslyReceivedRI()
        //{
        //    // Loop through each RadioButton in the RadioButtonGroup
        //    string selectedValue = string.Empty;

        //    foreach (var view in RadioButtonGroupReceivedRIPreviously.Children)
        //    {
        //        if (view is RadioButton radioButton && radioButton.IsChecked)
        //        {
        //            // Retrieve the selected value
        //            selectedValue = radioButton.Content.ToString();

        //        }

        //    }
        //    return selectedValue;
        //}
        private string GetChildRecordTally()
        {
            // Loop through each RadioButton in the RadioButtonGroup
            string selectedValue = string.Empty;

            foreach (var view in VaccinationRecordTallyGroupButton.Children)
            {
                if (view is RadioButton radioButton && radioButton.IsChecked)
                {
                    // Retrieve the selected value
                    selectedValue = radioButton.Content.ToString();

                }

            }
            return selectedValue;
        }

        private string AllAdministeredAntigens1()
        {
            bool isHepBChecked = HepB01.IsChecked;
            bool isBCGChecked = BCG1.IsChecked;
            bool isYFChecked = YF1.IsChecked;
            bool isMENAChecked = MENA1.IsChecked;
            bool isnOPV2Checked = nOPV2.IsChecked;

            string selectedOptions = string.Empty;


            if (isHepBChecked) selectedOptions += "HepB , ";
            if (isBCGChecked) selectedOptions += "BCG , ";
            if (isYFChecked) selectedOptions += "YF , ";
            if (isMENAChecked) selectedOptions += "MENA , ";
            if (isnOPV2Checked) selectedOptions += "nOPV2 , ";
            if (PENTATypes1.SelectedIndex != -1) { selectedOptions += PENTATypes1.SelectedItem.ToString() + " ,"; };
            if (MeaslesTypes1.SelectedIndex != -1) { selectedOptions += MeaslesTypes1.SelectedItem.ToString() + " ,"; };
            if (PCVTypes1.SelectedIndex != -1) { selectedOptions += PCVTypes1.SelectedItem.ToString() + " ,"; };
            if (ROTATypes1.SelectedIndex != -1) { selectedOptions += ROTATypes1.SelectedItem.ToString() + " ,"; };
            if (IPVTypes1.SelectedIndex != -1) { selectedOptions += IPVTypes1.SelectedItem.ToString() + " ,"; };
            if (OPVTypes1.SelectedIndex != -1) { selectedOptions += OPVTypes1.SelectedItem.ToString() + " ,"; };


            return selectedOptions;
        }


        //BEGIN NEW IMPLEMNTATION

        void PENTA_CheckedChangedA(System.Object sender, Xamarin.Forms.CheckedChangedEventArgs e)
        {
            var item = sender as CheckBox;

            if (item.IsChecked)
            {
                PENTATypes1.SelectedIndex = -1;
                PENTATypes1.IsVisible = true;
            }
            if (!item.IsChecked)
            {
                PENTATypes1.SelectedIndex = -1;
                PENTATypes1.IsVisible = false;
            }

        }

        void Measles_CheckedChangedA(System.Object sender, Xamarin.Forms.CheckedChangedEventArgs e)
        {
            var item = sender as CheckBox;

            if (item.IsChecked)
            {
                MeaslesTypes1.SelectedIndex = -1;
                MeaslesTypes1.IsVisible = true;
            }
            if (!item.IsChecked)
            {
                MeaslesTypes1.SelectedIndex = -1;
                MeaslesTypes1.IsVisible = false;
            }
        }

        void PCV_CheckedChangedA(System.Object sender, Xamarin.Forms.CheckedChangedEventArgs e)
        {
            var item = sender as CheckBox;

            if (item.IsChecked)
            {
                PCVTypes1.SelectedIndex = -1;
                PCVTypes1.IsVisible = true;
            }
            if (!item.IsChecked)
            {
                PCVTypes1.SelectedIndex = -1;
                PCVTypes1.IsVisible = false;
            }
        }

        void IPV_CheckedChangedA(System.Object sender, Xamarin.Forms.CheckedChangedEventArgs e)
        {
            var item = sender as CheckBox;

            if (item.IsChecked)
            {
                IPVTypes1.SelectedIndex = -1;
                IPVTypes1.IsVisible = true;
            }
            if (!item.IsChecked)
            {
                IPVTypes1.SelectedIndex = -1;
                IPVTypes1.IsVisible = false;
            }
        }

        void OPV_CheckedChangedA(System.Object sender, Xamarin.Forms.CheckedChangedEventArgs e)
        {
            var item = sender as CheckBox;

            if (item.IsChecked)
            {
                OPVTypes1.SelectedIndex = -1;
                OPVTypes1.IsVisible = true;
            }
            if (!item.IsChecked)
            {
                OPVTypes1.SelectedIndex = -1;
                OPVTypes1.IsVisible = false;
            }
        }

        void ROTA_CheckedChangedA(System.Object sender, Xamarin.Forms.CheckedChangedEventArgs e)
        {
            var item = sender as CheckBox;

            if (item.IsChecked)
            {
                ROTATypes1.SelectedIndex = -1;
                ROTATypes1.IsVisible = true;
            }
            if (!item.IsChecked)
            {
                ROTATypes1.SelectedIndex = -1;
                ROTATypes1.IsVisible = false;
            }
        }

        void IPV_CheckedChanged_1A(System.Object sender, Xamarin.Forms.CheckedChangedEventArgs e)
        {
            var item = sender as CheckBox;

            if (item.IsChecked)
            {
                IPVTypes1.SelectedIndex = -1;
                IPVTypes1.IsVisible = true;
            }
            if (!item.IsChecked)
            {
                IPVTypes1.SelectedIndex = -1;
                IPVTypes1.IsVisible = false;
            }
        }

        void Measles_CheckedChanged_1A(System.Object sender, Xamarin.Forms.CheckedChangedEventArgs e)
        {
            var item = sender as CheckBox;

            if (item.IsChecked)
            {
                MeaslesTypes1.SelectedIndex = -1;
                MeaslesTypes1.IsVisible = true;
            }
            if (!item.IsChecked)
            {
                MeaslesTypes1.SelectedIndex = -1;
                MeaslesTypes1.IsVisible = false;
            }
        }


        //END NEW IMPLEMENTATION

        private string AllAdministeredAntigens()
        {
            bool isHepBChecked = HepB0.IsChecked;
            bool isBCGChecked = BCG.IsChecked;
            bool isYFChecked = YF.IsChecked;
            bool isMENAChecked = MENA.IsChecked;
            bool isNOPV2Checked = nOPV2.IsChecked;

            string selectedOptions = string.Empty;


            if (isHepBChecked) selectedOptions += "HepB , ";
            if (isBCGChecked) selectedOptions += "BCG , ";
            if (isYFChecked) selectedOptions += "YF , ";
            if (isMENAChecked) selectedOptions += "MENA , ";
            if (isNOPV2Checked) selectedOptions += "nOPV2 , ";
            if (PENTATypes.SelectedIndex != -1) { selectedOptions += PENTATypes.SelectedItem.ToString()+ " ,"; };
            if (MeaslesTypes.SelectedIndex != -1) { selectedOptions += MeaslesTypes.SelectedItem.ToString() + " ,"; };
            if (PCVTypes.SelectedIndex != -1) { selectedOptions += PCVTypes.SelectedItem.ToString() + " ,"; };
            if (ROTATypes.SelectedIndex != -1) { selectedOptions += ROTATypes.SelectedItem.ToString() + " ,"; };
            if (IPVTypes.SelectedIndex != -1) { selectedOptions += IPVTypes.SelectedItem.ToString() + " ,"; };
            if (OPVTypes.SelectedIndex != -1) { selectedOptions += OPVTypes.SelectedItem.ToString() + " ,"; };


            return selectedOptions;
        }


        private async void OnGetLocationClicked(object sender, EventArgs e)
        {
            try
            {
                var location = await Geolocation.GetLastKnownLocationAsync();

                if (location == null)
                {
                    // No cached location, get the real-time location
                    location = await Geolocation.GetLocationAsync(new GeolocationRequest
                    {
                        DesiredAccuracy = GeolocationAccuracy.Medium,
                        Timeout = TimeSpan.FromSeconds(30)
                    });
                }

                if (location != null)
                {
                    LocationLabel.Text = $"Latitude: {location.Latitude}, Longitude: {location.Longitude}";
                }
                else
                {
                    LocationLabel.Text = "Unable to retrieve location.";
                }
            }
            catch (FeatureNotSupportedException Ex)
            {
                // Handle not supported on device exception
                LocationLabel.Text = "Location not supported on this device.";
            }
            catch (PermissionException Ex)
            {
                // Handle permission exception
                LocationLabel.Text = "Location permission denied.";
            }
            catch (Exception ex)
            {
                // Handle other exceptions
                LocationLabel.Text = "An error occurred: " + ex.Message;
            }
        }

       

        void AEFI_CheckedChanged(System.Object sender, Xamarin.Forms.CheckedChangedEventArgs e)
        {

            var radioButton = sender as RadioButton;


            if (radioButton == AEFIYes)
            {
                RadioButtonGroupAEFI.BindingContext = "Yes";
                SeriousAEFI.IsVisible = true;
                NonSeriousAEFI.IsVisible = true;
                AEFITypeLabel.IsVisible = true;
                compulsory.IsVisible = true;
                AEFITypeLabel.IsVisible = true;
                RadioButtonGroupAEFIType.IsVisible = true;
            }
            if (radioButton == AEFINo)
            {
                RadioButtonGroupAEFI.BindingContext = "No";
                SeriousAEFI.IsVisible = false;
                NonSeriousAEFI.IsVisible = false;
                AEFITypeLabel.IsVisible = false;
                compulsory.IsVisible = false;
                AEFITypeLabel.IsVisible = false;
                RadioButtonGroupAEFIType.IsVisible = false;
            }
        }


        void AEFIType_CheckedChanged(System.Object sender, Xamarin.Forms.CheckedChangedEventArgs e)
        {

            var radioButton = sender as RadioButton;


            if (radioButton == SeriousAEFI)
            {
                // For Yes Option
                RadioButtonGroupAEFIType.BindingContext = "Serious";
            }
            if (radioButton == NonSeriousAEFI)
            {
                // For Yes Option
                RadioButtonGroupAEFIType.BindingContext = "Non-Serious";
            }

        }

        void PENTA_CheckedChanged(System.Object sender, Xamarin.Forms.CheckedChangedEventArgs e)
        {
            var item = sender as CheckBox;

            if (item.IsChecked){
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

        void DueForNextAntigen_SelectedIndexChanged(System.Object sender, System.EventArgs e)
        {
            var item = sender as Picker;

            if (item?.SelectedItem == null)
                return;

            string selected = item.SelectedItem.ToString();

            if (selected == "Yes")
            {
                string selectedAge1 = AgePickerCurrent.SelectedIndex.ToString();

                if (selectedAge1 == "-1")
                {
                    DisplayAlert("ERROR", "SELECT CHILD CURRENT AGE", "OK");
                    item.SelectedIndex = -1;
                }
                else
                {
                    followupQ.IsVisible = true;
                }
                
            }
            else if (selected == "No" || selected == "Not due for next Antigen")
            {
                followupQ.IsVisible = false;
            }
        }

    }
}

