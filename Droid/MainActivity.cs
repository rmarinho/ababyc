using System;
using Android.App;
using Android.Content.PM;
using Android.OS;
using ababyc.Services;
using Xamarin.Forms;
using Microsoft.Azure.Mobile;
using Microsoft.Azure.Mobile.Analytics;
using Microsoft.Azure.Mobile.Crashes;

namespace ababyc.Droid
{
	[Activity(Label = "ababyc.Droid", Icon = "@drawable/icon", Theme = "@style/MyTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
	{
		protected override void OnCreate(Bundle bundle)
		{
			TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar;
			MobileCenter.Start("48223df6-db72-4346-a7c2-5ceef3dcf544", typeof(Analytics), typeof(Crashes));
			base.OnCreate(bundle);

			Forms.Init(this, bundle);
			DependencyService.Register<ISoundService, SoundService>();
			DependencyService.Register<ILanguageService, LanguageService>();
			DependencyService.Register<ISpeakService, SpeakService>();
			LoadApplication(new App());
		}
	}
}
