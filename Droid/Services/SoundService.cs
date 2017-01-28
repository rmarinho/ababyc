using System;
using System.IO;
using System.Threading.Tasks;
using ababyc.Services;
using Android.Content.Res;
using Android.Media;
using Xamarin.Forms;

namespace ababyc.Droid
{
	public class SoundService : ISoundService
	{
		bool isPlayerPrepared;
		MediaPlayer _player;
		bool _disposed;

		public void Dispose()
		{
			if (_disposed && _player != null)
			{
				_disposed = true;
				_player.Dispose();
				_player = null;
			}
		}

		public void Play()
		{
			if (_player != null && !_player.IsPlaying)
			{
				_player?.Start();
			}
		}

		public void Stop()
		{
			if ((_player == null)) return;

			if (_player.IsPlaying)
				_player.Stop();

			_player.Release();
			_player = null;
		}
		public double Volume
		{
			get
			{
				return 0.5;
			}
			set
			{
				if (isPlayerPrepared)
					_player?.SetVolume((float)value, (float)value);
			}
		}

		public bool IsPlaying
		{
			get
			{
				return _player.IsPlaying;
			}
		}

		public Task PlayEmbebedResourceAsync(string path)
		{
			var assembly = typeof(ILanguageService).Assembly;
			var stream = assembly.GetManifestResourceStream(path);
			return PlayStreamAsync(stream, "");

		}

		public async Task PlayStreamAsync(System.IO.Stream stream, string mimeType)
		{
			if (_player == null)
			{
				_player = new MediaPlayer();
				_player.SetAudioStreamType(Android.Media.Stream.Music);
			}
			else
			{
				_player.Reset();
			}

			await _player.SetDataSourceAsync(new CustomAudioSource(stream));


			_player.Prepared += (s, e) =>
				{
					isPlayerPrepared = true;
					Play();
				};
			_player.PrepareAsync();

		}


		public async Task PlayUrlAsync(Uri url)
		{
			if (_player == null)
			{
				_player = new MediaPlayer();
				_player.SetAudioStreamType(Android.Media.Stream.Music);
			}
			else
			{
				_player.Reset();
			}

			await _player.SetDataSourceAsync(Forms.Context, Android.Net.Uri.Parse(url.ToString()));

			_player.Prepared += (s, e) =>
				{
					isPlayerPrepared = true;
					Play();
				};
			_player.PrepareAsync();
		}


		async Task StartPlayerAsyncFromAssetsFolder(AssetFileDescriptor fp)
		{

			if (_player == null)
			{
				_player = new MediaPlayer();
				_player.SetAudioStreamType(Android.Media.Stream.Music);
			}
			else
			{
				_player.Reset();
			}

			if (fp == null)
				throw new System.IO.FileNotFoundException("Make sure you set your file in the Assets folder");

			await _player.SetDataSourceAsync(fp.FileDescriptor, fp.StartOffset, fp.Length);
			_player.Prepared += (s, e) =>
				{
					isPlayerPrepared = true;
				};
			_player.PrepareAsync();
		}
	}

	class CustomAudioSource : MediaDataSource
	{
		byte[] _stream;
		public CustomAudioSource(System.IO.Stream stream)
		{
			using (MemoryStream ms = new MemoryStream())
			{
				stream.CopyTo(ms);
				_stream = ms.ToArray();
			}
		}

		public override long Size => _stream.Length;

		public override void Close()
		{
			_stream = null;
		}

		public override int ReadAt(long position, byte[] buffer, int offset, int size)
		{
			var length = _stream.Length;
			if (position >= length)
				return -1; // -1 indicates EOF
			if (position + size > length)
			{
				var ss = ((int)position + size) - length;
				size -= ss;

			}
			Buffer.BlockCopy(_stream, (int)position, buffer, offset, size);
			return size;
		}
	}
}
