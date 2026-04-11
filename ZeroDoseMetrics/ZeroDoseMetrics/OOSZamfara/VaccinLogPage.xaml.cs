using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using ClosedXML.Excel;
using SQLite;
using Xamarin.Essentials;
using Xamarin.Forms;
using ZeroDoseMetrics.Model;
using static Xamarin.Forms.Internals.GIFBitmap;
using System.IO;
using Xamarin.Forms.PlatformConfiguration;


namespace ZeroDoseMetrics.OOSZamfara
{	
	public partial class VaccinLogPage : ContentPage
	{
        public string InterviewerNo { get; set; }

        public string Code { get; set; }

		public VaccinLogPage ()
		{
			InitializeComponent ();
            //InterviewerNo = interviewerNo;
            OnAppearing();
            //Synchronize_Clicked += OnClick();


        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                conn.CreateTable<OOSList>();
                //var linelists = conn.Table<OOSList>().Where(x => x.Completed == 1 && x.VaccinatorNumber == InterviewerNo && x.uploaded ==0).OrderByDescending(x => x.Id).ToList();
                var linelists = conn.Table<OOSList>().Where(x => x.Completed == 1 && x.uploaded == 0).OrderByDescending(x => x.Id).ToList();
                ChildrenLineList.ItemsSource = linelists;

            }

        }


        void Review_Button_Clicked(System.Object sender, System.EventArgs e)
        {
            var button = sender as Button;
            var item = button?.CommandParameter as OOSList;

            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {

                //Navigation.PushAsync(new VaccinationPage(record, helper));

            }
        }

        public void TriggerSynchronization()
        {
            Synchronize_Clicked(this, EventArgs.Empty);
        }

        public async void Synchronize_Clicked(System.Object sender, System.EventArgs e)
        {
            feedbackLabel.Text = "Synchronization to server started....";
            feedbackLabel.IsVisible = true;
            activityIndicator.IsVisible = true;
            activityIndicator.IsRunning = true;
            string PhoneNo = InterviewerNo;

            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                conn.CreateTable<OOSList>(); // Assuming the table name is VaccinationRecord

                // Fetch records to synchronize
                List<OOSList> recordsToSync = conn.Table<OOSList>()
                    //.Where(x => x.Completed == 1 && x.isCheckedForSync == 1 && x.uploaded == 0) no need to check for isCheckedForSync anymore, just sync
                    .Where(x => x.Completed == 1 && x.uploaded == 0)
                    .ToList();


                if (recordsToSync.Count > 0)
                {
                    // Serialize the records into JSON format
                    List<OOSList> updatedRecordForSync = new List<OOSList>();
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
                        var request = new HttpRequestMessage(HttpMethod.Post, "http://azmda.com.ng/KTOOS/icreate.php");
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
                                foreach (var record in recordsToSync)
                                {
                                    record.uploaded = 1;  // Set uploaded to 1 if successful
                                    conn.Update(record);  // Update the record in the database
                                }

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

                    //End Sync of OOSList
                
                   
                    OnAppearing();
                }
                else
                {
                    await DisplayAlert("Error", "Select atleast one record to synchronize", "OK");
                    activityIndicator.IsVisible = false;
                    activityIndicator.IsRunning = false;
                }

            }

        }


        //private async void Synchronize(List<OOSList> list)
        //{
        //    //BEGIN API CALL
        //    try
        //    {
        //        string jsonString = System.Text.Json.JsonSerializer.Serialize(list);

        //        var client = new HttpClient();
        //        var request = new HttpRequestMessage(HttpMethod.Post, "http://cloudbits.com.ng/DEMOAPI/icreate.php");
        //        var content = new StringContent(jsonString, null, "application/json");
        //        request.Content = content;
        //        var response = await client.SendAsync(request);
        //        response.EnsureSuccessStatusCode();
        //        //var statCode = response.StatusCode;
        //        Console.WriteLine(await response.Content.ReadAsStringAsync());
        //        Code = await response.Content.ReadAsStringAsync();
        //        //End API CALL
        //    }
        //    catch (Exception ex)
        //    {
        //        var message = ex.Message.ToUpper();
        //    }
            
        //}



        void checkedForSynchronization_CheckedChanged(System.Object sender, Xamarin.Forms.CheckedChangedEventArgs e)
        {
            var box = sender as CheckBox;
            var item = box?.BindingContext as OOSList;
           

            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {

                if(box.IsChecked == false)
                {
                    conn.CreateTable<OOSList>();
                    var ret = conn.Table<OOSList>().Where(x => x.Id == item.Id).First();
                    ret.isCheckedForSync = 0;
                    int rows = conn.Update(ret);
                }
                if(box.IsChecked == true)
                {
                    
                    conn.CreateTable<OOSList>();
                    var ret = conn.Table<OOSList>().Where(x => x.Id == item.Id).First();
                    ret.isCheckedForSync = 1;
                    int rows = conn.Update(ret);
                }
                
                
            }

            //OnAppearing();

        }

        private async void Export_Clicked(System.Object sender, System.EventArgs e)
        {
            //Passcode passcode = new Passcode();
            //using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            //{
            //    conn.CreateTable<Passcode>();
            //    passcode = conn.Table<Passcode>().FirstOrDefault();
            //}

                string result = await DisplayPromptAsync("Enter Passcode", "Please enter your passcode:",
                                                     accept: "OK", cancel: "Cancel",
                                                     maxLength: 4, keyboard: Keyboard.Numeric);

            if (result != null)
            {
                // Passcode entered and OK clicked
                if (result == "9101")
                {
                    await DisplayAlert("Success", "Passcode correct!, please wait....", "OK");
                    //PerformAction();
                    try
                    {
                        using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                        {
                            try
                            {
                                // Query the database
                                conn.CreateTable<OOSList>();
                                var data = conn.Table<OOSList>().Where(x => x.Completed == 1 && x.uploaded == 0).ToList();

                                if (data.Count > 0)
                                {
                                    // Create Excel file

                                    var x = CheckAndRequestStoragePermission();

                                    using (var workbook = new XLWorkbook())
                                    {
                                        var worksheet = workbook.Worksheets.Add("OOSList");

                                        // Add headers
                                        var properties = typeof(OOSList).GetProperties();
                                        for (int i = 0; i < properties.Length; i++)
                                        {
                                            worksheet.Cell(1, i + 1).Value = properties[i].Name;
                                        }

                                        // Add data
                                        for (int row = 0; row < data.Count; row++)
                                        {
                                            var item = data[row];
                                            for (int col = 0; col < properties.Length; col++)
                                            {
                                                worksheet.Cell(row + 2, col + 1).Value = properties[col].GetValue(item)?.ToString() ?? "";
                                            }
                                        }


                                        // Save the Excel file
                                        var fileName = $"OOSList_Export_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
                                        var filePath = App.ExcelExportLocation;
                                        var fullPath = Path.Combine(filePath, fileName);
                                        workbook.SaveAs(fullPath);


                                        // Share the file
                                        await Share.RequestAsync(new ShareFileRequest
                                        {
                                            Title = "Export OOSList",
                                            File = new ShareFile(fullPath)
                                        });

                                        foreach (var record in data)
                                        {
                                            record.uploaded = 2;  // Set uploaded to 2 for exported data
                                            conn.Update(record);  // Update the record in the database
                                            OnAppearing();
                                        }

                                        //await DisplayAlert("Success", "Data  successfully saved to " + fullPath, "OK");
                                    }
                                }
                                else
                                {
                                    await DisplayAlert("Info", "Nothing to Export", "OK");
                                }


                            }
                            catch (Exception ex)
                            {
                                await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
                            }

                        }


                        // Your file saving logic here
                        // For example:
                        // File.WriteAllText(fullPath, "Your file content");

                        // Notify the system about the new file
                        //var mediaScanIntent = new Intent(Intent.ActionMediaScannerScanFile);
                        //mediaScanIntent.SetData(Android.Net.Uri.FromFile(new Java.IO.File(fullPath)));
                        //Android.App.Application.Context.SendBroadcast(mediaScanIntent);
                    }
                    catch (UnauthorizedAccessException ex)
                    {
                        // Handle permission denied error
                        Console.WriteLine($"Permission denied: {ex.Message}");
                        // You might want to prompt the user to grant permissions here
                    }
                    catch (Exception ex)
                    {
                        // Handle other exceptions
                        Console.WriteLine($"Error saving file: {ex.Message}");
                    }
                    //PerformActionEnd
                }
                else
                {
                    await DisplayAlert("Error", "Incorrect passcode, please contact Admin", "OK");
                }
            }

        }


        public async Task<bool> CheckAndRequestStoragePermission()
        {
            var status = await Permissions.CheckStatusAsync<Permissions.StorageWrite>();
            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.StorageWrite>();
            }
            return status == PermissionStatus.Granted;
        }


    }

    public class Result
    {
        public string ChildID { get; set; }
        public string operation { get; set; }
        public bool success { get; set; }
        public string error { get; set; }
    }

    public class ApiResponse
    {
        public string status { get; set; }
        public List<Result> results { get; set; }
    }





}



