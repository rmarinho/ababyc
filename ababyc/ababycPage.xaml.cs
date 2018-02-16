using System;
using ababyc.Models;
using ababyc.Services;
using ababyc.ViewModels;
using Xamarin.Forms;

namespace ababyc
{
	public partial class ababycPage : ContentPage
	{

		public MainViewModel ViewModel
		{
			get
			{
				return this.BindingContext as MainViewModel;
			}
		}
		public ababycPage()
		{
			InitializeComponent();
			BindingContext = new MainViewModel();
			ViewModel.HookInterationService(new InteractionService(interactionBox));
		}
	}

	class FigureTemplateSelector : DataTemplateSelector
	{
		public FigureTemplateSelector()
		{

		}

		protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
		{
			if (item is LetterFigure || item is NumberFigure)
				return TextTemplate;
			if (item is ShapeFigure)
				return ShapeTemplate;

			return TextTemplate;
		}

		public DataTemplate TextTemplate { get; set; }
		public DataTemplate ShapeTemplate { get; set; }
	}

	public class InteractionService : IInteractionService
	{
		View _view;
		public InteractionService(View view)
		{
			_view = view;
			TapGestureRecognizer tapGesture = new TapGestureRecognizer();
			view.GestureRecognizers.Add(tapGesture);
			tapGesture.Tapped += (sender, e) => { InteractionOccured?.Invoke(view, new InteractionEventArgs(Models.InteractionType.MouseClick)); };
		}

		public Rectangle InteractionSize => _view.Bounds;
		public bool IsHardKeyboardPresent
		{
			get
			{
				return false;
			}
		}

		public event EventHandler<InteractionEventArgs> InteractionOccured;

		public void HandleMouseDown()
		{
			throw new NotImplementedException();
		}

		public void HandleMouseUp()
		{
			throw new NotImplementedException();
		}
	}
}
