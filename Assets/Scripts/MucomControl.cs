using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mucom;
using UniRx;
using System.IO;

public class MucomControl : MonoBehaviour
{
	public AudioSource _audioSource;
	public InputField _text;
	public Button _compileButton;
	public Button _stopButton;

	private MucomAudioClip _audioClip;

	private void Awake()
	{
		_compileButton.OnClickAsObservable().Subscribe(_ =>
		{
			_audioSource.Stop();

			using (var w = new StreamWriter("mucombin.muc", false, System.Text.Encoding.GetEncoding(932)))
			{
				w.Write(_text.text);
			}

			_audioSource.clip = null;
			_audioClip?.Dispose();
			_audioClip = null;

			var audioClip = new MucomAudioClip(".", "mucombin.muc");
			try
			{
				if (audioClip.AvailableSongData)
				{
					Debug.LogError(audioClip.CompileResult);
					_audioSource.clip = audioClip.UnityAudioClip;
					_audioSource.Play();
				}
				else
				{
					Debug.LogError(audioClip.CompileResult);
					audioClip.Dispose();
				}
			}
			catch
			{
				audioClip.Dispose();
				throw;
			}
		}).AddTo(this);

		_stopButton.OnClickAsObservable().Subscribe(_ =>
		{
			_audioSource.Stop();
		}).AddTo(this);

	}

	private void OnDestroy()
	{
		_audioClip?.Dispose();
		_audioClip = null;
	}
}
