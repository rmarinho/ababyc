using System;
using System.IO;
using System.Threading.Tasks;
using ababyc.Services;
using AVFoundation;
using Foundation;

namespace ababyc.iOS
{
	public class SoundService : ISoundService
	{
		public void Dispose()
		{

		}

		public Task PlayEmbebedResourceAsync(string path)
		{
			var assembly = typeof(ILanguageService).Assembly;
			using (var stream = assembly.GetManifestResourceStream(path))
				return PlayStreamAsync(stream, "");

		}

		public Task PlayStreamAsync(Stream stream, string mimeType)
		{
			var tcs = new TaskCompletionSource<bool>();
			var assembly = typeof(ILanguageService).Assembly;
			PlayStream(stream, tcs);
			return tcs.Task;
		}

		public Task PlayUrlAsync(Uri url)
		{
			var tcs = new TaskCompletionSource<bool>();
			using (var player = AVAudioPlayer.FromUrl(new NSUrl(url.ToString())))
				Play(player, tcs);
			return tcs.Task;
		}

		static void PlayStream(Stream stream, TaskCompletionSource<bool> tcs)
		{
			using (var player = AVAudioPlayer.FromData(NSData.FromStream(stream)))
				Play(player, tcs);
		}

		static void Play(AVAudioPlayer player, TaskCompletionSource<bool> tcs)
		{
			try
			{
				player.Play();
				player.FinishedPlaying += (sender, e) => tcs.SetResult(true);

			}
			catch (Exception ex)
			{
				tcs.SetException(ex);
			}
		}
	}
}
