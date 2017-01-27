using System;
using System.Threading.Tasks;
using ababyc.Models;
using Xamarin.Forms;

namespace ababyc
{
	public class GlowCustomControl : BaseCustomControl
	{
		public GlowCustomControl()
		{

		}

		protected override void OnPropertyChanged(string propertyName = null)
		{
			if (propertyName == nameof(View.Width))
			{
			}
			base.OnPropertyChanged(propertyName);
		}

		public static readonly BindableProperty AnimateGlowProperty = BindableProperty.Create(nameof(AnimateGlow), typeof(bool), typeof(GlowCustomControl), false, propertyChanged: OnAnimateGlowChanged);

		public static readonly BindableProperty GlowColorProperty = BindableProperty.Create(nameof(GlowColor), typeof(Color), typeof(GlowCustomControl), Color.Green, propertyChanged: OnAnimateGlowChanged);

		public static readonly BindableProperty GlowAmountProperty = BindableProperty.Create(nameof(GlowAmount), typeof(double), typeof(GlowCustomControl), 5.0, propertyChanged: OnAnimateGlowChanged);

		public static readonly BindableProperty MaxGlowAmountProperty = BindableProperty.Create(nameof(MaxGlowAmount), typeof(double), typeof(GlowCustomControl), 5.0, propertyChanged: OnAnimateGlowChanged);

		public bool AnimateGlow
		{
			get
			{
				return (bool)GetValue(AnimateGlowProperty);
			}
			set
			{
				SetValue(AnimateGlowProperty, value);
			}
		}

		public double GlowAmount
		{
			get
			{
				return (double)GetValue(GlowAmountProperty);
			}
			set
			{
				SetValue(GlowAmountProperty, value);
			}
		}

		public double MaxGlowAmount
		{
			get
			{
				return (double)GetValue(MaxGlowAmountProperty);
			}
			set
			{
				SetValue(MaxGlowAmountProperty, value);
			}
		}

		public Color GlowColor
		{
			get
			{
				return (Color)GetValue(GlowColorProperty);
			}
			set
			{
				SetValue(GlowColorProperty, value);
			}
		}

		static void OnAnimateGlowChanged(BindableObject bindable, object oldValue, object newValue)
		{
			var instance = bindable as GlowCustomControl;
			if (instance.AnimateGlow && Settings.Default.UseEffects && Settings.Default.UseAnimations)
			{
				instance.AddAnimateGlow(instance.GlowAmount, instance.MaxGlowAmount);
			}
		}

		void AddAnimateGlow(double glowAmount, double maxGlowAmount)
		{

		}
	}
}
