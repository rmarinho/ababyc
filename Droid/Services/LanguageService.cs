using System.Collections.Generic;
using System.Linq;
using ababyc.Models;
using ababyc.Services;
using Java.Util;

namespace ababyc.Droid
{
	public class LanguageService : ILanguageService
	{
		static Language _defaultLanguage;
		static List<Language> _availableLanguages;

		public Language DefaultLanguage => _defaultLanguage ?? (_defaultLanguage = GetLanguages().FirstOrDefault((arg) => arg.Id == Locale.Default.Language));
		public IList<Language> GetLanguages() => _availableLanguages ?? (_availableLanguages = Locale.GetAvailableLocales().Select((arg) => new Language { Id = arg.Language, Locale = arg.Language, FriendlyName = arg.DisplayLanguage }).ToList());

	}
}
