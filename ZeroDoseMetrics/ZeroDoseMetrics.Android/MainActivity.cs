using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;
using System.IO;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Essentials;
using AndroidX.Core.App;
using Android;

namespace ZeroDoseMetrics.Droid
{
    [Activity(Label = "IEVManagementApp", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public static readonly int REQUEST_EXTERNAL_STORAGE = 1;

        public static void VerifyStoragePermissions(Activity activity)
        {
            var permission = ActivityCompat.CheckSelfPermission(activity, Manifest.Permission.WriteExternalStorage);

            if (permission != Permission.Granted)
            {
                ActivityCompat.RequestPermissions(
                    activity,
                    new string[] { Manifest.Permission.ReadExternalStorage, Manifest.Permission.WriteExternalStorage },
                    REQUEST_EXTERNAL_STORAGE
                );
            }
        }


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            VerifyStoragePermissions(this);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            Xamarin.FormsMaps.Init(this, savedInstanceState);


            //Xamarin.Forms.DependencyService.Register<NetworkService>();

            string dbName = "OOSDB.sqlite";
            var documentPath = FileSystem.AppDataDirectory;

            var folderPath = Path.Combine(documentPath, "OOSDATA");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var fullPath = Path.Combine(folderPath, dbName);

            //string folderPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            //string folderPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
            //string fullPath = Path.Combine(folderPath, dbName);
            //string fullPath = Path.Combine("/storage/emulated/Download", dbName);

            //var fullPath = Path.Combine(Android.OS.Environment.ExternalStorageDirectory.AbsolutePath, Android.OS.Environment.DirectoryDocuments, "IEVDATA");
            // Ensure the directory exists
            //if (!Directory.Exists(fullPath))
            //{
            //    Directory.CreateDirectory(fullPath);
            //}

            // Specify the database file path
            //var dbPath = Path.Combine(fullPath, dbName);



            //Excel Export Path Logic
            string excelPath = Android.App.Application.Context.GetExternalFilesDir(Android.OS.Environment.DirectoryDownloads).AbsolutePath;

            var excelFolderPath = Path.Combine(excelPath, "EXCELOOS");

            //if (!Directory.Exists(excelFolderPath))
            //{
            //    Directory.CreateDirectory(excelFolderPath);
            //}

            //var excelFullPath = Path.Combine(excelFolderPath, dbName);


            LoadApplication(new App(fullPath, excelFolderPath));

            Xamarin.Forms.Application.Current.On<Xamarin.Forms.PlatformConfiguration.Android>().UseWindowSoftInputModeAdjust(WindowSoftInputModeAdjust.Resize); // added for screen size scrolling, etc
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}
