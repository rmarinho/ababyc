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
			MobileCenter.Start("eb06135e-bd1e-4aa9-ac2c-d58f1a1e5037", typeof(Analytics), typeof(Crashes));
			base.OnCreate(bundle);

			Forms.Init(this, bundle);
			DependencyService.Register<ISoundService, SoundService>();
			DependencyService.Register<ILanguageService, LanguageService>();
			DependencyService.Register<ISpeakService, SpeakService>();
			LoadApplication(new App());
		}
	}
}
