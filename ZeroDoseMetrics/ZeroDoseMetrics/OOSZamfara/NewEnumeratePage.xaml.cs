using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Xml;
using SQLite;
using Xamarin.Essentials;
using Xamarin.Forms;
using ZeroDoseMetrics.Model;

using Newtonsoft.Json;
using System.Reflection;
using System.IO;
using System.Linq;

using System.Diagnostics;
//using Xamarin.Forms.PlatformConfiguration.iOSSpecific;

namespace ZeroDoseMetrics.OOSZamfara
{	
	public partial class NewEnumeratePage : ContentPage
	{
        public Login helper { get; set; }

        public OOSList newChild { get; set; }

        public string TeamCode { get; set; }
        public string Settlement { get; set; }
        public string HealthFacility { get; set; }
        public string InterviewerName { get; set; }
        public string PhoneNumber { get; set; }


        private Dictionary<string, List<string>> nigeriaStates;


        public NewEnumeratePage (Login helper)
		{

            InitializeComponent();
            LoadLocations();
            //Assembly assembly = Assembly.GetExecutingAssembly();

            //foreach (string resource in assembly.GetManifestResourceNames())
            //{
            //    Debug.WriteLine(resource);
            //}




            this.helper = helper;
            newChild = new OOSList();
            this.TeamCode = helper.TeamCode;
            this.Settlement = helper.Settlement;
            this.HealthFacility = helper.HealthFacility;
            this.PhoneNumber = helper.PhoneNo;
            this.InterviewerName = helper.InterviewerName;
            CheckBoxGroup.IsVisible = false;


            var lga = helper.LGA.ToUpper();
            var retlga = lga.Substring(0, 3);
            var ward = helper.Ward.ToUpper();
            var retward = ward.Substring(0, 3);

            string time = DateTime.Now.ToString("hh:mm tt");
            string date = DateTime.Now.ToString("dddd, dd MMMM yyyy");
            string unique = DateTime.Now.ToString("MMddHHmmss");

            EnumeratorNameEntry.Text = InterviewerName;
            phoneNoEntry.Text = PhoneNumber;
            dateEntry.Text = date;
            timeEntry.Text = time;
            teamCodeEntry.Text = TeamCode;
            lgaEntry.Text = lga;
            wardEntry.Text = ward;
            LocationLabel.Text = LocationLabel.Text;
            childIDEnty.Text = "IEV/"+retlga+"/"+ retward+"/"+ unique;
            settlementEntry.Text = Settlement;
            catchmentAreaHFEntry.Text = HealthFacility;
            RadioButtonGroupAEFI.IsVisible = false;
            AEFIAfterLbl.IsVisible = false;
            AEFIAfterLblComp.IsVisible = false;
        }

        void SettlementPicker_SelectedIndexChanged(System.Object sender, System.EventArgs e)
        {

        }

        void CatchmentAreaHF_SelectedIndexChanged(System.Object sender, System.EventArgs e)
        {

        }

        void Gender_CheckedChanged(System.Object sender, Xamarin.Forms.CheckedChangedEventArgs e)
        {
            
        }

        void PreviouslyReceivedRI_CheckedChanged(System.Object sender, Xamarin.Forms.CheckedChangedEventArgs e)
        {
            
        }

        void AEFI_CheckedChanged(System.Object sender, Xamarin.Forms.CheckedChangedEventArgs e)
        {

            var radioButton = sender as RadioButton;


            if (radioButton == AEFIYes)
            {
                RadioButtonGroupAEFI.BindingContext = "Yes";
                RadioButtonGroupAEFIType.IsVisible = true;
                SeriousAEFI.IsVisible = true;
                NonSeriousAEFI.IsVisible = true;
                AEFITypeLabel.IsVisible = true;
            }
            if (radioButton == AEFINo)
            {
                RadioButtonGroupAEFI.BindingContext = "No";
                SeriousAEFI.IsVisible = false;
                NonSeriousAEFI.IsVisible = false;
                AEFITypeLabel.IsVisible = false;
                RadioButtonGroupAEFIType.IsVisible = false;

            }

        }

        private void LoadLocations()
        {
            var assembly = Assembly.GetExecutingAssembly();

            var resourceName = "ZeroDoseMetrics.Data.nigeria_location.json";

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                string json = reader.ReadToEnd();

                nigeriaStates =
                    JsonConvert.DeserializeObject<Dictionary<string, List<string>>>(json);
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

        void AgePicker_SelectedIndexChanged(System.Object sender, System.EventArgs e)
        {
            if (AgePicker.SelectedItem == null)
                //return; // or show a message
                DisplayAlert("ERROR", "SELECT CHILD CURRENT AGE", "OK");
            string selectedCurrentAge = AgePicker.SelectedItem.ToString();
            if (selectedCurrentAge == "Birth - 5 weeks")
            {
                //OPVStackL.IsVisible = true;
                //HepBStack.IsVisible = true;
                //BCGStack.IsVisible = true;

                ////set others false
                //PentaStack.IsVisible = false;
                //PCVStack.IsVisible = false;
                //RotaStack.IsVisible = false;
                //IPVStack.IsVisible = false;
                //MeaslesStack.IsVisible = false;
                //YFStack.IsVisible = false;
                //MenAStack.IsVisible = false;
                atBirth.IsVisible = true;
                atSixToNineWeeks.IsVisible = false;
                atTenToThirteenWeeks.IsVisible = false;
                atfourteenWeeksEightMonths.IsVisible = false;
                atNineMonthsToElevenMonmths_atTwelveMonthsToFourteenMonths.IsVisible = false;
                atFifteenMonthsToTwentyThreeMonths.IsVisible = false;
            }
            else if (selectedCurrentAge == "6 weeks – 9 weeks")
            {
                //OPVStackL.IsVisible = true;
                //HepBStack.IsVisible = true;
                //BCGStack.IsVisible = true;
                //PentaStack.IsVisible = true;
                //PCVStack.IsVisible = true;
                //RotaStack.IsVisible = true;
                //IPVStack.IsVisible = true;

                //// set false
                //MeaslesStack.IsVisible = false;
                //YFStack.IsVisible = false;
                //MenAStack.IsVisible = false;
                atBirth.IsVisible = true;
                atSixToNineWeeks.IsVisible = true;
                atTenToThirteenWeeks.IsVisible = false;
                atfourteenWeeksEightMonths.IsVisible = false;
                atNineMonthsToElevenMonmths_atTwelveMonthsToFourteenMonths.IsVisible = false;
                atFifteenMonthsToTwentyThreeMonths.IsVisible = false;

            }
            else if (selectedCurrentAge == "10 weeks – 13 weeks")
            {
                //OPVStackL.IsVisible = true;
                //HepBStack.IsVisible = true;
                //BCGStack.IsVisible = true;
                //PentaStack.IsVisible = true;
                //PCVStack.IsVisible = true;
                //RotaStack.IsVisible = true;
                //IPVStack.IsVisible = true;

                //// set false
                //MeaslesStack.IsVisible = false;
                //YFStack.IsVisible = false;
                //MenAStack.IsVisible = false;

                atBirth.IsVisible = true;
                atSixToNineWeeks.IsVisible = true;
                atTenToThirteenWeeks.IsVisible = true;
                atfourteenWeeksEightMonths.IsVisible = false;
                atNineMonthsToElevenMonmths_atTwelveMonthsToFourteenMonths.IsVisible = false;
                atFifteenMonthsToTwentyThreeMonths.IsVisible = false;
            }
            else if (selectedCurrentAge == "14 weeks – 8 months")
            {
                //OPVStackL.IsVisible = true;
                //HepBStack.IsVisible = true;
                //BCGStack.IsVisible = true;
                //PentaStack.IsVisible = true;
                //PCVStack.IsVisible = true;
                //RotaStack.IsVisible = true;
                //IPVStack.IsVisible = true;

                //// set false
                //MeaslesStack.IsVisible = false;
                //YFStack.IsVisible = false;
                //MenAStack.IsVisible = false;
                atBirth.IsVisible = true;
                atSixToNineWeeks.IsVisible = true;
                atTenToThirteenWeeks.IsVisible = true;
                atfourteenWeeksEightMonths.IsVisible = true;
                atNineMonthsToElevenMonmths_atTwelveMonthsToFourteenMonths.IsVisible = false;
                atFifteenMonthsToTwentyThreeMonths.IsVisible = false;
            }
            else if (selectedCurrentAge == "9 months – 11 months")
            {
                //OPVStackL.IsVisible = true;
                //HepBStack.IsVisible = true;
                //BCGStack.IsVisible = true;
                //PentaStack.IsVisible = true;
                //PCVStack.IsVisible = true;
                //RotaStack.IsVisible = true;
                //IPVStack.IsVisible = true;
                //MeaslesStack.IsVisible = true;
                //YFStack.IsVisible = true;
                //MenAStack.IsVisible = true;

                atBirth.IsVisible = true;
                atSixToNineWeeks.IsVisible = true;
                atTenToThirteenWeeks.IsVisible = true;
                atfourteenWeeksEightMonths.IsVisible = true;
                atNineMonthsToElevenMonmths_atTwelveMonthsToFourteenMonths.IsVisible = true;
                atFifteenMonthsToTwentyThreeMonths.IsVisible = false;
            }
            else if (selectedCurrentAge == "12 months – 14 months")
            {
                //OPVStackL.IsVisible = true;
                //BCGStack.IsVisible = true;
                //PentaStack.IsVisible = true;
                //PCVStack.IsVisible = true;
                //RotaStack.IsVisible = true;
                //IPVStack.IsVisible = true;
                //MeaslesStack.IsVisible = true;
                //YFStack.IsVisible = true;
                //MenAStack.IsVisible = true;

                ////set false
                //HepBStack.IsVisible = false;


                atBirth.IsVisible = true;
                atSixToNineWeeks.IsVisible = true;
                atTenToThirteenWeeks.IsVisible = true;
                atfourteenWeeksEightMonths.IsVisible = true;
                atNineMonthsToElevenMonmths_atTwelveMonthsToFourteenMonths.IsVisible = true;
                atFifteenMonthsToTwentyThreeMonths.IsVisible = false;
            }
            else if (selectedCurrentAge == "15 months – 23 months" ||selectedCurrentAge == "24 months – 59 months")
            {
                //OPVStackL.IsVisible = true;
                //BCGStack.IsVisible = true;
                //PentaStack.IsVisible = true;
                //PCVStack.IsVisible = true;
                //RotaStack.IsVisible = true;
                //IPVStack.IsVisible = true;
                //MeaslesStack.IsVisible = true;
                //YFStack.IsVisible = true;
                //MenAStack.IsVisible = true;

                ////set false
                //HepBStack.IsVisible = false;


                atBirth.IsVisible = true;
                atSixToNineWeeks.IsVisible = true;
                atTenToThirteenWeeks.IsVisible = true;
                atfourteenWeeksEightMonths.IsVisible = true;
                atNineMonthsToElevenMonmths_atTwelveMonthsToFourteenMonths.IsVisible = true;
                atFifteenMonthsToTwentyThreeMonths.IsVisible = true;
            }

        }

        void Submit_Clicked(System.Object sender, System.EventArgs e)
        {

            //validation
            string coordi = LocationLabel.Text;
            string settlementTypePic = SettlementTypePicker?.SelectedItem?.ToString();
            string respond = RespondentPicker?.SelectedItem?.ToString();
            string household = HouseholdNameEnty.Text;
            string caregiver = caregiverNameEnty.Text;
            string caregiverNumber = caregiverNumberEnty.Text;
            string childID = childIDEnty.Text;
            string childName = childNameEnty.Text;
            string age = AgePicker?.SelectedItem?.ToString();
            string gender = GenderPicker?.SelectedItem?.ToString();
            string antigenPicker = ChildreceivedAntigenPicker?.SelectedItem?.ToString();
            string administeredAntigens = AllAdministeredAntigens();
            string RICardAvailable = ChildRICardPicker?.SelectedItem?.ToString();// check
            string InternationalBorderSettlementType = IntSettlementTypePicker?.SelectedItem?.ToString();
            string NeighbouringCountryName = neighbouringCountryTypePicker?.SelectedItem?.ToString();
            string SettlementHabitationStatus = HabitationStatusTypePicker?.SelectedItem?.ToString();
            string ReasonForDesertion = reasonDesertionTypePicker?.SelectedItem?.ToString();
            string AccessibilityStatus = AccessibilityStatusTypePicker?.SelectedItem?.ToString();
            string NomadicPopStayPeriod = nomardicStayPeriodPicker?.SelectedItem?.ToString();
            string NomadicPopulationMove = nomardicrelocationYesNoPicker?.SelectedItem?.ToString();
            string NomadicWhenMoving = nomardicRelocationYesFollowupPicker?.SelectedItem?.ToString();
            string AFPCases = AFPCase?.SelectedItem?.ToString();
            string AFPCaseCount = AFPCountEntry.Text;
            string AFPReportingDSNO = reportedToDSNO?.SelectedItem?.ToString();
            string StateFrom = stateToPicker?.SelectedItem?.ToString();
            string LGAFrom = LGAPicker?.SelectedItem?.ToString();
            string StateTo = stateToPicker?.SelectedItem?.ToString();
            string LGATo = LGAToPicker?.SelectedItem?.ToString();

            if (followupQ.IsVisible && (administeredAntigens == ""))
            {

                DisplayAlert("ERROR", "SELECT ANTIGEN BEFORE YOU PROCEED", "OK");

            }
            else if (!(string.IsNullOrEmpty(caregiverNumber)) && !(caregiverNumber.Length == 11))
            {              
                 DisplayAlert("ERROR PHONE NO", "PHONE NUMBER MUST BE 11 DIGITS IF AVAILABLE", "OK");
            }
            //else if(aefi == "Yes" && aefiType == "")
            //{
            //    DisplayAlert("ERROR", "SELECT AEFI TYPE BEFORE YOU PROCEED", "OK");
            //}       
            else if (string.IsNullOrEmpty(LocationLabel.Text) || settlementTypePic == "-1" || coordi == "Fetch location..."
                || respond == null || settlementTypePic == "-1" || household == null || caregiver == null
                || childID == null || childName == null || age == "-1" || gender == "-1" || antigenPicker == "-1" || respond == "-1"
                )
            {
                DisplayAlert("ERROR", "FILL ALL FIELDS IN THE FORM BEFORE SUBMITTING", "OK");
            }
            else
            {
                string geocord = LocationLabel.Text;
                string lat = geocord.Split(',')[0];
                string lat_ = lat.Split(':')[1];
                double x = Double.Parse(lat_.Trim(), CultureInfo.InvariantCulture);

                string longi = geocord.Split(',')[1];
                string longi_ = lat.Split(':')[1];
                double y = Double.Parse(longi_.Trim(), CultureInfo.InvariantCulture);

                newChild.VaccinatorName = EnumeratorNameEntry.Text.Trim();
                newChild.VaccinatorNumber = phoneNoEntry.Text.Trim();
                newChild.Date = dateEntry.Text.Trim();
                newChild.Time = timeEntry.Text.Trim();
                newChild.LGA = lgaEntry.Text.Trim();
                newChild.Ward = wardEntry.Text.Trim();
                newChild.SettlementType = SettlementTypePicker.SelectedItem.ToString().Trim();
                newChild.Latitude = x;
                newChild.Longitude = y;
                newChild.Respondent = RespondentPicker.SelectedItem.ToString().Trim(); ;
                newChild.HouseHoldHeadName = HouseholdNameEnty.Text.Trim();
                newChild.CaregiverName = caregiverNameEnty.Text.Trim();
                if (!(string.IsNullOrEmpty(caregiverNumber)))
                {
                    newChild.CaregiverNumber = caregiverNumberEnty.Text.Trim();
                }
                newChild.ChildID = childIDEnty.Text.Trim();
                newChild.ChildName = childNameEnty.Text.Trim();
                newChild.CurrentAge = AgePicker.SelectedItem.ToString().Trim();
                newChild.Gender = GenderPicker.SelectedItem.ToString().Trim();
                newChild.HasReceivedAntigen = ChildreceivedAntigenPicker.SelectedItem.ToString().Trim();
                //newChild.AntigensReceived = AllAdministeredAntigens();

                if(followupQ.IsVisible == false)
                {
                    newChild.OldAntigensReceived = null;
                }
                else
                {
                    newChild.OldAntigensReceived = AllAdministeredAntigens();
                } 
                newChild.Completed = 0;
                newChild.SettlementName = settlementEntry.Text.Trim();
                newChild.CatchmentAreaHF = catchmentAreaHFEntry.Text.Trim();
                newChild.TeamCode = teamCodeEntry.Text.Trim();
                newChild.Age = age.Trim();
                helper.LGA = lgaEntry.Text.Trim();
                helper.Ward = wardEntry.Text.Trim();


                newChild.RICardAvailable = RICardAvailable;
                newChild.InternationalBorderSettlementType = InternationalBorderSettlementType;
                newChild.NeighbouringCountryName = NeighbouringCountryName;
                newChild.SettlementHabitationStatus = SettlementHabitationStatus;
                newChild.ReasonForDesertion = ReasonForDesertion;
                newChild.AccessibilityStatus = AccessibilityStatus;
                newChild.NomadicPopStayPeriod = NomadicPopStayPeriod;
                newChild.NomadicPopulationMove = NomadicPopulationMove;
                newChild.NomadicWhenMoving = NomadicWhenMoving;
                newChild.StateFrom = StateFrom;
                newChild.LGAFrom = LGAFrom;
                newChild.StateTo = StateTo;
                newChild.LGATo = LGATo;
                newChild.AFPCase = AFPCases;
                newChild.AFPCaseCount = AFPCaseCount;
                newChild.AFPReportingDSNO = AFPReportingDSNO;


                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {

                    conn.CreateTable<OOSList>();

                    var existingChild = conn.Table<OOSList>()
                    .FirstOrDefault(g => g.ChildID == newChild.ChildID && g.SettlementName == newChild.SettlementName && g.VaccinatorNumber == newChild.VaccinatorNumber);
                    if (existingChild != null)
                    {
                        // Overwrite the existing record
                        newChild.Id = existingChild.Id;
                        int row = conn.Update(newChild);
                        if (row > 0)
                        {
                            DisplayAlert("Success", "Child record exists it has be successfully updated", "OK");
                            Navigation.PushAsync(new ChildrenLineListPage(helper));

                        }
                        else
                        {
                            DisplayAlert("Failure", "Error saving Child record", "OK");
                        }
                    }
                    else
                    {
                        // Insert new record
                        int row = conn.Insert(newChild);
                        if (row > 0)
                        {
                            DisplayAlert("Success", "Child record successfully saved", "OK");
                            Navigation.PushAsync(new ChildrenLineListPage(helper));

                        }
                        else
                        {
                            DisplayAlert("Failure", "Error saving Child record", "OK");
                        }
                    }

                }
            }

            //vcalidation
        }

        private void ToggleInternationalBorderSection(bool visible)
        {
            intlBorderLbl.IsVisible = visible;
            IntBorderFrame.IsVisible = visible;
            neighbouringCountryLbl.IsVisible = visible;
            neighbouringCountryFrame.IsVisible = visible;



        }

        private void ToggleNomadicSection(bool visible)
        {
            nomadicStayLBL.IsVisible = visible;
            nomadicStayFrame.IsVisible = visible;
            nomadicplantoleaveLBL.IsVisible = visible;
            nomadicplantoleaveFrame.IsVisible = visible;
            nomadicFromWhereLBL.IsVisible = visible;
            nomardicFromWherePicker.IsVisible = visible;
            nomadicFromWhereFrame.IsVisible = visible;
            


        }



        void SettlementTypePicker_SelectedIndexChanged(System.Object sender, System.EventArgs e)
        {
            if (!(sender is Picker picker))
                return;


            string settlementType = picker.SelectedItem?.ToString();

            ToggleInternationalBorderSection(settlementType == "International Border");

            ToggleNomadicSection(settlementType == "Migrant/Nomadic");

        }

        void GenderPicker_SelectedIndexChanged(System.Object sender, System.EventArgs e)
        {

        }

        void ChildreceivedAntigenPicker_SelectedIndexChanged(System.Object sender, System.EventArgs e)
        {
            var item = sender as Picker;

            if (item?.SelectedItem == null)
                return;

            string selected = item.SelectedItem.ToString();

            if (selected == "Yes")
            {
                string selectedAge1 = AgePicker.SelectedIndex.ToString();

                if (selectedAge1 == "-1")
                {
                    DisplayAlert("ERROR", "SELECT CHILD CURRENT AGE", "OK");
                    item.SelectedIndex = -1;
                }
                else
                {
                    followupQ.IsVisible = true;
                    CheckBoxGroup.IsVisible = true;
                    //setStacks();

                }

            }
            else if (selected == "No")
            {
                followupQ.IsVisible = false;
                

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

        private string AllAdministeredAntigens()
        {
            //bool isHepBChecked = HepB0.IsChecked;
            //bool isBCGChecked = BCG.IsChecked;
            //bool isYFChecked = YF.IsChecked;
            //bool isMENAChecked = MENA.IsChecked;

            string selectedOptions = string.Empty;

            if (BCG_.IsChecked) selectedOptions += "BCG , ";
            if (OPV0_.IsChecked) selectedOptions += "OPV 0 , ";
            if (HepB0_.IsChecked) selectedOptions += "Hep B0 , ";
            if (PENTA1.IsChecked) selectedOptions += "PENTA 1 , ";
            if (PCV1.IsChecked) selectedOptions += "PCV 1 , ";
            if (OPV1.IsChecked) selectedOptions += "OPV 1 , ";
            if (IPV1.IsChecked) selectedOptions += "IPV 1 , ";
            if (ROTA1.IsChecked) selectedOptions += "ROTA 1 , ";
            if (PENTA2.IsChecked) selectedOptions += "PENTA 2 , ";
            if (PCV2.IsChecked) selectedOptions += "PCV 2 , ";
            if (OPV2.IsChecked) selectedOptions += "OPV 2 , ";
            if (ROTA2.IsChecked) selectedOptions += "ROTA 2 , ";
            if (PENTA3.IsChecked) selectedOptions += "PENTA 3 , ";
            if (PCV3.IsChecked) selectedOptions += "PCV 3 , ";
            if (OPV3.IsChecked) selectedOptions += "OPV 3 , ";
            if (ROTA3.IsChecked) selectedOptions += "ROTA 3 , ";
            if (IPV2.IsChecked) selectedOptions += "IPV 2 , ";
            if (Measles1.IsChecked) selectedOptions += "MEASLES 1 , ";
            if (YF.IsChecked) selectedOptions += "YELLOW FEVER , ";
            if (MenA.IsChecked) selectedOptions += "MEN A , ";
            if (Measles2.IsChecked) selectedOptions += "MEASLES 2 , ";
            if (nOPV2.IsChecked) selectedOptions += "nOPV2 , ";
            //if (HPV.IsChecked) selectedOptions += "HPV , ";


            //if (isHepBChecked) selectedOptions += "HepB | ";
            //if (isBCGChecked) selectedOptions += "BCG | ";
            //if (isYFChecked) selectedOptions += "YF | ";
            //if (isMENAChecked) selectedOptions += "MENA | ";
            //if (PENTATypes.SelectedIndex != -1) { selectedOptions += PENTATypes.SelectedItem.ToString() + " |"; };
            //if (MeaslesTypes.SelectedIndex != -1) { selectedOptions += MeaslesTypes.SelectedItem.ToString() + " |"; };
            //if (PCVTypes.SelectedIndex != -1) { selectedOptions += PCVTypes.SelectedItem.ToString() + " |"; };
            //if (ROTATypes.SelectedIndex != -1) { selectedOptions += ROTATypes.SelectedItem.ToString() + " |"; };
            //if (IPVTypes.SelectedIndex != -1) { selectedOptions += IPVTypes.SelectedItem.ToString() + " |"; };
            //if (OPVTypes.SelectedIndex != -1) { selectedOptions += OPVTypes.SelectedItem.ToString() + " |"; };


            return selectedOptions;
        }

        void RespondentEnty_SelectedIndexChanged(System.Object sender, System.EventArgs e)
        {
        }


        void PENTA_CheckedChanged(System.Object sender, Xamarin.Forms.CheckedChangedEventArgs e)
        {
            var item = sender as CheckBox;

            //if (item.IsChecked)
            //{
            //    PENTATypes.SelectedIndex = -1;
            //    PENTATypes.IsVisible = true;
            //}
            //if (!item.IsChecked)
            //{
            //    PENTATypes.SelectedIndex = -1;
            //    PENTATypes.IsVisible = false;
            //}

        }

        void Measles_CheckedChanged(System.Object sender, Xamarin.Forms.CheckedChangedEventArgs e)
        {
            var item = sender as CheckBox;

            //if (item.IsChecked)
            //{
            //    MeaslesTypes.SelectedIndex = -1;
            //    MeaslesTypes.IsVisible = true;
            //}
            //if (!item.IsChecked)
            //{
            //    MeaslesTypes.SelectedIndex = -1;
            //    MeaslesTypes.IsVisible = false;
            //}
        }

        void PCV_CheckedChanged(System.Object sender, Xamarin.Forms.CheckedChangedEventArgs e)
        {
            var item = sender as CheckBox;

            //if (item.IsChecked)
            //{
            //    PCVTypes.SelectedIndex = -1;
            //    PCVTypes.IsVisible = true;
            //}
            //if (!item.IsChecked)
            //{
            //    PCVTypes.SelectedIndex = -1;
            //    PCVTypes.IsVisible = false;
            //}
        }

        void IPV_CheckedChanged(System.Object sender, Xamarin.Forms.CheckedChangedEventArgs e)
        {
            var item = sender as CheckBox;

            //if (item.IsChecked)
            //{
            //    IPVTypes.SelectedIndex = -1;
            //    IPVTypes.IsVisible = true;
            //}
            //if (!item.IsChecked)
            //{
            //    IPVTypes.SelectedIndex = -1;
            //    IPVTypes.IsVisible = false;
            //}
        }

        void OPV_CheckedChanged(System.Object sender, Xamarin.Forms.CheckedChangedEventArgs e)
        {
            var item = sender as CheckBox;

            //if (item.IsChecked)
            //{
            //    OPVTypes.SelectedIndex = -1;
            //    OPVTypes.IsVisible = true;
            //}
            //if (!item.IsChecked)
            //{
            //    OPVTypes.SelectedIndex = -1;
            //    OPVTypes.IsVisible = false;
            //}
        }

        void ROTA_CheckedChanged(System.Object sender, Xamarin.Forms.CheckedChangedEventArgs e)
        {
            var item = sender as CheckBox;

            //if (item.IsChecked)
            //{
            //    ROTATypes.SelectedIndex = -1;
            //    ROTATypes.IsVisible = true;
            //}
            //if (!item.IsChecked)
            //{
            //    ROTATypes.SelectedIndex = -1;
            //    ROTATypes.IsVisible = false;
            //}
        }

        void IPV_CheckedChanged_1(System.Object sender, Xamarin.Forms.CheckedChangedEventArgs e)
        {
            var item = sender as CheckBox;

            //if (item.IsChecked)
            //{
            //    IPVTypes.SelectedIndex = -1;
            //    IPVTypes.IsVisible = true;
            //}
            //if (!item.IsChecked)
            //{
            //    IPVTypes.SelectedIndex = -1;
            //    IPVTypes.IsVisible = false;
            //}
        }

        void Measles_CheckedChanged_1(System.Object sender, Xamarin.Forms.CheckedChangedEventArgs e)
        {
            var item = sender as CheckBox;

            //if (item.IsChecked)
            //{
            //    MeaslesTypes.SelectedIndex = -1;
            //    MeaslesTypes.IsVisible = true;
            //}
            //if (!item.IsChecked)
            //{
            //    MeaslesTypes.SelectedIndex = -1;
            //    MeaslesTypes.IsVisible = false;
            //}
        }


        void setStacks()
        {
            if (AgePicker.SelectedItem == null)
                //return; // or show a message
                DisplayAlert("ERROR", "SELECT CHILD CURRENT AGE", "OK");
            string selectedCurrentAge = AgePicker.SelectedItem.ToString();
            if (selectedCurrentAge == "Birth - 5 weeks")
            {
                //OPVStackL.IsVisible = true;
                //HepBStack.IsVisible = true;
                //BCGStack.IsVisible = true;

                ////set others false
                //PentaStack.IsVisible = false;
                //PCVStack.IsVisible = false;
                //RotaStack.IsVisible = false;
                //IPVStack.IsVisible = false;
                //MeaslesStack.IsVisible = false;
                //YFStack.IsVisible = false;
                //MenAStack.IsVisible = false;


            }
            else if (selectedCurrentAge == "6 weeks – 9 weeks")
            {
                //OPVStackL.IsVisible = true;
                //HepBStack.IsVisible = true;
                //BCGStack.IsVisible = true;
                //PentaStack.IsVisible = true;
                //PCVStack.IsVisible = true;
                //RotaStack.IsVisible = true;
                //IPVStack.IsVisible = true;

                //// set false
                //MeaslesStack.IsVisible = false;
                //YFStack.IsVisible = false;
                //MenAStack.IsVisible = false;

            }
            else if (selectedCurrentAge == "10 weeks – 13 weeks")
            {
                //OPVStackL.IsVisible = true;
                //HepBStack.IsVisible = true;
                //BCGStack.IsVisible = true;
                //PentaStack.IsVisible = true;
                //PCVStack.IsVisible = true;
                //RotaStack.IsVisible = true;
                //IPVStack.IsVisible = true;

                //// set false
                //MeaslesStack.IsVisible = false;
                //YFStack.IsVisible = false;
                //MenAStack.IsVisible = false;
            }
            else if (selectedCurrentAge == "14 weeks – 8 months")
            {
                //OPVStackL.IsVisible = true;
                //HepBStack.IsVisible = true;
                //BCGStack.IsVisible = true;
                //PentaStack.IsVisible = true;
                //PCVStack.IsVisible = true;
                //RotaStack.IsVisible = true;
                //IPVStack.IsVisible = true;

                //// set false
                //MeaslesStack.IsVisible = false;
                //YFStack.IsVisible = false;
                //MenAStack.IsVisible = false;
            }
            else if (selectedCurrentAge == "9 months – 11 months")
            {
                //OPVStackL.IsVisible = true;
                //HepBStack.IsVisible = true;
                //BCGStack.IsVisible = true;
                //PentaStack.IsVisible = true;
                //PCVStack.IsVisible = true;
                //RotaStack.IsVisible = true;
                //IPVStack.IsVisible = true;
                //MeaslesStack.IsVisible = true;
                //YFStack.IsVisible = true;
                //MenAStack.IsVisible = true;
            }
            else if (selectedCurrentAge == "12 months – 14 months")
            {
                //OPVStackL.IsVisible = true;
                //HepBStack.IsVisible = true;
                //BCGStack.IsVisible = true;
                //PentaStack.IsVisible = true;
                //PCVStack.IsVisible = true;
                //RotaStack.IsVisible = true;
                //IPVStack.IsVisible = true;
                //MeaslesStack.IsVisible = true;
                //YFStack.IsVisible = true;
                //MenAStack.IsVisible = true;
            }
            else if (selectedCurrentAge == "15 months – 23 months")
            {
                //OPVStackL.IsVisible = true;
                //BCGStack.IsVisible = true;
                //PentaStack.IsVisible = true;
                //PCVStack.IsVisible = true;
                //RotaStack.IsVisible = true;
                //IPVStack.IsVisible = true;
                //MeaslesStack.IsVisible = true;
                //YFStack.IsVisible = true;
                //MenAStack.IsVisible = true;

                ////set false
                //HepBStack.IsVisible = false;
            }
        }

        void InternationalSettlementTypePicker_SelectedIndexChanged(System.Object sender, System.EventArgs e)
        {

        }

        void NeighbouringCountryTypePicker_SelectedIndexChanged(System.Object sender, System.EventArgs e)
        {
        }

        void HabitationStatusTypePicker_SelectedIndexChanged(System.Object sender, System.EventArgs e)
        {
            var habitationStatus = sender as Picker;

            if (habitationStatus?.SelectedItem.ToString() == "Deserted")
            {
                reasonDesertionLbl.IsVisible = true;
                reasonDesertionFrame.IsVisible = true;
                
            }
            else
            {
                reasonDesertionLbl.IsVisible = false;
                reasonDesertionFrame.IsVisible = false;
               
            }
        }

        void DesertionReasonTypePicker_SelectedIndexChanged(System.Object sender, System.EventArgs e)
        {
        }

        void AccessibilityStatusTypePicker_SelectedIndexChanged(System.Object sender, System.EventArgs e)
        {
        }

        void nomardicrelocationTypePicker_SelectedIndexChanged(System.Object sender, System.EventArgs e)
        {
            var relocationOption =  sender as Picker;
            if(relocationOption?.SelectedItem.ToString() == "Yes")
            {
                relocationYesLBL.IsVisible = true;
                relocationYesFrame.IsVisible = true;
                stateToPicker.IsVisible = true;
                nomardicToWhereFrame.IsVisible = true;
                nomardicToWherePicker.IsVisible = true;
                nomadicToWhereLBL.IsVisible = true;
            }
            else
            {
                relocationYesLBL.IsVisible = false;
                relocationYesFrame.IsVisible = false;
                stateToPicker.IsVisible = false;
                nomardicToWhereFrame.IsVisible = false;
                nomardicToWherePicker.IsVisible = false;
                nomadicToWhereLBL.IsVisible = false;

                StateToPickerFrame.IsVisible = false;
                stateToPicker.ItemsSource = null;
                LGAToPicker.IsVisible = false;
                LGAToPickerFrame.IsVisible = false;
                LGAToPicker.ItemsSource = null;

            }
        }

        void nomardicRelocationYesFollowupPicker_SelectedIndexChanged(System.Object sender, System.EventArgs e)
        {
        }

        void ChildRICardPicker_SelectedIndexChanged(System.Object sender, System.EventArgs e)
        {
        }

        void AFPCase_SelectedIndexChanged(System.Object sender, System.EventArgs e)
        {

            var AFPCase = sender as Picker;
            if(AFPCase?.SelectedItem.ToString() == "Yes")
            {
                AFPLbl.IsVisible = true;
                AFPCountEntry.IsVisible = true;
                LGADSNOLbl.IsVisible = true;
                reportedToDSNO.IsVisible = true;
                LGADSNOFrame.IsVisible = true;

            }
            else
            {
                AFPLbl.IsVisible = false;
                AFPCountEntry.IsVisible = false;
                LGADSNOLbl.IsVisible = false;
                reportedToDSNO.IsVisible = false;
                LGADSNOFrame.IsVisible = false;
            }
        }

        void reportedToDSNO_SelectedIndexChanged(System.Object sender, System.EventArgs e)
        {

        }

        void nomardicFromWherePicker_SelectedIndexChanged(System.Object sender, System.EventArgs e)
        {
            var locations = sender as Picker;

            if(locations?.SelectedItem?.ToString() == "Nigeria States")
            {
                StatePickerFrame.IsVisible = true;
                statePicker.ItemsSource = nigeriaStates.Keys.ToList();
            }
            else
            {
                StatePickerFrame.IsVisible = false;
                statePicker.ItemsSource = null;
                LGAPicker.IsVisible = false;
                LGAPickerFrame.IsVisible = false;
                LGAPicker.ItemsSource = null;
            }
        }

        void statePicker_SelectedIndexChanged(System.Object sender, System.EventArgs e)
        {
            if (statePicker.SelectedItem == null)
                return;

            string state =
            statePicker.SelectedItem.ToString();
            LGAPicker.IsVisible = true;
            LGAPickerFrame.IsVisible = true;
            LGAPicker.ItemsSource =
            nigeriaStates[state];
        }

        void nomardicToWherePicker_SelectedIndexChanged(System.Object sender, System.EventArgs e)
        {

            var locations = sender as Picker;

            if (locations?.SelectedItem?.ToString() == "Nigeria States")
            {
                StateToPickerFrame.IsVisible = true;
                stateToPicker.ItemsSource = nigeriaStates.Keys.ToList();
            }
            else
            {
                StateToPickerFrame.IsVisible = false;
                stateToPicker.ItemsSource = null;
                LGAToPicker.IsVisible = false;
                LGAToPickerFrame.IsVisible = false;
                LGAToPicker.ItemsSource = null;

            }

        }

        void LGAPicker_SelectedIndexChanged(System.Object sender, System.EventArgs e)
        {
        }

        void stateToPicker_SelectedIndexChanged(System.Object sender, System.EventArgs e)
        {
            if (stateToPicker.SelectedItem == null)
                return;

            string state =
            stateToPicker.SelectedItem.ToString();
            LGAToPicker.IsVisible = true;
            LGAToPickerFrame.IsVisible = true;
            LGAToPicker.ItemsSource =
            nigeriaStates[state];
        }
    }
}

