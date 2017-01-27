using System;
using System.Threading.Tasks;
using ababyc.Models;
using Xamarin.Forms;

namespace ababyc
{
	public class BaseCustomControl : ContentView
	{
		public BaseCustomControl()
		{
			if (Settings.Default.UseAnimations)
			{
				AddInitialAnimation(this);
				if (Settings.Default.FadeAway)
					AddFinalAnimation(this);
			}
		}

		static async Task AddFinalAnimation(View view)
		{
			await Task.Delay(Settings.Default.FadeAfter * 1000 / 2);
			await view.FadeTo(0, (uint)(Settings.Default.FadeAfter * 1000 / 2));
			Device.BeginInvokeOnMainThread(() => view.IsEnabled = false);
		}

		static async Task AddInitialAnimation(View view)
		{
			uint duration = 500;
			view.Opacity = 0;
			view.Scale = 0;
			var randomAnimation = ApplyRandomAnimationEffect(view, duration);
			await Task.WhenAll(view.FadeTo(1, duration), view.ScaleTo(1, duration), randomAnimation);
		}

		static Task ApplyJiggle(View view, uint duration)
		{
			view.Rotation = 0;
			return view.RotateTo(20, duration, Easing.SpringIn);
		}

		static Task ApplySnap(View view, uint duration)
		{
			view.Scale = 0;
			return view.ScaleTo(1, duration, Easing.SinInOut);
		}

		static Task ApplyThrob(View view, uint duration)
		{
			view.Scale = 0.95;
			return view.ScaleTo(1, duration, Easing.SpringIn);
		}

		static Task ApplyRotate(View view, uint duration)
		{
			view.Rotation = 0;
			return view.RotateTo(360, duration, Easing.BounceIn);
		}

		static Task ApplyRandomAnimationEffect(View view, uint duration)
		{
			int e = Utils.RandomBetweenTwoNumbers(0, 3);
			switch (Utils.RandomBetweenTwoNumbers(0, 3))
			{
				case 0:
					return ApplyJiggle(view, duration);
				case 1:
					return ApplySnap(view, duration);
				case 2:
					return ApplyThrob(view, duration);
				case 3:
					return ApplyRotate(view, duration);

			}
			return Task.FromResult(false);
		}

	}
}
