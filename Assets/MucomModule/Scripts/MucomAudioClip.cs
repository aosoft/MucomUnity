using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mucom
{
	public class MucomAudioClip : System.IDisposable
	{
		private AudioClip _audioClip;
		private MucomModule _mucom;

		private readonly int MucomSampleRate = 55467;

		public MucomAudioClip(string workingDirectory, string songFilename)
		{
			try
			{
				_mucom = new MucomModule();
				_mucom.SetRate(MucomSampleRate);

				AvailableSongData = _mucom.Open(workingDirectory, songFilename);
				if (AvailableSongData)
				{
					_mucom.Play();
					_audioClip = AudioClip.Create(
						string.Format("mucom({0})", songFilename),
						int.MaxValue, 2, MucomSampleRate, true, OnPCMReaderCallback, OnPCMSetPositionCallback);
				}
			}
			catch
			{
				Dispose();
				throw;
			}
		}

		public void Dispose()
		{
			if (_audioClip != null)
			{
				Object.Destroy(_audioClip);
			}

			_mucom?.Close();
			_mucom?.Dispose();
			_mucom = null;
		}

		public AudioClip UnityAudioClip
		{
			get
			{
				return _audioClip;
			}
		}

		public bool AvailableSongData
		{
			get;
		}

		public string CompileResult
		{
			get
			{
				return _mucom.GetResult();
			}
		}

		private void OnPCMReaderCallback(float[] data)
		{
			if (_mucom == null)
			{
				return;
			}

			var tmp = new short[data.Length];
			_mucom.Mix(tmp, tmp.Length / 2);

			for (int i = 0; i < data.Length; i++)
			{
				data[i] = (float)tmp[i] / 32767.0f;
			}
		}

		private void OnPCMSetPositionCallback(int position)
		{
		}

	}
}