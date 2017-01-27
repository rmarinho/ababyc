using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using ababyc.Models;
using ababyc.Services;
using AVFoundation;

namespace ababyc.iOS
{
	public class SpeakService : ISpeakService
	{
		const string DEFAULT_LOCALE = "en-US";
		static AVSpeechSynthesisVoice _voice = AVSpeechSynthesisVoice.FromLanguage(DEFAULT_LOCALE);
		static AVSpeechSynthesizer _speechSynthesizer = new AVSpeechSynthesizer();

		public void Dispose()
		{
			_voice?.Dispose();
			_voice = null;
		}

		public bool SupportsSSML => false;

		public void SetLanguage(Language language)
		{
			string locale = DEFAULT_LOCALE;
			_voice = AVSpeechSynthesisVoice.FromLanguage(locale) ?? AVSpeechSynthesisVoice.FromLanguage(DEFAULT_LOCALE);
		}

		public Task SpeakSSMLAsync(string text)
		{
			throw new NotImplementedException();
		}

		public Task SpeakTextAsync(string text)
		{
			return Task.Run(() =>
			{
				var speechUtterance = new AVSpeechUtterance(text.ToLower())
				{
					Rate = AVSpeechUtterance.MaximumSpeechRate / 4,
					Voice = _voice,
					Volume = 0.5f,
					PitchMultiplier = 1.0f
				};
				_speechSynthesizer.SpeakUtterance(speechUtterance);
			});
		}

		public Task SpeakUriStreamAsync(Uri url)
		{
			throw new NotImplementedException();
		}

		public IEnumerable<string> GetInstalledLanguages()
		{
			return AVSpeechSynthesisVoice.GetSpeechVoices().Select(a => a.Language).Distinct();
		}
	}
}
