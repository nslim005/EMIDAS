using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SQLite;
using Xamarin.Essentials;
using Xamarin.Forms;
using ZeroDoseMetrics.Model;

namespace ZeroDoseMetrics.OOSZamfara
{
    public partial class ChildrenLineListPage : ContentPage
    {
        private ObservableCollection<OOSList> Item;
        public string TeamCode { get; set; }
        public string Settlement { get; set; }
        public string HealthFacility { get; set; }
        public string InterviewerName { get; set; }
        public string PhoneNumber { get; set; }
        public string UserId { get; set; }
        public string LGA { get; set; }
        public string Ward { get; set; }
        public OOSList searchEntity { get; set; }
        public Login helper { get; set; }

        public ChildrenLineListPage(Login login)
        {
            InitializeComponent();
            Item = new ObservableCollection<OOSList>();
            ChildrenLineList.ItemsSource = Item;
            this.TeamCode = login.TeamCode;
            this.Settlement = login.Settlement;
            this.HealthFacility = login.HealthFacility;
            this.PhoneNumber = login.PhoneNo;
            this.InterviewerName = login.InterviewerName;
            this.LGA = login.LGA;
            this.Ward = login.Ward;
            searchEntity = new OOSList();
            helper = new Login();
            downloadLineList.Toggled += onToggleDownload; // Add an event handler for the Toggled event
            OnAppearing();
        }

        //void searchBar_TextChanged(System.Object sender, Xamarin.Forms.TextChangedEventArgs e)
        //{
        //    var SearchItem = e.NewTextValue?.ToLower() ?? string.Empty;
        //    using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
        //    {
        //        List<Child> AllChildren = new List<Child>();
        //        AllChildren = conn.Table<Child>().Where(x => x.AZMAdministratorPhone == login.PhoneNo && x.Completed == 1 && x.SettlementName == login.Settlement).OrderByDescending(x => x.Id).ToList();
        //        if (string.IsNullOrWhiteSpace(SearchItem))
        //        {
        //            HHLineList.ItemsSource = AllChildren;
        //            noResultsLabel.IsVisible = false;
        //            return;
        //        }
        //        var filteredChildren = AllChildren.Where(child =>
        //        (child.ChildName?.ToLower().Contains(SearchItem?.ToLower()) ?? false) ||
        //        (child.HouseHoldPhone?.ToString().Contains(SearchItem) ?? false) ||
        //        (child.NameOfHouseHoldHead?.ToLower().Contains(SearchItem?.ToLower()) ?? false) ||
        //        (child.HouseID?.ToLower().Contains(SearchItem?.ToLower()) ?? false)
        //        ).ToList();

        //        HHLineList.ItemsSource = filteredChildren;
        //        countTotalLbl.Text = filteredChildren.Count.ToString() + " Records";
        //        noResultsLabel.IsVisible = !filteredChildren.Any();

        //    }
        //}

        //private List<OOSList> GetChildrenList(OOSList searchEntity)
        //{
        //    List<OOSList> list = new List<OOSList>();

        //    using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
        //    {
        //        conn.CreateTable<OOSList>();

        //        if (string.IsNullOrEmpty(searchEntity.ChildID))
        //        {

        //            list = conn.Table<OOSList>()
        //                .Where(x => x.TeamCode == TeamCode && x.SettlementName == Settlement && x.Completed == 0)
        //                .OrderByDescending(x => x.Id)
        //                .ToList()
        //                .GroupBy(x => x.ChildID)
        //                .Select(g => g.First())
        //                .ToList(); ;
        //        }
        //        else
        //        {
        //            list = conn.Table<OOSList>()
        //               .Where(x => x.TeamCode == TeamCode && x.SettlementName == Settlement && x.Completed == 0 && x.ChildID.ToLower().Contains(searchEntity.ChildID.ToLower()))
        //               .OrderByDescending(x => x.Id)
        //               .ToList()
        //               .GroupBy(x => x.ChildID)
        //               .Select(g => g.First())
        //               .ToList(); ;
        //        }

        //    }

        //    return list;
        //}


        private List<OOSList> GetChildrenList(string searchText)
        {
            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                conn.CreateTable<OOSList>();

                var query = conn.Table<OOSList>()
                                .Where(x => x.TeamCode == TeamCode && x.Completed == 0);

                if (!string.IsNullOrWhiteSpace(searchText))
                {
                    searchText = searchText.ToLower();

                    query = query.Where(x =>
                        (x.ChildID != null && x.ChildID.ToLower().Contains(searchText)) ||
                        (x.ChildName != null && x.ChildName.ToLower().Contains(searchText)) ||
                        (x.SettlementName != null && x.SettlementName.ToLower().Contains(searchText))
                    );
                }

                return query
                    .OrderByDescending(x => x.Id)
                    .ToList()
                    .GroupBy(x => x.ChildID)
                    .Select(g => g.First())
                    .ToList();
            }
        }

        async private void onToggleDownload(object sender, ToggledEventArgs e)
        {

            try
            {

                // This method will be called whenever the switch is toggled
                if (e.Value) // Switch is On
                {
                    activityIndicator.IsVisible = true;
                    activityIndicator.IsRunning = true;
                    feedbackLabel.IsVisible = true;
                    feedbackLabel.Text = "Download of LIne list started....";
                    // Begin make the call to pull linelist

                    string apiUrl = "http://azmda.com.ng/KTOOS/igetLineList.php";
                    string teamCode = TeamCode;
                    string settlement = Settlement;

                    var client = new HttpClient();
                    var content = new StringContent(JsonConvert.SerializeObject(new
                    {
                        TeamCode = teamCode,
                        SettlementName = settlement
                    }), Encoding.UTF8, "application/json");

                    var response = await client.PostAsync(apiUrl, content);
                    var responseString = await response.Content.ReadAsStringAsync();

                    List<OOSList> OOSData = JsonConvert.DeserializeObject<List<OOSList>>(responseString);

                    using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                    {

                        //process data here
                        foreach (var record in OOSData)
                        {

                            //check if data exists, else add
                            conn.CreateTable<OOSList>();
                            var oos = conn.Table<OOSList>()
                                .Where(x => x.ChildID == record.ChildID && x.Completed == 0).OrderByDescending(x => x.Id).ToList(); // updated after stable version

                            if (oos.Count > 0)
                            {

                            }
                            else
                            {
                                //add to the database
                                OOSList ooslist = new OOSList
                                {

                                    VaccinatorName = record.VaccinatorName.Trim(),
                                    VaccinatorNumber = record.VaccinatorNumber.Trim(),
                                    TeamCode = record.TeamCode.Trim(),
                                    Respondent = record.Respondent.Trim(),
                                    HouseHoldHeadName = record.HouseHoldHeadName.Trim(),
                                    HouseHoldPhone = record.HouseHoldPhone.Trim(),
                                    CaregiverName = record.CaregiverName.Trim(),
                                    ChildID = record.ChildID.Trim(),
                                    ChildName = record.ChildName.Trim(),
                                    Gender = record.Gender.Trim(),
                                    HasReceivedAntigen = record.HasReceivedAntigen.Trim(),
                                    OldAntigensReceived = record.OldAntigensReceived.Trim(),
                                    AntigensReceived = record.AntigensReceived.Trim(),
                                    AEFI = record.AEFI.Trim(),
                                    AEFIType = record.AEFIType.Trim(),
                                    Age = record.Age.Trim(),
                                    CurrentAge = record.CurrentAge.Trim(),
                                    AgeCategory = record.AgeCategory.Trim(),
                                    CaregiverNumber = record.CaregiverNumber.Trim(),
                                    CatchmentAreaHF = record.CatchmentAreaHF.Trim(),
                                    SettlementName = record.SettlementName.Trim(),
                                    LGA = record.LGA,
                                    Ward = record.Ward,
                                    SettlementType = record.SettlementType,
                                    Latitude = record.Latitude,
                                    Longitude = record.Longitude,
                                    Completed = 0,
                                    Date = "",
                                    Time = "",
                                    Temp = "",
                                    DueForNextAntigen = record.DueForNextAntigen,
                                    isCheckedForSync = 0,
                                    uploaded = 0,
                                    VaccinationStatus = record.VaccinationStatus,
                                    TargetStatus = "Online"

                                };
                                int rows = conn.Insert(ooslist);

                            }

                        }

                        //End make the call to pull linelist

                        conn.CreateTable<OOSList>();
                        var newoos = conn.Table<OOSList>().ToList();
                        if (newoos.Count > 0)
                        {
                            feedbackLabel.Text = "Download of LIne list Completed....";
                        }
                        else
                        {
                            feedbackLabel.Text = "No new Record to Download....";
                        }

                    }

                    // Clear feedback after a delay
                    await Task.Delay(1000); // Optional delay before clearing message

                    using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                    {
                        conn.CreateTable<OOSList>();
                        //var linelists = conn.Table<OOSList>().ToList();
                        var linelists = conn.Table<OOSList>()
                            .Where(x => x.TeamCode == TeamCode && x.SettlementName == Settlement && x.Completed == 0).OrderByDescending(x => x.Id).ToList(); // updated after stable version

                        string totalRecord = linelists.Count.ToString();
                        ChildrenLineList.ItemsSource = linelists;
                        countTotalLbl.Text = totalRecord + " U2 Children";
                    }

                    downloadLineList.IsToggled = false;
                    activityIndicator.IsRunning = false;
                    activityIndicator.IsVisible = false;
                    feedbackLabel.IsVisible = false;
                }
                if (!e.Value) // Switch is Off
                {
                    //do nothing. This is switching the toggle off
                }
            }
            catch (HttpRequestException ex)
            {
                // This catches the Java.Net.ConnectException and shows a friendly message
                feedbackLabel.Text = "Network error: Unable to connect to the server. Please check your internet connection.";
                downloadLineList.IsToggled = false;
                activityIndicator.IsRunning = false;
                activityIndicator.IsVisible = false;
                feedbackLabel.IsVisible = false;
            }
            catch (System.Net.Sockets.SocketException ex)
            {
                // Catch lower-level socket errors
                feedbackLabel.Text = "Network Error: Network connectivity issue. Please try again later.";
                downloadLineList.IsToggled = false;
                activityIndicator.IsRunning = false;
                activityIndicator.IsVisible = false;
                feedbackLabel.IsVisible = false;
            }
            catch (Exception ex)
            {
                // Generic catch for any other types of exceptions
                feedbackLabel.Text = "Network error: Unable to connect. Please check your internet connection.";
                downloadLineList.IsToggled = false;
                activityIndicator.IsRunning = false;
                activityIndicator.IsVisible = false;
                feedbackLabel.IsVisible = false;
            }


        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            var lga = LGA.ToUpper();
            var retlga = lga.Substring(0, 3);
            string totalRecord = "";
            var ward = Ward.ToUpper();
            var retward = ward.Substring(0, 3);
            string searchParameter = searchBar.Text = "IEV/" + retlga + "/" + retward + "/";
            //if (searchEntity.ChildID.Length <= 12)
            //{
            //    using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            //    {
            //        conn.CreateTable<OOSList>();
            //        //var linelists = conn.Table<OOSList>().ToList();
            //        var linelists = conn.Table<OOSList>()
            //         .Where(x => x.TeamCode == TeamCode && x.SettlementName == Settlement && x.Completed == 0)
            //         .OrderByDescending(x => x.Id)
            //         .ToList()
            //         .GroupBy(x => x.ChildID)
            //        .Select(g => g.First())
            //        .ToList(); ; // updated after stable version

            //        totalRecord = linelists.Count.ToString();
            //        ChildrenLineList.ItemsSource = linelists;
            //        countTotalLbl.Text = totalRecord + " U2 Children";
            //    }

            //}
            //else
            //{
                var linelist = GetChildrenList(searchParameter);

                totalRecord = linelist.Count.ToString();
                ChildrenLineList.ItemsSource = linelist;
                countTotalLbl.Text = totalRecord + " U2 Children";
            //}

        }

        async public void GotoDirection(decimal lat, decimal longi)
        {
            double latitude = Convert.ToDouble(lat);
            double longitude = Convert.ToDouble(longi);

            //await Map.OpenAsync(12.97360882, 7.570604637);

            await Map.OpenAsync(latitude, longitude, new MapLaunchOptions
            {
                Name = "Name here",
                NavigationMode = NavigationMode.Default
            });

        }


        void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            var button = sender as Button;
            var item = button?.CommandParameter as OOSList;

            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                conn.CreateTable<OOSList>();
                OOSList record = conn.Table<OOSList>().Where(x => x.Id == item.Id).FirstOrDefault();


                decimal lat = Convert.ToDecimal(record.Latitude);
                decimal longi = Convert.ToDecimal(record.Longitude);

                GotoDirection(lat, longi);
            }

        }

        void Button_Vaccinate(System.Object sender, System.EventArgs e)
        {
            var button = sender as Button;
            var item = button?.CommandParameter as OOSList;

            helper.InterviewerName = InterviewerName;
            helper.PhoneNo = PhoneNumber;
            helper.TeamCode = TeamCode;

            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                conn.CreateTable<OOSList>();
                OOSList record = conn.Table<OOSList>().Where(x => x.Id == item.Id).FirstOrDefault();
                record.Temp = TeamCode; //used to temporarily hold the teamcode of a logged in user

                Navigation.PushAsync(new VaccinationPage(record, helper));

            }
        }


        void VaccineUtilization(System.Object sender, System.EventArgs e)
        {
            Login newChildEnumeratorDetails = new Login();

            newChildEnumeratorDetails.InterviewerName = this.InterviewerName;
            newChildEnumeratorDetails.PhoneNo = this.PhoneNumber;
            newChildEnumeratorDetails.TeamCode = this.TeamCode;
            newChildEnumeratorDetails.HealthFacility = this.HealthFacility;
            newChildEnumeratorDetails.Settlement = this.Settlement;
            newChildEnumeratorDetails.LGA = this.LGA;
            newChildEnumeratorDetails.Ward = this.Ward;
            Navigation.PushAsync(new VaccineUtilization(newChildEnumeratorDetails));
        }

        void vaccination_log(System.Object sender, System.EventArgs e)
        {
            //Navigation.PushAsync(new VaccinLogPage(PhoneNumber));
            Navigation.PushAsync(new VaccinLogPage());
        }

        void EnumerateChild_Clicked(System.Object sender, System.EventArgs e)
        {
            Login newChildEnumeratorDetails = new Login();

            newChildEnumeratorDetails.InterviewerName = this.InterviewerName;
            newChildEnumeratorDetails.PhoneNo = this.PhoneNumber;
            newChildEnumeratorDetails.TeamCode = this.TeamCode;
            newChildEnumeratorDetails.HealthFacility = this.HealthFacility;
            newChildEnumeratorDetails.Settlement = this.Settlement;
            newChildEnumeratorDetails.LGA = this.LGA;
            newChildEnumeratorDetails.Ward = this.Ward;

            Navigation.PushAsync(new NewEnumeratePage(newChildEnumeratorDetails));
        }

        //void searchBar_TextChanged(System.Object sender, Xamarin.Forms.TextChangedEventArgs e)
        //{
        //    var search = sender as SearchBar;
        //    searchEntity.ChildID = search.Text.ToString();
        //    List<OOSList> linelist = new List<OOSList>();
        //    string totalRecord = "";

        //    if (searchEntity.ChildID.Length <= 12)
        //    {
        //        using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
        //        {
        //            conn.CreateTable<OOSList>();
        //            var linelists = conn.Table<OOSList>()
        //                .Where(x => x.TeamCode == TeamCode && x.SettlementName == Settlement && x.Completed == 0).OrderByDescending(x => x.Id).ToList(); // updated after stable version

        //            totalRecord = linelists.Count.ToString();
        //            ChildrenLineList.ItemsSource = linelists;
        //            countTotalLbl.Text = totalRecord + " U2 Children";
        //        }

        //    }
        //    else
        //    {
        //        linelist = GetChildrenList(searchEntity);
        //        totalRecord = linelist.Count.ToString();
        //        ChildrenLineList.ItemsSource = linelist;
        //        countTotalLbl.Text = totalRecord + " U2 Children";
        //    }

        //}


        void searchBar_TextChanged(System.Object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            var search = sender as SearchBar;
            //searchEntity.ChildID = search.Text.ToString();
            string searchParamenter = search.Text.ToString();
            List<OOSList> linelist = new List<OOSList>();
            string totalRecord = "";

            //if (searchEntity.ChildID.Length <= 12)
            //{
            //    using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            //    {
            //        conn.CreateTable<OOSList>();
            //        var linelists = conn.Table<OOSList>()
            //            .Where(x => x.TeamCode == TeamCode && x.SettlementName == Settlement && x.Completed == 0).OrderByDescending(x => x.Id).ToList(); // updated after stable version

            //        totalRecord = linelists.Count.ToString();
            //        ChildrenLineList.ItemsSource = linelists;
            //        countTotalLbl.Text = totalRecord + " U2 Children";
            //    }

            //}
            //else
            //{
                linelist = GetChildrenList(searchParamenter);
                totalRecord = linelist.Count.ToString();
                ChildrenLineList.ItemsSource = linelist;
                countTotalLbl.Text = totalRecord + " U2 Children";
            //}

        }


        async void ToolbarItem_Clicked(System.Object sender, System.EventArgs e)
        {
            string result = await DisplayPromptAsync("Enter Passcode", "If you are sure you want to DELETE Enter Pass code:",
                                                    accept: "OK", cancel: "Cancel",
                                                    maxLength: 4, keyboard: Keyboard.Numeric);

            if (result != null)
            {
                // Passcode entered and OK clicked
                if (result == "0101")
                {
                    await DisplayAlert("Success", "Passcode correct!, please wait....", "OK");

                    try
                    {
                        using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                        {
                            conn.Execute("DROP TABLE IF EXISTS OOSList");

                            //create again
                            conn.CreateTable<OOSList>();
                            await Navigation.PopAsync();
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }

        }


        public class LoginHelper
        {
            public string InterviewerName { get; set; }

            public string PhoneNo { get; set; }

            public string TeamCode { get; set; }

            public string HealthFacility { get; set; }

            public string Settlement { get; set; }
        }
    }
}

