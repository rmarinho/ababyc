using System;
using System.Collections.Generic;
using System.Linq;
using ababyc.Services;
using Foundation;
using Microsoft.Azure.Mobile;
using UIKit;
using Xamarin.Forms;

namespace ababyc.iOS
{
	[Register("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
			MobileCenter.Configure("41184976-5a88-45b7-a80f-c305fb873d70");
			global::Xamarin.Forms.Forms.Init();
			DependencyService.Register<ISoundService, SoundService>();
			DependencyService.Register<ILanguageService, LanguageService>();
			DependencyService.Register<ISpeakService, SpeakService>();
			var application = new App();
			LoadApplication(application);

			return base.FinishedLaunching(app, options);
		}
	}
}
