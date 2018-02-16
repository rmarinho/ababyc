using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using ababyc;
using ababyc.iOS;
using ababyc.Models;
using CoreGraphics;
using OpenTK;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

//[assembly: ExportRenderer(typeof(ShapeGlowCustomControl), typeof(ShapeGlowCustomControlRenderer))]

namespace ababyc.iOS
{
	public class ShapeGlowCustomControlRenderer : ViewRenderer<ShapeGlowCustomControl, UIView>
	{
		public ShapeGlowCustomControlRenderer()
		{
		}

		public override void LayoutSubviews()
		{
			Control.Frame = Frame;
			base.LayoutSubviews();
		}
		protected override void OnElementChanged(ElementChangedEventArgs<ShapeGlowCustomControl> e)
		{
			if (e.NewElement != null)
			{
				if (Control == null)
				{
					var element = e.NewElement;
					SetNativeControl(new ShapeUIView(element.ShapeType) { FillColor = element.ShapeColor, StrokeColor = element.GlowColor });
				}
			}
			base.OnElementChanged(e);
		}

		class ShapeUIView : UIImageView
		{
			ShapeType _type;
			public ShapeUIView(ShapeType type)
			{
				_type = type;
			}

			public Color FillColor
			{
				get;
				set;
			}

			public Color StrokeColor
			{
				get;
				set;
			}

			public override void LayoutSubviews()
			{
				if (Image == null)
					Image = DrawShape(_type, Bounds.Size, UIColor.Red, UIColor.Purple);
				base.LayoutSubviews();
			}

			UIImage DrawShape(ShapeType shapeType, CGSize size, UIColor strokeColor, UIColor fillColor)
			{
				float width = (float)size.Width;
				float height = (float)size.Height;
				var stroke = 1;
				var scale = (width / 2) - (stroke * 2);
				var center = new Vector2(width / 2, height / 2);
				UIImage starImage;
				UIGraphics.BeginImageContext(new CGSize(width, height));
				using (CGContext g = UIGraphics.GetCurrentContext())
				{
					g.SetLineWidth(stroke);
					FillColor.ToUIColor().SetFill();
					StrokeColor.ToUIColor().SetStroke();
					var shapePoints = new Vector2[0];
					switch (shapeType)
					{
						case ShapeType.Oval:
							var radiusX = (width / 3) - stroke;
							var radiusY = (height / 2) - stroke;
							g.AddEllipseInRect(new CGRect(0, 0, radiusX * 2, radiusY * 2));
							break;
						case ShapeType.Circle:
							var radius = Math.Min(width, height) / 2 - stroke;
							g.AddEllipseInRect(new CGRect(0, 0, radius * 2, radius * 2));
							break;
						case ShapeType.Line:
							shapePoints = CreateLineGeometry(scale, center);
							break;
						case ShapeType.Hexagon:
							shapePoints = CreateHexagonGeometry(scale, center);
							break;
						case ShapeType.Rectangle:
							shapePoints = CreateRectangleGeometry(scale, center);
							break;
						case ShapeType.Square:
							shapePoints = CreateSquareGeometry(scale, center);
							break;
						case ShapeType.Trapezoid:
							shapePoints = CreateTrapezoidGeometry(scale, center);
							break;
						case ShapeType.Triangle:
							shapePoints = CreateTriangleGeometry(scale, center);
							break;
						case ShapeType.Star:
							shapePoints = CreateStarGeometry(scale, center);
							break;
						default:

							break;
					}
					if (shapePoints.Count() > 0)
					{
						var points = new List<CGPoint>();

						foreach (var item in shapePoints)
						{
							points.Add(new CGPoint(item.X, item.Y));
						}

						var path = new CGPath();

						path.AddLines(points.ToArray());

						path.CloseSubpath();

						g.AddPath(path);

					}
					g.DrawPath(CGPathDrawingMode.FillStroke);
					starImage = UIGraphics.GetImageFromCurrentImageContext();
				}

				return starImage;
			}

			static Vector2[] CreateLineGeometry(float scale, Vector2 center)
			{
				Vector2[] points = {
					new Vector2(-1,-1),
					new Vector2(1,1)
				};

				var transformedPoints = from point in points
										select point * scale + center;

				return transformedPoints.ToArray();
			}

			static Vector2[] CreateHexagonGeometry(float scale, Vector2 center)
			{
				Vector2[] points = {
					new Vector2(-1,0),
					new Vector2(-0.6f,-1),
					new Vector2(0.6f,-1),
					new Vector2(1,0),
					new Vector2(0.6f,1),
					new Vector2(-0.6f,1),
				};

				var transformedPoints = from point in points
										select point * scale + center;

				return transformedPoints.ToArray();
			}

			static Vector2[] CreateRectangleGeometry(float scale, Vector2 center)
			{
				Vector2[] points = {
					new Vector2(-1f, -0.5f),
					new Vector2(-1f, 0.5f),
					new Vector2(1f, 0.5f),
					new Vector2(1f, -0.5f)
				};

				var transformedPoints = from point in points
										select point * scale + center;

				return transformedPoints.ToArray();
			}

			static Vector2[] CreateSquareGeometry(float scale, Vector2 center)
			{
				Vector2[] points =
				{
					new Vector2(-1, -1),
					new Vector2(-1, 1),
					new Vector2(1, 1),
					new Vector2(1, -1)
				};

				var transformedPoints = from point in points
										select point * scale + center;

				return transformedPoints.ToArray();
			}

			static Vector2[] CreateStarGeometry(float scale, Vector2 center)
			{
				Vector2[] points =
				{
					new Vector2(-0.24f, -0.24f),
					new Vector2(0, -1),
					new Vector2(0.24f, -0.24f),
					new Vector2(1, -0.2f),
					new Vector2(0.4f, 0.2f),
					new Vector2(0.6f, 1),
					new Vector2(0, 0.56f),
					new Vector2(-0.6f, 1),
					new Vector2(-0.4f, 0.2f),
					new Vector2(-1, -0.2f),
				};

				var transformedPoints = from point in points
										select point * scale + center;

				return transformedPoints.ToArray();
			}

			static Vector2[] CreateTriangleGeometry(float scale, Vector2 center)
			{
				Vector2[] points =
				{
					new Vector2(-1,1),
					new Vector2(0,-1),
					new Vector2(1,1)
				};

				var transformedPoints = from point in points
										select point * scale + center;

				return transformedPoints.ToArray();
			}

			static Vector2[] CreateTrapezoidGeometry(float scale, Vector2 center)
			{
				Vector2[] points =
				{
					new Vector2(-1,0.7f),
					new Vector2(-0.6f,-0.7f),
					new Vector2(0.6f,-0.7f),
					new Vector2(1,0.7f)
				};

				var transformedPoints = from point in points
										select point * scale + center;

				return transformedPoints.ToArray();
			}
		}
	}
}
