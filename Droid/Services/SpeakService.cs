using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ababyc.Models;
using ababyc.Services;
using Android.Speech.Tts;
using Xamarin.Forms;

namespace ababyc.Droid
{
	public class SpeakService : Java.Lang.Object, ISpeakService, TextToSpeech.IOnInitListener
	{
		TextToSpeech speaker;
		string toSpeak;

		public SpeakService()
		{
		}

		public bool SupportsSSML
		{
			get
			{
				return false;
			}
		}

		public void SetLanguage(Language language)
		{
			//throw new NotImplementedException();
		}

		public Task SpeakSSMLAsync(string text)
		{
			return Task.FromResult(true);
		}

		public void OnInit(OperationResult status)
		{
			if (status.Equals(OperationResult.Success))
			{
				var p = new Dictionary<string, string>();
				speaker.Speak(toSpeak, QueueMode.Flush, p);
			}
		}

		public Task SpeakTextAsync(string text)
		{
			return Task.Run(() =>
			{
				var ctx = Forms.Context; // useful for many Android SDK features
				toSpeak = text;
				if (speaker == null)
				{
					speaker = new TextToSpeech(ctx, this);
				}
				else {
					var p = new Dictionary<string, string>();
					speaker.Speak(toSpeak, QueueMode.Flush, p);
				}
			});
		}

		public Task SpeakUriStreamAsync(Uri url)
		{
			return Task.FromResult(true);
		}
	}
}
