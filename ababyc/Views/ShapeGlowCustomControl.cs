using System;
using ababyc.Models;
using Xamarin.Forms;

namespace ababyc
{
	public class ShapeGlowCustomControl : GlowCustomControl
	{
		public ShapeGlowCustomControl()
		{
		}
		public static readonly BindableProperty ShapeTypeProperty = BindableProperty.Create(nameof(ShapeType), typeof(ShapeType), typeof(ShapeGlowCustomControl), default(ShapeType));

		public static readonly BindableProperty ShapeColorProperty = BindableProperty.Create(nameof(ShapeColor), typeof(Color), typeof(ShapeGlowCustomControl), Color.Blue);


		public ShapeType ShapeType
		{
			get
			{
				return (ShapeType)GetValue(ShapeTypeProperty);
			}
			set
			{
				SetValue(ShapeTypeProperty, value);
			}
		}

		public Color ShapeColor
		{
			get
			{
				return (Color)GetValue(ShapeColorProperty);
			}
			set
			{
				SetValue(ShapeColorProperty, value);
			}
		}
	}
}
