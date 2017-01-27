using System.Collections.Generic;
using ababyc.Models;

namespace ababyc.Services
{
	public interface ILanguageService
	{
		string GetLanguageTextForLetter(string letter);
		string GetLanguageTextForShape(ShapeType shape);

		IList<Language> GetLanguages();

		Language DefaultLanguage
		{
			get;
		}

		string GetResourceText(string key);
	}
}
