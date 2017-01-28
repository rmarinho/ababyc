using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using ababyc;
using ababyc.iOS;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(ItemsControl), typeof(ItemsControlRenderer))]
namespace ababyc.iOS
{
	public class ItemsControlRenderer : ViewRenderer<ItemsControl, UIView>
	{
		UIView holderView;
		bool _disposed;
		public ItemsControlRenderer()
		{
		}

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
					holderView = new UIView(Bounds);
					SetNativeControl(holderView);
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
						var view = (item as ViewCell).View as VisualElement;
						var renderer = Platform.GetRenderer(view);
						if (renderer == null)
							renderer = Platform.CreateRenderer(view);
						Platform.SetRenderer(view, renderer);

						var bounds = AbsoluteLayout.GetLayoutBounds(view);
						renderer.SetElementSize(bounds.Size);
						renderer.NativeView.Frame = bounds.ToRectangleF();
						holderView.AddSubview(renderer.NativeView);
					}

					break;

				case NotifyCollectionChangedAction.Remove:
					foreach (var item in e.OldItems)
					{
						var view = (item as ViewCell).View as VisualElement;
						var renderer = Platform.GetRenderer(view);
						if (renderer == null)
							throw new ArgumentNullException(nameof(renderer));
						renderer.NativeView?.RemoveFromSuperview();
						renderer.Dispose();
						Platform.SetRenderer(view, null);
					}

					break;

				case NotifyCollectionChangedAction.Move:
				case NotifyCollectionChangedAction.Replace:
				case NotifyCollectionChangedAction.Reset:

					return;
			}
		}
	}
}