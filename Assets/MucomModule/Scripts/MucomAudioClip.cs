using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mucom
{
	[RequireComponent(typeof(AudioSource))]
	public class MucomAudioSource : MonoBehaviour
	{
		private AudioSource _audioSource;
		private AudioClip _audioClip;
		private MucomModule _mucom;

		private readonly int MucomSampleRate = 55467;

		private void Awake()
		{
			_audioSource = GetComponent<AudioSource>();

			_mucom = new MucomModule();
			_mucom.SetRate(MucomSampleRate);

			_mucom.Open(@".", @"");
			_mucom.Play();

			_audioClip = AudioClip.Create(
				"mucom", int.MaxValue, 2, MucomSampleRate, true, OnPCMReaderCallback, OnPCMSetPositionCallback);
			_audioSource.clip = _audioClip;

			_audioSource.Play();
		}

		private void OnDestroy()
		{
			if (_audioClip != null)
			{
				Destroy(_audioClip);
			}

			_mucom?.Close();
			_mucom?.Dispose();
			_mucom = null;
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