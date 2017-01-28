using System;
using System.Collections.Specialized;
using ababyc;
using ababyc.Droid;
using Android.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(ItemsControl), typeof(ItemsControlRenderer))]
namespace ababyc.Droid
{
	public class ItemsControlRenderer : ViewRenderer<ItemsControl, ViewGroup>
	{
		bool _disposed;

		protected override void Dispose(bool disposing)
		{
			if (disposing && !_disposed)
			{
				_disposed = true;
				if (Element != null)
				{
					var templatedItems = ((ITemplatedItemsView<Cell>)Element).TemplatedItems;
					templatedItems.CollectionChanged -= OnCollectionChanged;
				}
			}
			base.Dispose(disposing);
		}

		protected override void OnElementChanged(ElementChangedEventArgs<ItemsControl> e)
		{
			if (e.OldElement != null)
			{
				var templatedItems = ((ITemplatedItemsView<Cell>)e.OldElement).TemplatedItems;
				templatedItems.CollectionChanged -= OnCollectionChanged;
			}
			if (e.NewElement != null)
			{
				if (Control == null)
				{
					SetNativeControl(new Android.Widget.AbsoluteLayout(Context));
				}
				var templatedItems = ((ITemplatedItemsView<Cell>)e.NewElement).TemplatedItems;

				templatedItems.CollectionChanged += OnCollectionChanged;

			}
			base.OnElementChanged(e);
		}

		void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			switch (e.Action)
			{
				case NotifyCollectionChangedAction.Add:
					foreach (var item in e.NewItems)
					{
						AddFigure((item as ViewCell).View);
					}
					break;

				case NotifyCollectionChangedAction.Remove:
					foreach (var item in e.OldItems)
					{
						RemoveFigure((item as ViewCell).View);
					}
					break;
				case NotifyCollectionChangedAction.Move:
				case NotifyCollectionChangedAction.Replace:
				case NotifyCollectionChangedAction.Reset:
					return;
			}
		}

		void RemoveFigure(VisualElement view)
		{
			if (view == null)
				throw new ArgumentNullException(nameof(view));
			var renderer = Platform.GetRenderer(view);
			if (renderer == null)
				throw new ArgumentNullException(nameof(renderer));
			Control.RemoveView(renderer.ViewGroup);
			renderer.Dispose();
			Platform.SetRenderer(view, null);
		}

		void AddFigure(VisualElement view)
		{
			if (view == null)
				throw new ArgumentNullException(nameof(view));
			var renderer = Platform.GetRenderer(view);
			if (renderer == null)
				renderer = Platform.CreateRenderer(view);
			Platform.SetRenderer(view, renderer);

			var bounds = Xamarin.Forms.AbsoluteLayout.GetLayoutBounds(view);
			renderer.Element.Layout(bounds);
			var width = (int)Forms.Context.ToPixels(bounds.Width);
			var height = (int)Forms.Context.ToPixels(bounds.Height);
			var layoutParams = new Android.Widget.AbsoluteLayout.LayoutParams(width, height, (int)Forms.Context.ToPixels(bounds.Left), (int)Forms.Context.ToPixels(bounds.Top));
			Control.AddView(renderer.ViewGroup, layoutParams);
		}
	}
}
