using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using DocumentFormat.OpenXml.Bibliography;
using System.Net.Http;
using DocumentFormat.OpenXml.Office2010.ExcelAc;
using SQLite;
using Xamarin.Forms;
using ZeroDoseMetrics.Model;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace ZeroDoseMetrics.OOSZamfara
{	
	public partial class DefaulterLinlistPage : ContentPage
	{
        private ObservableCollection<DefaulterList> Item;
        //public string TeamCode { get; set; }
        //public string Settlement { get; set; }
        public string HealthFacility { get; set; }
        public string InterviewerName { get; set; }
        public string PhoneNumber { get; set; }
        public string UserId { get; set; }
        public string LGA { get; set; }
        public string Ward { get; set; }
        public DefaulterList searchEntity { get; set; }
        public Login helper { get; set; }


        public DefaulterLinlistPage (Login login)
		{
			InitializeComponent ();
            Item = new ObservableCollection<DefaulterList>();
            //DefaulterLinlistPage.ItemsSource = Item;
            this.HealthFacility = login.HealthFacility;
            this.PhoneNumber = login.PhoneNo;
            this.InterviewerName = login.InterviewerName;
            this.LGA = login.LGA;
            this.Ward = login.Ward;
            searchEntity = new DefaulterList();
            helper = new Login();
            downloadLineList.Toggled += onToggleDownload; // Add an event handler for the Toggled event
            OnAppearing();
        }

        void searchBar_TextChanged(System.Object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            var search = sender as SearchBar;
            searchEntity.ChildID = search.Text.ToString();
            List<DefaulterList> linelist = new List<DefaulterList>();
            string totalRecord = "";

            if (searchEntity.ChildID.Length <= 12)
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<DefaulterList>();
                    var linelists = conn.Table<DefaulterList>()
                        .Where(x => x.CatchmentAreaHF == HealthFacility && (x.Completed == 10 || x.Completed == 11))
                        .OrderByDescending(x => x.Id)
                        .ToList()
                        .GroupBy(x => x.ChildID)
                        .Select(g => g.First())
                        .ToList();// updated after stable version

                    totalRecord = linelists.Count.ToString();
                    ChildrenLineList.ItemsSource = linelists;
                    countTotalLbl.Text = totalRecord + " Defaulter(s)";
                }

            }
            else
            {
                linelist = GetChildrenList(searchEntity);
                totalRecord = linelist.Count.ToString();
                ChildrenLineList.ItemsSource = linelist;
                countTotalLbl.Text = totalRecord + " Defaulter(s)";
            }

        }



        void EnumerateChild_Clicked(System.Object sender, System.EventArgs e)
        {
            Login newChildEnumeratorDetails = new Login();

            newChildEnumeratorDetails.InterviewerName = this.InterviewerName;
            newChildEnumeratorDetails.PhoneNo = this.PhoneNumber;
            //newChildEnumeratorDetails.TeamCode = this.TeamCode;
            newChildEnumeratorDetails.HealthFacility = this.HealthFacility;
            //newChildEnumeratorDetails.Settlement = this.Settlement;
            newChildEnumeratorDetails.LGA = this.LGA;
            newChildEnumeratorDetails.Ward = this.Ward;

            Navigation.PushAsync(new NewDefaulterPage(newChildEnumeratorDetails));
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

                    string apiUrl = "http://azmda.com.ng/KTOOS/igetDefaulterChildLineList.php";
                    //string teamCode = TeamCode;
                    //string settlement = Settlement;

                    var client = new HttpClient();
                    var content = new StringContent(JsonConvert.SerializeObject(new
                    {
                        //TeamCode = teamCode,
                        //SettlementName = settlement
                    }), Encoding.UTF8, "application/json");

                    var response = await client.PostAsync(apiUrl, content);
                    var responseString = await response.Content.ReadAsStringAsync();

                    List<DefaulterList> OOSData = JsonConvert.DeserializeObject<List<DefaulterList>>(responseString);

                    using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                    {

                        //process data here
                        foreach (var record in OOSData)
                        {

                            //check if data exists, else add
                            conn.CreateTable<DefaulterList>();
                            var oos = conn.Table<DefaulterList>()
                                .Where(x => x.ChildID == record.ChildID && x.Completed == 0).OrderByDescending(x => x.Id).ToList(); // updated after stable version

                            if (oos.Count > 0)
                            {

                            }
                            else
                            {
                                //add to the database
                                DefaulterList DefaulterList = new DefaulterList
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
                                    TargetStatus = record.TargetStatus,

                                };
                                int rows = conn.Insert(DefaulterList);

                            }

                        }

                        //End make the call to pull linelist

                        conn.CreateTable<DefaulterList>();
                        var newoos = conn.Table<DefaulterList>().ToList();
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
                        conn.CreateTable<DefaulterList>();
                        //var linelists = conn.Table<DefaulterList>().ToList();
                        var linelists = conn.Table<DefaulterList>()
                            .Where(x => x.CatchmentAreaHF == HealthFacility && x.Completed == 0).OrderByDescending(x => x.Id).ToList(); // updated after stable version

                        string totalRecord = linelists.Count.ToString();
                        ChildrenLineList.ItemsSource = linelists;
                        countTotalLbl.Text = totalRecord + " Defaulter(s)";
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

        public async void sync_defaulters(System.Object sender, System.EventArgs e)
        {
            feedbackLabel.Text = "Synchronization to server started....";
            feedbackLabel.IsVisible = true;
            activityIndicator.IsVisible = true;
            activityIndicator.IsRunning = true;
            //string PhoneNo = InterviewerNo;

            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                conn.CreateTable<DefaulterList>(); // Assuming the table name is VaccinationRecord

                // Fetch records to synchronize
                List<DefaulterList> recordsToSync = conn.Table<DefaulterList>()
                    //.Where(x => x.Completed == 1 && x.isCheckedForSync == 1 && x.uploaded == 0) no need to check for isCheckedForSync anymore, just sync
                    .Where(x => x.Completed == 11 || x.Completed == 10)
                    .ToList();


                if (recordsToSync.Count > 0)
                {
                    // Serialize the records into JSON format
                    List<DefaulterList> updatedRecordForSync = new List<DefaulterList>();
                    foreach (var item in recordsToSync)
                    {
                        item.uploaded = 1;
                        updatedRecordForSync.Add(item);
                    }

                    try
                    {
                        string jsonString = System.Text.Json.JsonSerializer.Serialize(updatedRecordForSync);
                        // Prepare the HttpClient and request
                        var client = new HttpClient();
                        var request = new HttpRequestMessage(HttpMethod.Post, "http://azmda.com.ng/KTOOS/icreateDefaulter.php");
                        var content = new StringContent(jsonString, null, "application/json");
                        request.Content = content;

                        // Send the request
                        var response = await client.SendAsync(request);

                        if (response.IsSuccessStatusCode)
                        {
                            var responseCode = await response.Content.ReadAsStringAsync();

                            if (responseCode == "{\"status\":\"success\"}")
                            {
                                // Update 'uploaded' status for each record
                                //foreach (var record in recordsToSync)
                                //{
                                //    record.uploaded = 1;  // Set uploaded to 1 if successful
                                //    conn.Update(record);  // Update the record in the database
                                //}

                                feedbackLabel.IsVisible = false;
                                activityIndicator.IsVisible = false;
                                activityIndicator.IsRunning = false;
                                await DisplayAlert("Success", "Vaccination Record Synchronized successfully", "OK");
                            }
                            else
                            {
                                feedbackLabel.IsVisible = false;
                                activityIndicator.IsVisible = false;
                                activityIndicator.IsRunning = false;
                                await DisplayAlert("Error", "Server responded with an unexpected code: " + responseCode, "OK");

                            }
                        }
                        else
                        {
                            // If not a successful status code, mark records as not uploaded
                            foreach (var record in recordsToSync)
                            {
                                record.uploaded = 0;  // Set uploaded to 0 if unsuccessful
                                conn.Update(record);  // Update the record in the database
                            }
                            feedbackLabel.IsVisible = false;
                            activityIndicator.IsVisible = false;
                            activityIndicator.IsRunning = false;
                            await DisplayAlert("Error", "Ensure your network is good and try again.", "OK");
                        }
                    }
                    catch (HttpRequestException ex)
                    {
                        // Handle any exceptions that occurred during the request
                        foreach (var record in recordsToSync)
                        {
                            record.uploaded = 0;  // Set uploaded to 0 if there was an error
                            conn.Update(record);  // Update the record in the database
                        }
                        feedbackLabel.IsVisible = false;
                        activityIndicator.IsVisible = false;
                        activityIndicator.IsRunning = false;
                        await DisplayAlert("Error", "Network error: Unable to connect to the server." +
                        "Please check your internet connection: " + ex.Message, "OK");

                    }
                    catch (System.Net.Sockets.SocketException ex)
                    {
                        // Catch lower-level socket errors

                        feedbackLabel.IsVisible = false;
                        activityIndicator.IsVisible = false;
                        activityIndicator.IsRunning = false;
                        await DisplayAlert("network Error", "Network Error: Network connectivity issue. Please try again later.", "OK");
                    }
                    catch (Exception ex)
                    {
                        // Generic catch for any other types of exceptions
                        //feedbackLabel.Text = $"Error: {ex.Message}";
                        feedbackLabel.IsVisible = false;
                        activityIndicator.IsVisible = false;
                        activityIndicator.IsRunning = false;
                        await DisplayAlert("Network Error", "Network error: Unable to connect to the server. Please check your internet connection.", "");
                    }

                    //End Sync of DefaulterList


                    OnAppearing();
                }
                else
                {
                    await DisplayAlert("Error", "No record, atleast select one record to synchronize", "OK");
                    activityIndicator.IsVisible = false;
                    activityIndicator.IsRunning = false;
                }

            }

        }




        void OnclickCompliance(System.Object sender, System.EventArgs e)
        {
            //Navigation.PushAsync(new VaccinLogPage(PhoneNumber));
            //Navigation.PushAsync(new VaccinLogPage());
        }

        async void OnClickClearTestData(System.Object sender, System.EventArgs e)
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
                            conn.Execute("DROP TABLE IF EXISTS DefaulterList");

                            //create again
                            conn.CreateTable<DefaulterList>();
                            await Navigation.PopAsync();
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
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
            searchBar.Text = "DEF/" + retlga + "/" + retward + "/";
            if (searchEntity.ChildID.Length <= 12)
            {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<DefaulterList>();
                    //var linelists = conn.Table<DefaulterList>().ToList();
                    var linelists = conn.Table<DefaulterList>()
                    .Where(x => x.CatchmentAreaHF == HealthFacility && (x.Completed == 10 || x.Completed == 11))
                    .OrderByDescending(x => x.Id)
                    .ToList()
                    .GroupBy(x => x.ChildID)
                    .Select(g => g.First())
                    .ToList();// updated after stable version
                    totalRecord = linelists.Count.ToString();
                    ChildrenLineList.ItemsSource = linelists;
                    countTotalLbl.Text = totalRecord + " Defaulter(s)";
                }

            }
            else
            {
                var linelist = GetChildrenList(searchEntity);
                totalRecord = linelist.Count.ToString();
                ChildrenLineList.ItemsSource = linelist;
                countTotalLbl.Text = totalRecord + " Defaulter(s)";
            }

        }

        private List<DefaulterList> GetChildrenList(DefaulterList searchEntity)
        {
            List<DefaulterList> list = new List<DefaulterList>();

            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                conn.CreateTable<DefaulterList>();

                if (string.IsNullOrEmpty(searchEntity.ChildID))
                {
                    list = conn.Table<DefaulterList>()
                    .Where(x => x.CatchmentAreaHF == HealthFacility && (x.Completed == 10 || x.Completed == 11))
                    .OrderByDescending(x => x.Id)
                    .ToList()
                    .GroupBy(x => x.ChildID)
                    .Select(g => g.First())
                    .ToList();

                }
                else
                {
                    list = conn.Table<DefaulterList>()
                    //.Where(x => x.CatchmentAreaHF == HealthFacility && (x.Completed == 10 || x.Completed == 11)&& x.ChildID.ToLower().Contains(searchEntity.ChildID.ToLower())).OrderByDescending(x => x.Id).ToList();   
                    .Where(x => x.CatchmentAreaHF == HealthFacility && (x.Completed == 10 || x.Completed == 11) && x.ChildID.ToLower().Contains(searchEntity.ChildID.ToLower()))
                    .OrderByDescending(x => x.Id)
                    .ToList()
                    .GroupBy(x => x.ChildID)
                    .Select(g => g.First())
                    .ToList();

                }

            }

            return list;
        }


        void View_button(System.Object sender, System.EventArgs e)
        {
            var button = sender as Button;
            var item = button?.CommandParameter as DefaulterList;

            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                conn.CreateTable<DefaulterList>();
                DefaulterList record = conn.Table<DefaulterList>().Where(x => x.Id == item.Id).FirstOrDefault();
                Navigation.PushAsync(new ViewDefaulterAntigen(record));
            }

        }

        void Update_button(System.Object sender, System.EventArgs e)
        {
            var button = sender as Button;
            var item = button?.CommandParameter as DefaulterList;

            helper.InterviewerName = InterviewerName;
            helper.PhoneNo = PhoneNumber;
            //helper.TeamCode = TeamCode;

            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                conn.CreateTable<DefaulterList>();
                DefaulterList record = conn.Table<DefaulterList>().Where(x => x.Id == item.Id).FirstOrDefault();
                //record.Temp = TeamCode; //used to temporarily hold the teamcode of a logged in user

                Navigation.PushAsync(new UpdateDefaulterAntigen(record));

            }
        }


    }
}

