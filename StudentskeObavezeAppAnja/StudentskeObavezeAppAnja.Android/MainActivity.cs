﻿using Plugin.LocalNotification;
using System;
using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;


namespace StudentskeObavezeAppAnja.Droid
{
    [Activity(Label = "StudentskeObavezeAppAnja", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize )]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            NotificationCenter.CreateNotificationChannel();

            base.OnCreate(savedInstanceState);

            Xamarin.Forms.Forms.SetFlags("Shell_UWP_Experimental"); 

            Window.SetStatusBarColor(Android.Graphics.Color.Gray);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            LoadApplication(new App());
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}