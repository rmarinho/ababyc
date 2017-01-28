using System;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ababyc.Services;
using static ababyc.Utils;
using Xamarin.Forms;
using ababyc.Models;

namespace ababyc.ViewModels
{
	public class MainViewModel : BaseViewModel, IDisposable
	{
		const string helloMessage = "helloMessage";
		const string introSound = "ababyc.Data.Sounds.EditedJackPlaysBabySmash.wav";
		const int timerDelay = 30 * 1000;
		internal IInteractionService _interactionService;
		ISpeakService _speakService;
		ISoundService _soundService;
		ILanguageService _languageService;
		ResourcesService _resourceService;
		Timer timer;
		bool disposed;
		public MainViewModel()
		{
			_resourceService = DependencyService.Get<ResourcesService>();
			_speakService = DependencyService.Get<ISpeakService>();
			_soundService = DependencyService.Get<ISoundService>();
			_languageService = DependencyService.Get<ILanguageService>();

			if (_speakService == null)
				throw new ArgumentNullException(nameof(_speakService));

			if (_languageService == null)
				throw new ArgumentNullException(nameof(_languageService));

			if (_soundService == null)
				throw new ArgumentNullException(nameof(_soundService));

			//timer to check disable figures and remove them from the collection
			timer = new Timer((obj) => CheckFiguresToRemove(), null, timerDelay, timerDelay);

			//check native interactionService
			var interactionService = DependencyService.Get<IInteractionService>();
			if (interactionService != null)
				HookInterationService(interactionService);

			//play the intro sound
			_soundService.PlayEmbebedResourceAsync(introSound);
		}

		public void HookInterationService(IInteractionService interactionservice)
		{
			if (interactionservice == null)
				throw new ArgumentNullException(nameof(interactionservice));
			_interactionService = interactionservice;
			_interactionService.InteractionOccured += InteractionService_InteractionOccured;
		}

		public void Dispose()
		{
			if (disposed)
				return;
			disposed = true;

			timer?.Dispose();
			timer = null;

			if (_interactionService != null)
			{
				_interactionService.InteractionOccured -= InteractionService_InteractionOccured;
				_interactionService = null;
			}

			_speakService = null;

		}

		public string HelloMessage
		{
			get
			{
				return _resourceService.GetResourceText(helloMessage, _languageService.DefaultLanguage.Locale);
			}
		}

		ObservableCollection<Figure> _figures = new ObservableCollection<Figure>();
		public ObservableCollection<Figure> Figures
		{
			get
			{
				return _figures;
			}
			set
			{
				SetField(ref _figures, value);
			}
		}

		async void InteractionService_InteractionOccured(object sender, InteractionEventArgs e)
		{
			switch (e.Interaction)
			{
				case InteractionType.MouseClick:
					await ProcessKey("55");
					break;
				case InteractionType.MouseMove:
					break;
				case InteractionType.KeyPress:
					await ProcessKey(e.Key);
					break;
				case InteractionType.Exit:
					Clear();
					break;
				default:
					break;
			}
		}

		void Clear()
		{
			Figures.Clear();
		}

		async Task ProcessKey(string key)
		{
			if (string.IsNullOrEmpty(key))
				return;

			//could be a letter or number
			if (key.Length == 1)
			{
				// If a letter was pressed, display the letter.
				if (Regex.IsMatch(key, @"^[a-zA-Z]+$"))
					await AddLetter(key[0]);

				// If a number is pressed, display the number.
				if (Regex.IsMatch(key, @"^[0-9]+$"))
				{
					int number;
					if (int.TryParse(key, out number))
						await AddNumber(int.Parse(key));
				}
			}
			else {
				// Otherwise, display a random shape.
				await AddFigureAsync(new ShapeFigure());
			}

			CheckFiguresToRemove();
		}

		void CheckFiguresToRemove()
		{
			if (Figures.Count >= Settings.Default.ClearAfter)
				Device.BeginInvokeOnMainThread(() =>
				{
					Figures.RemoveAt(0);
				});

			for (int i = Figures.Count - 1; i >= 0; i--)
			{
				var shape = Figures[i];
				if (!shape.IsVisible)
				{
					Device.BeginInvokeOnMainThread(() =>
					{
						Figures.Remove(shape);
					});
				}
			}
		}

		Task AddLetter(char letter)
		{
			var nl = new LetterFigure(letter);
			return AddFigureAsync(nl);
		}

		Task AddNumber(int number)
		{
			var nf = new NumberFigure(number);
			return AddFigureAsync(nf);
		}

		async Task AddFigureAsync(Figure figure)
		{
			figure.StrokeColor = GetRandomColor();
			figure.FillColor = GetRandomColor();

			//TODO: what should this be
			var shapeWidth = Settings.Default.FontSize;
			var shapeHeight = Settings.Default.FontSize;

			var bounds = _interactionService.InteractionSize;
			var availableWidth = bounds.Width;
			var availableHeight = bounds.Height;

			figure.Size = new Size(shapeWidth, shapeHeight);
			var x = RandomBetweenTwoNumbers(0, Convert.ToInt32(availableWidth - figure.Size.Width));
			var y = RandomBetweenTwoNumbers(0, Convert.ToInt32(availableHeight - figure.Size.Height));
			figure.Position = new Point(x, y);


			// var nameFunc = hashTableOfFigureGenerators[Utils.RandomBetweenTwoNumbers(0, hashTableOfFigureGenerators.Count - 1)];
			Figures.Add(figure);
			await Speak(figure);
		}

		async Task Speak(Figure figure)
		{
			if (!Settings.Default.Speak)
				return;

			var shape = figure as ShapeFigure;
			if (shape != null)
			{
				var textToRead = _resourceService.GetLanguageTextForShape(shape.Type, _languageService.DefaultLanguage.Locale);
				if (textToRead == null)
					textToRead = shape.ToString();
				await _speakService.SpeakTextAsync(textToRead);
			}
			else {
				var textToRead = figure.ToString();
				if (_speakService.SupportsSSML)
				{
					await _speakService.SpeakSSMLAsync(_resourceService.GetLanguageTextForLetter(textToRead));
				}
				else
				{
					await _speakService.SpeakTextAsync(textToRead);
				}
			}
		}
	}
}
