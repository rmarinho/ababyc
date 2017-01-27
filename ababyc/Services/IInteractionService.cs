using System;
using ababyc.Models;
using Xamarin.Forms;

namespace ababyc.Services
{
	public interface IInteractionService
	{
		Rectangle InteractionSize { get; }

		event EventHandler<InteractionEventArgs> InteractionOccured;
		void HandleMouseDown();
		void HandleMouseUp();

		bool IsHardKeyboardPresent { get; }
	}

	public class InteractionEventArgs : EventArgs
	{
		public InteractionEventArgs(InteractionType interaction)
		{
			this.Interaction = interaction;
		}
		public InteractionType Interaction
		{
			get; set;
		}

		public string Key
		{
			get; set;
		}

		public Point Position
		{
			get; set;
		}

	}
}