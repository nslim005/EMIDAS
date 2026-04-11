using System;
using System.Collections.Generic;

using Xamarin.Forms;
using ZeroDoseMetrics.Model;
using Xamarin.Essentials;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using SQLite;
using System.Globalization;
using System.Net.Http;

namespace ZeroDoseMetrics.OOSZamfara
{
    public partial class VaccineUtilization : ContentPage
    {
        public Login helper { get; set; }

        public OOSList newChild { get; set; }
        public VaccinesUtilization vaccUtil { get; set; }
        public string TeamCode { get; set; }
        public string Settlement { get; set; }
        public string HealthFacility { get; set; }
        public string InterviewerName { get; set; }
        public string PhoneNumber { get; set; }


        public VaccineUtilization(Login helper)
        {
            InitializeComponent();
            this.helper = helper;
            newChild = new OOSList();
            this.TeamCode = helper.TeamCode;
            this.Settlement = helper.Settlement;
            this.HealthFacility = helper.HealthFacility;
            this.PhoneNumber = helper.PhoneNo;
            this.InterviewerName = helper.InterviewerName;
            var lga = helper.LGA.ToUpper();
            var retlga = lga.Substring(0, 3);
            var ward = helper.Ward.ToUpper();
            var retward = ward.Substring(0, 3);
            vaccUtil = new VaccinesUtilization();

            string time = DateTime.Now.ToString("hh:mm tt");
            string date = DateTime.Now.ToString("dddd, dd MMMM yyyy");
            string unique = DateTime.Now.ToString("MMddHHmmss");

            EnumeratorNameEntry.Text = InterviewerName;
            phoneNoEntry.Text = PhoneNumber;
            dateEnt.Text = date;
            timeEnt.Text = time;
            teamCodeEntry.Text = TeamCode;
            lgaEntry.Text = lga;
            wardEntry.Text = ward;
            LocationLabel.Text = LocationLabel.Text;
            settlementEntry.Text = Settlement;
            hfEntry.Text = HealthFacility;
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
            public async void Button_Clicked(System.Object sender, System.EventArgs e)
            {

            try
            {
                //string coordi = LocationLabel.Text;
                string BCGDosesRec = BCGDosesRecv.Text;
                string BCGDosesOpn = BCGDosesOpnd.Text;

                string HepBDosesRec = HepBDosesRecv.Text;
                string HepBDosesOpn = HepBDosesOpnd.Text;

                string OPVRec = OPVRecv.Text;
                string OPVOpen = OPVOpend.Text;

                string PentaRec = PentaRecv.Text;
                string PentaOpen = PentaOpend.Text;

                string PCVRec = PCVRecv.Text;
                string PCVOpen = PCVOpend.Text;

                string RotaRec = RotaRecv.Text;
                string RotaOpen = RotaOpend.Text;

                string IPVRec = IPVRecv.Text;
                string IPVOpen = IPVOpend.Text;

                string YFRec = YFRecv.Text;
                string YFOpen = YFOpend.Text;

                string MenARec = MenARecv.Text;
                string MenAOpen = MenAOpend.Text;

                string MeaslesRec = MeaslesRecv.Text;
                string MeaslesOpn = MeaslesOpnd.Text;

                string HPVRec = HPVRecv.Text;
                string HPVOpen = HPVOpend.Text;

                string TDRec = TDRecv.Text;
                string TDOpen = TDOpend.Text;

                string PCMRec = RecPCM.Text;
                string PCMUsed = usedPCM.Text;

                string SoapRec = receivedSoap.Text;
                string SoapOpen = usedSoap.Text;

                string AFPCasesRecorde = AFPCasesRecorded.Text;
                string measlesCaseRecorde = measlesCaseRecorded.Text;
                string TFCasesRecorde = TFCasesRecorded.Text;
                string choleraCasesRecorde = choleraCasesRecorded.Text;
                string diptheriaCasesRecorde = diptheriaCasesRecorded.Text;
                string meningitisCasesRecorde = meningitisCasesRecorded.Text;
                string neoTetanisCasesRecorde = neoTetanisCasesRecorded.Text;
                string other = others.Text;

                string time = DateTime.Now.ToString("hh:mm tt");
                string date = DateTime.Now.ToString("dddd, dd MMMM yyyy");

                //validation
                if (string.IsNullOrEmpty(LocationLabel.Text))
                {
                    await DisplayAlert("ERROR", "Please take Geocordinates", "OK");
                }
                else if (LocationLabel.Text == "Fetch location...")
                {
                    await DisplayAlert("ERROR", "Please take Geocordinates", "OK");
                }
                //new pattern from here
                else if (!int.TryParse(BCGDosesOpn, out int bcgOpn) || !int.TryParse(BCGDosesRec, out int bcgRec))
                {
                    await DisplayAlert("ERROR", "Invalid number for BCG doses", "OK");
                }
                else if (bcgOpn > bcgRec)
                {
                    await DisplayAlert("ERROR", "BCG Doses Opened Cannot be greater than BCG Doses Received", "OK");
                }
                else if (!int.TryParse(HepBDosesOpn, out int hepOpn) || !int.TryParse(HepBDosesRec, out int hepRec))
                {
                    await DisplayAlert("ERROR", "Invalid number for HepB doses", "OK");
                }
                else if (hepOpn > hepRec)
                {
                    await DisplayAlert("ERROR", "HepB Doses Opened Cannot be greater than HepB Doses Received", "OK");
                }
                // Validate and compare numeric fields
                if (!int.TryParse(OPVRec, out int opvRec) || !int.TryParse(OPVOpen, out int opvOpen))
                {
                    await DisplayAlert("ERROR", "Invalid number for OPV doses", "OK");
                    return;
                }
                else if (opvOpen > opvRec)
                {
                    await DisplayAlert("ERROR", "OPV Doses Opened Cannot be greater than OPV Doses Received", "OK");
                    return;
                }

                if (!int.TryParse(PentaRec, out int pentaRec) || !int.TryParse(PentaOpen, out int pentaOpen))
                {
                    await DisplayAlert("ERROR", "Invalid number for Penta doses", "OK");
                    return;
                }
                else if (pentaOpen > pentaRec)
                {
                    await DisplayAlert("ERROR", "Penta Doses Opened Cannot be greater than Penta Doses Received", "OK");
                    return;
                }

                if (!int.TryParse(PCVRec, out int pcvRec) || !int.TryParse(PCVOpen, out int pcvOpen))
                {
                    await DisplayAlert("ERROR", "Invalid number for PCV doses", "OK");
                    return;
                }
                else if (pcvOpen > pcvRec)
                {
                    await DisplayAlert("ERROR", "PCV Doses Opened Cannot be greater than PCV Doses Received", "OK");
                    return;
                }

                if (!int.TryParse(RotaRec, out int rotaRec) || !int.TryParse(RotaOpen, out int rotaOpen))
                {
                    await DisplayAlert("ERROR", "Invalid number for Rota doses", "OK");
                    return;
                }
                else if (rotaOpen > rotaRec)
                {
                    await DisplayAlert("ERROR", "Rota Doses Opened Cannot be greater than Rota Doses Received", "OK");
                    return;
                }

                if (!int.TryParse(IPVRec, out int ipvRec) || !int.TryParse(IPVOpen, out int ipvOpen))
                {
                    await DisplayAlert("ERROR", "Invalid number for IPV doses", "OK");
                    return;
                }
                else if (ipvOpen > ipvRec)
                {
                    await DisplayAlert("ERROR", "IPV Doses Opened Cannot be greater than IPV Doses Received", "OK");
                    return;
                }

                if (!int.TryParse(YFRec, out int yfRec) || !int.TryParse(YFOpen, out int yfOpen))
                {
                    await DisplayAlert("ERROR", "Invalid number for Yellow Fever doses", "OK");
                    return;
                }
                else if (yfOpen > yfRec)
                {
                    await DisplayAlert("ERROR", "Yellow Fever Doses Opened Cannot be greater than Yellow Fever Doses Received", "OK");
                    return;
                }

                if (!int.TryParse(MenARec, out int menARec) || !int.TryParse(MenAOpen, out int menAOpen))
                {
                    await DisplayAlert("ERROR", "Invalid number for MenA doses", "OK");
                    return;
                }
                else if (menAOpen > menARec)
                {
                    await DisplayAlert("ERROR", "MenA Doses Opened Cannot be greater than MenA Doses Received", "OK");
                    return;
                }

                if (!int.TryParse(MeaslesRec, out int measlesRec) || !int.TryParse(MeaslesOpn, out int measlesOpen))
                {
                    await DisplayAlert("ERROR", "Invalid number for Measles doses", "OK");
                    return;
                }
                else if (measlesOpen > measlesRec)
                {
                    await DisplayAlert("ERROR", "Measles Doses Opened Cannot be greater than Measles Doses Received", "OK");
                    return;
                }

                if (!int.TryParse(HPVRec, out int hpvRec) || !int.TryParse(HPVOpen, out int hpvOpen))
                {
                    await DisplayAlert("ERROR", "Invalid number for HPV doses", "OK");
                    return;
                }
                else if (hpvOpen > hpvRec)
                {
                    await DisplayAlert("ERROR", "HPV Doses Opened Cannot be greater than HPV Doses Received", "OK");
                    return;
                }

                if (!int.TryParse(TDRec, out int tdRec) || !int.TryParse(TDOpen, out int tdOpen))
                {
                    await DisplayAlert("ERROR", "Invalid number for TD doses", "OK");
                    return;
                }
                else if (tdOpen > tdRec)
                {
                    await DisplayAlert("ERROR", "TD Doses Opened Cannot be greater than TD Doses Received", "OK");
                    return;
                }

                if (!int.TryParse(PCMRec, out int pcmRec) || !int.TryParse(PCMUsed, out int pcmUsed))
                {
                    await DisplayAlert("ERROR", "Invalid number for Dispersible PCM", "OK");
                    return;
                }
                else if (pcmUsed > pcmRec)
                {
                    await DisplayAlert("ERROR", "Dispersible PCM Used Cannot be greater than Dispersible PCM Received", "OK");
                    return;
                }

                if (!int.TryParse(SoapRec, out int soapRec) || !int.TryParse(SoapOpen, out int soapUsed))
                {
                    await DisplayAlert("ERROR", "Invalid number for Bar Soap", "OK");
                    return;
                }
                else if (soapUsed > soapRec)
                {
                    await DisplayAlert("ERROR", "Pieces of Bar Soap Utilized Cannot be greater than pieces of Bar Soap Received", "OK");
                    return;
                }

                //else if (Convert.ToInt32(BCGDosesOpn) > Convert.ToInt32(BCGDosesRec))
                //{
                //    await DisplayAlert("ERROR", "BCG Doses Opened Cannot be greater than BCG Doses Received", "OK");
                //}
                //else if (Convert.ToInt32(HepBDosesOpn) > Convert.ToInt32(HepBDosesRec))
                //{
                //    await DisplayAlert("ERROR", "HepB Doses Opened Cannot be greater than HepB Doses Received", "OK");
                //}
                //else if (Convert.ToInt32(OPVOpen) > Convert.ToInt32(OPVRec))
                //{
                //    await DisplayAlert("ERROR", "OPV Doses Opened Cannot be greater than OPV Doses Received", "OK");
                //}
                //else if (Convert.ToInt32(PentaOpen) > Convert.ToInt32(PentaRec))
                //{
                //    await DisplayAlert("ERROR", "Penta Doses Opened Cannot be greater than Penta Doses Received", "OK");
                //}
                //else if (Convert.ToInt32(PCVOpen) > Convert.ToInt32(PCVRec))
                //{
                //    await DisplayAlert("ERROR", "PCVOpen Doses Opened Cannot be greater than PCVRec Doses Received", "OK");
                //}
                //else if (Convert.ToInt32(RotaOpen) > Convert.ToInt32(RotaRec))
                //{
                //    await DisplayAlert("ERROR", "Rota Doses Opened Cannot be greater than Rota Doses Received", "OK");
                //}
                //else if (Convert.ToInt32(IPVOpen) > Convert.ToInt32(IPVRec))
                //{
                //    await DisplayAlert("ERROR", "IPV Doses Opened Cannot be greater than IPV Doses Received", "OK");
                //}
                //else if (Convert.ToInt32(YFOpen) > Convert.ToInt32(YFRec))
                //{
                //    await DisplayAlert("ERROR", "Yellow Fever Doses Opened Cannot be greater than Yellow Fever Doses Received", "OK");
                //}
                //else if (Convert.ToInt32(MenAOpen) > Convert.ToInt32(MenARec))
                //{
                //    await DisplayAlert("ERROR", "MenA Doses Opened Cannot be greater than MenA Doses Received", "OK");
                //}
                //else if (Convert.ToInt32(MeaslesOpn) > Convert.ToInt32(MeaslesRec))
                //{
                //    await DisplayAlert("ERROR", "Measles Doses Opened Cannot be greater than Measles Doses Received", "OK");
                //}
                //else if (Convert.ToInt32(HPVOpen) > Convert.ToInt32(HPVRec))
                //{
                //    await DisplayAlert("ERROR", "HPV Doses Opened Cannot be greater than HPV Doses Received", "OK");
                //}
                //else if (Convert.ToInt32(PCMUsed) > Convert.ToInt32(PCMRec))
                //{
                //    await DisplayAlert("ERROR", "Dispersible PCM Used Cannot be greater than Dispersible PCM Received", "OK");
                //}
                //else if (Convert.ToInt32(SoapOpen) > Convert.ToInt32(SoapRec))
                //{
                //    await DisplayAlert("ERROR", "Pieces of Bar Soap Utilized Cannot be greater than pieces of Bar Soap Received", "OK");
                //}
                //else if (Convert.ToInt32(TDOpen) > Convert.ToInt32(TDRec))
                //{
                //    await DisplayAlert("ERROR", "TD Doses Opened Cannot be greater than TD Doses Received", "OK");
                //}
                else if (string.IsNullOrEmpty(BCGDosesRec) || string.IsNullOrEmpty(BCGDosesOpn) || string.IsNullOrEmpty(HepBDosesRec) ||
                    string.IsNullOrEmpty(HepBDosesOpn) || string.IsNullOrEmpty(OPVRec) || string.IsNullOrEmpty(OPVOpen) || string.IsNullOrEmpty(PentaRec) ||
                    string.IsNullOrEmpty(PentaOpen) || string.IsNullOrEmpty(PCVRec) || string.IsNullOrEmpty(PCVOpen) || string.IsNullOrEmpty(RotaRec) ||
                    string.IsNullOrEmpty(RotaOpen) || string.IsNullOrEmpty(IPVRec) || string.IsNullOrEmpty(IPVOpen) || string.IsNullOrEmpty(YFRec) ||
                    string.IsNullOrEmpty(YFOpen) || string.IsNullOrEmpty(MenARec) || string.IsNullOrEmpty(MenAOpen) || string.IsNullOrEmpty(MeaslesRec) ||
                    string.IsNullOrEmpty(MeaslesOpn) || string.IsNullOrEmpty(HPVRec) || string.IsNullOrEmpty(HPVOpen) || string.IsNullOrEmpty(TDRec) ||
                    string.IsNullOrEmpty(TDOpen) || string.IsNullOrEmpty(AFPCasesRecorde) || string.IsNullOrEmpty(measlesCaseRecorde) || string.IsNullOrEmpty(TFCasesRecorde) ||
                    string.IsNullOrEmpty(choleraCasesRecorde) || string.IsNullOrEmpty(diptheriaCasesRecorde) || string.IsNullOrEmpty(meningitisCasesRecorde) ||
                    string.IsNullOrEmpty(neoTetanisCasesRecorde) || string.IsNullOrEmpty(other) || string.IsNullOrEmpty(PCMRec) || string.IsNullOrEmpty(PCMUsed) ||
                    string.IsNullOrEmpty(SoapRec) || string.IsNullOrEmpty(SoapOpen))
                {
                    await DisplayAlert("ERROR", "ALL FIELDS ARE COMPULSORY INCLUDING", "OK");
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

                    vaccUtil.VaccinatorName = InterviewerName.Trim();
                    vaccUtil.VaccinatorNumber = PhoneNumber.Trim();
                    vaccUtil.TeamCode = TeamCode;
                    vaccUtil.LGA = lgaEntry.Text.Trim();
                    vaccUtil.Ward = wardEntry.Text.Trim();
                    vaccUtil.CAHF = HealthFacility.Trim();
                    vaccUtil.Latitude = xx;
                    vaccUtil.Longitude = yy;
                    vaccUtil.Time = timeEnt.Text.Trim();
                    vaccUtil.Date = dateEnt.Text.Trim();
                    vaccUtil.Settlement = settlementEntry.Text.Trim();
                    vaccUtil.BCGDosesRecv = BCGDosesRec.Trim();
                    vaccUtil.BCGDosesOpnd = BCGDosesOpn.Trim();
                    vaccUtil.HepBDosesRecv = HepBDosesRec.Trim();
                    vaccUtil.HepBDosesOpnd = HepBDosesOpn.Trim();
                    vaccUtil.OPVRecv = OPVRec.Trim();
                    vaccUtil.OPVOpend = OPVOpen.Trim();
                    vaccUtil.PentaRecv = PentaRec.Trim();
                    vaccUtil.PentaOpend = PentaOpen.Trim();
                    vaccUtil.PCVRecv = PCVRec.Trim();
                    vaccUtil.PCVOpend = PCVOpen.Trim();
                    vaccUtil.RotaRecv = RotaRec.Trim();
                    vaccUtil.RotaOpend = RotaOpen.Trim();
                    vaccUtil.IPVRecv = IPVRec.Trim();
                    vaccUtil.IPVOpend = IPVOpen.Trim();
                    vaccUtil.YFRecv = YFRec.Trim();
                    vaccUtil.YFOpend = YFOpen.Trim();
                    vaccUtil.MenARecv = MenARec.Trim();
                    vaccUtil.MenAOpend = MenAOpen.Trim();
                    vaccUtil.MeaslesRecv = MeaslesRec.Trim();
                    vaccUtil.MeaslesOpnd = MeaslesOpn.Trim();
                    vaccUtil.HPVRecv = HPVRec.Trim();
                    vaccUtil.HPVOpend = HPVOpen.Trim();
                    vaccUtil.TDRecv = TDRec.Trim();
                    vaccUtil.TDOpend = TDOpen.Trim();
                    vaccUtil.AFPCasesRecorded = AFPCasesRecorde.Trim();
                    vaccUtil.MeaslesCaseRecorded = measlesCaseRecorde.Trim();
                    vaccUtil.TFCasesRecorded = TFCasesRecorde.Trim();
                    vaccUtil.choleraCasesRecorded = choleraCasesRecorde.Trim();
                    vaccUtil.diptheriaCasesRecorded = diptheriaCasesRecorde.Trim();
                    vaccUtil.meningitisCasesRecorded = meningitisCasesRecorde.Trim();
                    vaccUtil.neoTetanisCasesRecorded = neoTetanisCasesRecorde.Trim();
                    vaccUtil.others = other.Trim();
                    vaccUtil.pcmReceived = PCMRec.Trim();
                    vaccUtil.pcmOpened = PCMUsed.Trim();
                    vaccUtil.soapReceived = SoapRec.Trim();
                    vaccUtil.soapOpened = SoapOpen.Trim();

                    using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                    {
                        //int rows = 0;

                        //conn.CreateTable<VaccinesUtilization>();
                        //int rows = conn.Insert(vaccUtil);
                        string jsonString = System.Text.Json.JsonSerializer.Serialize(vaccUtil);
                        var client = new HttpClient();
                        var request = new HttpRequestMessage(HttpMethod.Post, "http://azmda.com.ng/KTOOS/createvaccineutilization.php");
                        var content = new StringContent(jsonString, null, "application/json");
                        request.Content = content;

                        // Send the request
                        var response = await client.SendAsync(request);

                        if (response.IsSuccessStatusCode)
                        {
                            var responseCode = await response.Content.ReadAsStringAsync();

                            if (responseCode == "{\"status\":\"success\",\"code\":200}\n\n\n")
                            {
                                await DisplayAlert("Success", "VPDs, Vaccine & Commodities Utilization Record saved Successfully", "OK");
                                await Navigation.PushAsync(new ChildrenLineListPage(helper));
                            }
                            else
                            {
                                await DisplayAlert("Failure", "Error saving Vaccine Utilization and VPDs record", "OK");
                            }

                        }
                        else
                        {
                            await DisplayAlert("Failure", "Network Error saving Vaccine Utilization and VPDs record. Check Internet", "OK");
                            await Navigation.PushAsync(new ChildrenLineListPage(helper));
                        }

                    }

                }
            }
            catch (Exception ex)
            {

            }

                
            }

        }
    }
