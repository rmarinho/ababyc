using System;
using ababyc.Models;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;

namespace ababyc
{
	public class ShapeGlowCustomControl : GlowCustomControl
	{

		SKCanvasView _canvas;

		public ShapeGlowCustomControl()
		{
			Content = _canvas = new SKCanvasView();
			_canvas.PaintSurface += _view_PaintSurface;
		}

		void _view_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
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
