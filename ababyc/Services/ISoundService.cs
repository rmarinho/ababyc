using System;
using System.IO;
using System.Threading.Tasks;

namespace ababyc.Services
{
	public interface ISoundService : IDisposable
	{
		Task PlayUrlAsync(Uri url);
		Task PlayEmbebedResourceAsync(string path);
		Task PlayStreamAsync(Stream stream, string mimeType);
	}
}
