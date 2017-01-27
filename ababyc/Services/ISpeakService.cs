using System;
using System.Threading.Tasks;
using ababyc.Models;

namespace ababyc.Services
{
	public interface ISpeakService : IDisposable
	{
		Task SpeakTextAsync(string text);
		Task SpeakSSMLAsync(string text);
		Task SpeakUriStreamAsync(Uri url);
		void SetLanguage(Language language);
		bool SupportsSSML { get; }
	}
}
