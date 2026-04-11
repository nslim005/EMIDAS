using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.Net.Http;
using SQLite;
using Xamarin.Forms;
using ZeroDoseMetrics.Model;
using System.Threading.Tasks;

namespace ZeroDoseMetrics.OOSZamfara
{	
	public partial class OOSLandingPage : ContentPage
	{
        Login isExist;
        OOSList localDBRecord;
        private List<LGAItem> lgaItems;
        private List<WardItem> wardItems;
        private List<HFItem> hfItems;
        private List<TeamCodeItem> teamCodeItems;
        private List<SettlementItem> settlementItems;
        private List<AppVersion> version;

        public OOSLandingPage ()
		{
			InitializeComponent ();
            isExist = new Login();
            localDBRecord = new OOSList();
            lgaItems = new List<LGAItem>();
            wardItems = new List<WardItem>();
            hfItems = new List<HFItem>();
            teamCodeItems = new List<TeamCodeItem>();
            settlementItems = new List<SettlementItem>();
            version = new List<AppVersion>();

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                //App Version:
                conn.CreateTable<AppVersion>();
                AppVersion appversion = GetAppVersion();
                if (appversion != null && !string.IsNullOrEmpty(appversion.Version))
                {
                    AppLabel.Text = "APP VERSION: " + appversion.Version;
                }

                conn.CreateTable<LGA>();
                var lgaList = GetLGA();
                //if (HFPickerLGA.SelectedIndex == -1)
                if (HFPickerLGA != null && HFPickerLGA.SelectedIndex == -1)
                {
                    if (HFPickerLGA.Items.Count == 0)
                    {
                        lgaItems = lgaList
                        .Where(lga => lga.Status == 1).Select(lga => new LGAItem { LGAId = lga.Id, LGAName = lga.LGAName }).ToList();
                        HFPickerLGA.ItemsSource = lgaItems;
                        HFPickerLGA.ItemDisplayBinding = new Binding("LGAName");
                    }
                }
                else
                {
                    foreach (var item in lgaList)
                    {

                        if (HFPickerLGA.Items.Contains(item.LGAName))
                        {
                            //do nothiong
                        }
                        else
                        {
                            HFPickerLGA.Items.Add(item.LGAName);
                        }

                    }
                }
            }

        }

        public List<CAHF> GetCAHF()
        {
            List<CAHF> list;

            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                list = conn.Table<CAHF>().Where(x=>x.Status == 1).ToList();
            }

            return list;
        }

        public List<LGA> GetLGA()
        {
            List<LGA> list;

            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                list = conn.Table<LGA>().Where(x => x.Status == 1).ToList();
            }

            return list;
        }

        public List<Ward> GetWard()
        {
            List<Ward> list;

            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                list = conn.Table<Ward>().Where(x => x.Status == 1).ToList();
            }

            return list;
        }

        public List<Team> GetTeam()
        {
            List<Team> list;

            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                list = conn.Table<Team>().Where(x => x.Status == 1).ToList();
            }

            return list;
        }

        public List<Settlement> GetSettlement()
        {
            List<Settlement> list;

            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                list = conn.Table<Settlement>().Where(x => x.Status == 1).ToList();
            }

            return list;
        }

        public List<Passcode> GetPassCode()
        {
            List<Passcode> list;

            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                list = conn.Table<Passcode>().ToList();
            }

            return list;
        }

        public AppVersion GetAppVersion()
        {
            AppVersion ret;

            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                ret = conn.Table<AppVersion>().FirstOrDefault();
            }

            return ret;
        }


        void loginButton_Clicked(System.Object sender, System.EventArgs e)
        {

            bool isInterviewerEmpty = string.IsNullOrEmpty(interviewerNameEntry.Text);
            bool isPhoneNumberEmpty = string.IsNullOrEmpty(interviewerPhoneNoEntry.Text);
            string isTeamCodeEmpty = TeamCodePicker.SelectedIndex.ToString();
            string isHFEmpty = HFPicker.SelectedIndex.ToString();
            string isSettlementEmpty = SettlementPicker.SelectedIndex.ToString();
            string isWardEmpty = HFPickerWard.SelectedIndex.ToString();
            string isLGAEmpty = HFPickerLGA.SelectedIndex.ToString();

            //validation

            if (isInterviewerEmpty || isPhoneNumberEmpty)
            {
                DisplayAlert("ERROR LOGIN", "ALL FIELDS ARE REQUIRED FOR LOGIN", "OK");
            }
            else if (isTeamCodeEmpty == "-1" || isHFEmpty == "-1" || isSettlementEmpty == "-1" || isWardEmpty == "-1" || isLGAEmpty == "-1")
            {
                DisplayAlert("ERROR LOGIN", "ALL FIELDS ARE REQUIRED FOR LOGIN", "OK");
            }
            else if (!(interviewerPhoneNoEntry.Text.Length == 11))
            {
                DisplayAlert("ERROR PHONE NO", "PHONE NUMBER MUST BE 11 DIGITS", "OK");
            }
            else
            {
                string InterviewerName = interviewerNameEntry.Text.Trim().ToUpper();
                string interviewerPhoneNumber = interviewerPhoneNoEntry.Text;
                string teamCode = ((TeamCodeItem)TeamCodePicker.SelectedItem).TeamCodeName.Trim();
                string catchmentAreaHF = ((HFItem)HFPicker.SelectedItem).HFName.Trim();
                string settlementName = ((SettlementItem)SettlementPicker.SelectedItem).SettlementName;
                string lga = ((LGAItem)HFPickerLGA.SelectedItem).LGAName;
                string ward = ((WardItem)HFPickerWard.SelectedItem).WardName;

                Login model = new Login
                {
                    InterviewerName = InterviewerName,
                    PhoneNo = interviewerPhoneNumber,
                    TeamCode = teamCode,
                    HealthFacility = catchmentAreaHF,
                    Settlement = settlementName,
                    LGA = lga.Trim(),
                    Ward = ward.Trim()
                };

                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    //check if the record exists already 
                    conn.CreateTable<Login>();
                    isExist = conn.Table<Login>()
                        .Where(x => x.PhoneNo.Equals(interviewerPhoneNumber) && x.InterviewerName.Equals(InterviewerName)).FirstOrDefault();

                    if (isExist != null)
                    {
                        Navigation.PushAsync(new ChildrenLineListPage(model));
                    }
                    else
                    {
                        //else create user
                        conn.CreateTable<Login>();
                        int rows = conn.Insert(model);
                        Navigation.PushAsync(new ChildrenLineListPage(model));
                    }
                }
            }


        }

        //LGA Operation
        void HFPickerLGA_SelectedIndexChanged(System.Object sender, System.EventArgs e)
        {
            var picker = sender as Picker;
            //HFPickerWard.Items.Clear();
            //HFPicker.Items.Clear();
            //TeamCodePicker.Items.Clear();
            //SettlementPicker.Items.Clear();

            if (HFPickerLGA.SelectedIndex != -1)
            {
                //Retrieval
                var selectedLGA = lgaItems[HFPickerLGA.SelectedIndex];
                int selectedLgaId = selectedLGA.LGAId;
                string selectedLgaName = selectedLGA.LGAName;

                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    List<Ward> wardlist = GetWard();

                    wardItems = wardlist.Where(x => x.LGAId == selectedLgaId).Select(ward => new WardItem { WardId = ward.Id, WardName = ward.WardName }).ToList();
                    HFPickerWard.ItemsSource = wardItems;
                    HFPickerWard.ItemDisplayBinding = new Binding("WardName");

                }

            }

        }

        // Ward Operation
        void HFPickerWard_SelectedIndexChanged(System.Object sender, System.EventArgs e)
        {
            var picker = sender as Picker;

            if (picker.SelectedIndex == -1)
            {

            }
            else
            {
                var selectedWard = wardItems[HFPickerWard.SelectedIndex];
                int selectedWardId = selectedWard.WardId;
                string selectedWardName = selectedWard.WardName;
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    List<CAHF> hflist = GetCAHF();
                    hfItems = hflist.Where(x => x.WardId == selectedWardId).Select(hf => new HFItem { HFId = hf.Id, HFName = hf.CAHFName }).ToList();
                    HFPicker.ItemsSource = hfItems;
                    HFPicker.ItemDisplayBinding = new Binding("HFName");

                }
            }
        }

        //CAHF Operation
        void HFPicker_SelectedIndexChanged(System.Object sender, System.EventArgs e)
        {

            var picker = sender as Picker;

            if (picker.SelectedIndex == -1)
            {

            }
            else
            {
                var selectedHF = hfItems[HFPicker.SelectedIndex];
                int selectedHFId = selectedHF.HFId;
                string selectedHFName = selectedHF.HFName;
                //string sel = picker.SelectedItem.ToString();
                //TeamCodePicker.Items.Clear();
                //SettlementPicker.Items.Clear();
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {

                    List<Team> teamlist = GetTeam();
                    teamCodeItems = teamlist.Where(x => x.CAHFId == selectedHFId).Select(team => new TeamCodeItem { TeamCodeId = team.Id, TeamCodeName = team.TeamCode }).ToList();
                    TeamCodePicker.ItemsSource = teamCodeItems;
                    TeamCodePicker.ItemDisplayBinding = new Binding("TeamCodeName");
                }
            }

        }

        //Team Operation
        void TeamCodePicker_SelectedIndexChanged(System.Object sender, System.EventArgs e)
        {

            var picker = sender as Picker;

            if (picker.SelectedIndex == -1)
            {

            }
            else
            {
                var selectedTeamCode = teamCodeItems[TeamCodePicker.SelectedIndex];
                int selectedTeamID = selectedTeamCode.TeamCodeId;
                string selectedTeamCodeName = selectedTeamCode.TeamCodeName;
                string sel = picker.SelectedItem.ToString();
                //SettlementPicker.Items.Clear();
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    List<Settlement> settlementlist = GetSettlement();
                    settlementItems = settlementlist.Where(x => x.TeamId == selectedTeamID).Select(settlement => new SettlementItem { SettlementId = settlement.Id, SettlementName = settlement.SettlementName }).ToList();
                    SettlementPicker.ItemsSource = settlementItems;
                    SettlementPicker.ItemDisplayBinding = new Binding("SettlementName");

                    //var selected = conn.Table<Team>().Where(x => x.TeamCode == sel).FirstOrDefault();

                    //List<Settlement> cahf = GetSettlement().Where(x => x.TeamId == selected.Id).ToList();

                    //foreach (var item in cahf)
                    //{

                    //    SettlementPicker.Items.Add(item.SettlementName);
                    //}
                }
            }

        }

        void SettlementPicker_SelectedIndexChanged(System.Object sender, System.EventArgs e)
        {

        }

        async void configureBtn_Clicked(System.Object sender, System.EventArgs e)
        {

            //begin mirrow all the tables online
            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {

                using (var client = new HttpClient())
                {
                    try
                    {
                        //check if the table exists already else create
                        conn.CreateTable<CAHF>();
                        conn.CreateTable<LGA>();
                        conn.CreateTable<Settlement>();
                        conn.CreateTable<State>();
                        conn.CreateTable<Team>();
                        conn.CreateTable<Ward>();
                        conn.CreateTable<AppVersion>();


                        string CAHFurl = "http://azmda.com.ng/KTOOS/getCAHF.php";
                        string LGAurl = "http://azmda.com.ng/KTOOS/getLGA.php";
                        string Settlementurl = "http://azmda.com.ng/KTOOS/getSettlement.php";
                        string Stateurl = "http://azmda.com.ng/KTOOS/getState.php";
                        string Teamurl = "http://azmda.com.ng/KTOOS/getTeam.php";
                        string Wardurl = "http://azmda.com.ng/KTOOS/getWard.php";
                        string AppVersionURL = "http://azmda.com.ng/KTOOS/getAppVersion.php";

                        //string CAHFurl = "http://azmda.com.ng/ZMDEMOAPI/getCAHF.php";
                        //string LGAurl = "http://azmda.com.ng/ZMDEMOAPI/getLGA.php";
                        //string Settlementurl = "http://azmda.com.ng/ZMDEMOAPI/getSettlement.php";
                        //string Stateurl = "http://azmda.com.ng/ZMDEMOAPI/getState.php";
                        //string Teamurl = "http://azmda.com.ng/ZMDEMOAPI/getTeam.php";
                        //string Wardurl = "http://azmda.com.ng/ZMDEMOAPI/getWard.php";
                        //string AppVersionURL = "http://azmda.com.ng/ZMDEMOAPI/getAppVersion.php";

                        //string CAHFurl = "http://azmda.com.ng/ZMSPECIALINTERVENTION/getCAHF.php";
                        //string LGAurl = "http://azmda.com.ng/ZMSPECIALINTERVENTION/getLGA.php";
                        //string Settlementurl = "http://azmda.com.ng/ZMSPECIALINTERVENTION/getSettlement.php";
                        //string Stateurl = "http://azmda.com.ng/ZMSPECIALINTERVENTION/getState.php";
                        //string Teamurl = "http://azmda.com.ng/ZMSPECIALINTERVENTION/getTeam.php";
                        //string Wardurl = "http://azmda.com.ng/ZMSPECIALINTERVENTION/getWard.php";
                        //string AppVersionURL = "http://azmda.com.ng/ZMSPECIALINTERVENTION/getAppVersion.php";

                        //Delete and recreate all the tables
                        // Drop the table
                        conn.Execute("DROP TABLE IF EXISTS CAHF");
                        conn.Execute("DROP TABLE IF EXISTS LGA");
                        conn.Execute("DROP TABLE IF EXISTS Settlement");
                        conn.Execute("DROP TABLE IF EXISTS State");
                        conn.Execute("DROP TABLE IF EXISTS Team");
                        conn.Execute("DROP TABLE IF EXISTS Ward");
                        conn.Execute("DROP TABLE IF EXISTS AppVersion");

                        //Recreate table
                        conn.CreateTable<CAHF>();
                        conn.CreateTable<LGA>();
                        conn.CreateTable<Settlement>();
                        conn.CreateTable<State>();
                        conn.CreateTable<Team>();
                        conn.CreateTable<Ward>();
                        conn.CreateTable<AppVersion>();


                        // Show loading spinner and message
                        activityIndicator.IsRunning = true;
                        activityIndicator.IsVisible = true;
                        feedbackLabel.Text = "Setting up, please wait...";

                        //CAHF DATA//
                        HttpResponseMessage CAHFresponse = await client.GetAsync(CAHFurl);
                        CAHFresponse.EnsureSuccessStatusCode();
                        string CAHFresponseBody = await CAHFresponse.Content.ReadAsStringAsync();

                        // Deserialize the JSON response
                        List<CAHF> CAHFdata = JsonConvert.DeserializeObject<List<CAHF>>(CAHFresponseBody);


                        // Process the data

                        foreach (var record in CAHFdata)
                        {
                            CAHF cahf = new CAHF
                            {
                                LGAId = record.LGAId,
                                WardId = record.WardId,
                                Status = record.Status,
                                CAHFName = record.CAHFName

                            };
                            int rows = conn.Insert(cahf);
                        }

                        //LGA DATA//
                        HttpResponseMessage LGAresponse = await client.GetAsync(LGAurl);
                        LGAresponse.EnsureSuccessStatusCode();
                        string LGAresponseBody = await LGAresponse.Content.ReadAsStringAsync();

                        // Deserialize the JSON response
                        List<LGA> LGAdata = JsonConvert.DeserializeObject<List<LGA>>(LGAresponseBody);

                        // Process the data

                        foreach (var record in LGAdata)
                        {
                            LGA lga = new LGA
                            {
                                StateId = record.StateId,
                                LGAName = record.LGAName,
                                Status = record.Status
                            };
                            int rows = conn.Insert(lga);


                        }
                        var lgaList = conn.Table<LGA>().ToList();
                        //HFPickerLGA.Items.Clear();

                        //lgaItems = lgaList.Select(lga => new LGAItem { LGAId = lga.Id, LGAName = lga.LGAName }).ToList();
                        //HFPickerLGA.ItemsSource = lgaItems;
                        //HFPickerLGA.ItemDisplayBinding = new Binding("LGAName");


                        //Settlement DATA//
                        HttpResponseMessage Settlementresponse = await client.GetAsync(Settlementurl);
                        Settlementresponse.EnsureSuccessStatusCode();
                        string SettlementresponseBody = await Settlementresponse.Content.ReadAsStringAsync();

                        // Deserialize the JSON response
                        List<Settlement> Settlementdata = JsonConvert.DeserializeObject<List<Settlement>>(SettlementresponseBody);

                        // Process the data

                        foreach (var record in Settlementdata)
                        {
                            Settlement settlement = new Settlement
                            {
                                LGAId = record.LGAId,
                                SettlementName = record.SettlementName,
                                TeamId = record.TeamId,
                                WardId = record.WardId,
                                CAHFId = record.CAHFId,
                                Status = record.Status
                            };
                            int rows = conn.Insert(settlement);
                        }

                        //State DATA//
                        HttpResponseMessage Stateresponse = await client.GetAsync(Stateurl);
                        Stateresponse.EnsureSuccessStatusCode();
                        string SateresponseBody = await Stateresponse.Content.ReadAsStringAsync();

                        // Deserialize the JSON response
                        List<State> Statedata = JsonConvert.DeserializeObject<List<State>>(SateresponseBody);

                        // Process the data

                        foreach (var record in Statedata)
                        {
                            State state = new State
                            {
                                StateName = record.StateName,
                                Status = record.Status
                            };
                            int rows = conn.Insert(state);
                        }

                        //Team DATA//
                        HttpResponseMessage Teamresponse = await client.GetAsync(Teamurl);
                        Teamresponse.EnsureSuccessStatusCode();
                        string TeamresponseBody = await Teamresponse.Content.ReadAsStringAsync();

                        // Deserialize the JSON response
                        List<Team> Teamdata = JsonConvert.DeserializeObject<List<Team>>(TeamresponseBody);

                        // Process the data

                        foreach (var record in Teamdata)
                        {
                            Team team = new Team
                            {
                                CAHFId = record.CAHFId,
                                LGAId = record.LGAId,
                                TeamCode = record.TeamCode,
                                WardId = record.WardId,
                                Status = record.Status
                            };
                            int rows = conn.Insert(team);
                        }

                        //Ward DATA//
                        HttpResponseMessage Wardresponse = await client.GetAsync(Wardurl);
                        Wardresponse.EnsureSuccessStatusCode();
                        string WardresponseBody = await Wardresponse.Content.ReadAsStringAsync();

                        // Deserialize the JSON response
                        List<Ward> Warddata = JsonConvert.DeserializeObject<List<Ward>>(WardresponseBody);

                        // Process the data

                        foreach (var record in Warddata)
                        {
                            Ward ward = new Ward
                            {
                                LGAId = record.LGAId,
                                WardName = record.WardName,
                                Status = record.Status
                            };
                            int rows = conn.Insert(ward);
                        }


                        //AppVersion DATA//
                        HttpResponseMessage AppVersionResponse = await client.GetAsync(AppVersionURL);
                        AppVersionResponse.EnsureSuccessStatusCode();
                        string AppVersionResponseBody = await AppVersionResponse.Content.ReadAsStringAsync();

                        // Deserialize the JSON response
                        List<AppVersion> AppVersionData = JsonConvert.DeserializeObject<List<AppVersion>>(AppVersionResponseBody);

                        // Process the data

                        foreach (var record in AppVersionData)
                        {
                            AppVersion version = new AppVersion
                            {
                                Version = record.Version,
                                Status = record.Status
                            };
                            int rows = conn.Insert(version);
                        }

                        AppVersion appversion = GetAppVersion();
                        if (appversion != null && !string.IsNullOrEmpty(appversion.Version))
                        {
                            AppLabel.Text = "APP VERSION: " + appversion.Version;
                        }
                        feedbackLabel.Text = "Setup completed.";
                    }
                    catch (HttpRequestException ex)
                    {
                        // This catches the Java.Net.ConnectException and shows a friendly message
                        feedbackLabel.Text = "Network error: Unable to connect to the server. Please check your internet connection.";
                    }
                    catch (System.Net.Sockets.SocketException ex)
                    {
                        // Catch lower-level socket errors
                        feedbackLabel.Text = "Network Error: Network connectivity issue. Please try again later.";
                    }
                    catch (Exception ex)
                    {
                        // Generic catch for any other types of exceptions
                        //feedbackLabel.Text = $"Error: {ex.Message}";
                        feedbackLabel.Text = "Network error: Unable to connect to the server. Please check your internet connection.";
                    }
                    finally
                    {
                        // Hide loading spinner after the API call
                        activityIndicator.IsRunning = false;
                        activityIndicator.IsVisible = false;

                        // Clear feedback after a delay
                        await Task.Delay(3000); // Optional delay before clearing message
                        feedbackLabel.Text = "";
                    }

                }

            }
        }

    }

    public class LGAItem
    {
        public int LGAId { get; set; }
        public string LGAName { get; set; }
    }

    public class WardItem
    {
        public int WardId { get; set; }
        public string WardName { get; set; }
    }

    public class HFItem
    {
        public int HFId { get; set; }
        public string HFName { get; set; }
    }

    public class TeamCodeItem
    {
        public int TeamCodeId { get; set; }
        public string TeamCodeName { get; set; }
    }

    public class SettlementItem
    {
        public int SettlementId { get; set; }
        public string SettlementName { get; set; }
    }

    public class Versionn
    {
        public int Id { get; set; }
        public string VersionName { get; set; }
    }
}

