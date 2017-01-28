using System.Collections.Generic;
using ababyc.Models;

namespace ababyc.Services
{
	public interface ILanguageService
	{
		IList<Language> GetLanguages();

		Language DefaultLanguage { get; }
	}
}
