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
		static string s_defaultSSML;

		static ResourceManager s_resourceManager = new ResourceManager("ababyc.Data.Languages.ResourcesBabySmash", typeof(ILanguageService).Assembly);
		Language _defaultLanguage;
		List<Language> _availableLanguages;

		public LanguageService()
		{
			GetDefaultSSML();
		}

		public Language DefaultLanguage => _defaultLanguage ?? (_defaultLanguage = GetLanguages().FirstOrDefault(v => v.Locale == AVSpeechSynthesisVoice.CurrentLanguageCode));


		public IList<Language> GetLanguages() => _availableLanguages ?? (_availableLanguages = AVSpeechSynthesisVoice.GetSpeechVoices().Select(v => new Language { Id = v.Identifier, Locale = v.Language, FriendlyName = v.Language }).ToList());


		public string GetLanguageTextForLetter(string letter)
		{
			return string.Format(s_defaultSSML, letter);
		}

		public string GetLanguageTextForShape(ShapeType shape)
		{
			var key = shape.ToString().ToLower();
			return GetResourceText(key);
		}

		public string GetResourceText(string key)
		{
			string result = s_resourceManager.GetString(key, new CultureInfo(DefaultLanguage.Locale));
			return result;
		}

		static void GetDefaultSSML()
		{
			var assembly = typeof(ILanguageService).Assembly;
			Stream stream = assembly.GetManifestResourceStream("ababyc.Data.ssml_character_default.xml");
			using (var reader = new StreamReader(stream))
			{
				s_defaultSSML = reader.ReadToEnd();
			}
		}
	}
}
