using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Resources;
using ababyc.Models;
using ababyc.Services;
using AVFoundation;
using System.Globalization;

namespace ababyc.iOS
{
	public class LanguageService : ILanguageService
	{
		static Language _defaultLanguage;
		static List<Language> _availableLanguages;

		public Language DefaultLanguage => _defaultLanguage ?? (_defaultLanguage = GetLanguages().FirstOrDefault(v => v.Locale == AVSpeechSynthesisVoice.CurrentLanguageCode));
		public IList<Language> GetLanguages() => _availableLanguages ?? (_availableLanguages = AVSpeechSynthesisVoice.GetSpeechVoices().Select(v => new Language { Id = v.Identifier, Locale = v.Language, FriendlyName = v.Language }).ToList());

	}
}