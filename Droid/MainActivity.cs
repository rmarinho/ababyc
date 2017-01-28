using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using ababyc.Services;
using Xamarin.Forms;

namespace ababyc.Droid
{
	[Activity(Label = "ababyc.Droid", Icon = "@drawable/icon", Theme = "@style/MyTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
	{
		protected override void OnCreate(Bundle bundle)
		{
			TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar;

			base.OnCreate(bundle);

			global::Xamarin.Forms.Forms.Init(this, bundle);
			DependencyService.Register<ISoundService, SoundService>();
			DependencyService.Register<ILanguageService, LanguageService>();
			DependencyService.Register<ISpeakService, SpeakService>();
			LoadApplication(new App());
		}
	}
}
