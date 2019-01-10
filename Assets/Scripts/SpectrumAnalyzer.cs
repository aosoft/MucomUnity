using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpectrumAnalyzer : MonoBehaviour
{
	public GameObject _barPrefab;
	public AudioSource _audioSource;

	private GameObject[] _meters = new GameObject[32];
	private float[] _meterValues = new float[32];
	private float[] _spectrumDataL = new float[1024];
	private float[] _spectrumDataR = new float[1024];

	private void Awake()
	{
		for (int i = 0; i < _meters.Length; i++)
		{
			_meters[i] = Instantiate(_barPrefab);
			_meters[i].transform.parent = this.transform;

			_meters[i].transform.localPosition = new Vector3(((float)i / _meters.Length) * 24.0f - 12.0f, 0.0f, 0.0f);
			_meterValues[i] = 0.0f;
		}
	}

	private void OnDestroy()
	{
		for (int i = 0; i < _meters.Length; i++)
		{
			if (_meters[i] != null)
			{
				Destroy(_meters[i]);
				_meters[i] = null;
			}
		}
	}


	// Update is called once per frame
	void Update()
	{
		var delta = Time.deltaTime * 10.0f;

		_audioSource.GetSpectrumData(_spectrumDataL, 0, FFTWindow.Hamming);
		_audioSource.GetSpectrumData(_spectrumDataR, 1, FFTWindow.Hamming);
		for (int i = 0; i < _meters.Length; i++)
		{
			var pos = _meters[i].transform.localPosition;
			var scale = _meters[i].transform.localScale;
			var i2 = Mathf.Min(1023, (int)Mathf.Pow(i, 2.0f));
			var f = (_spectrumDataL[i2] + _spectrumDataR[i2]) / 2.0f;

			if (f < _meterValues[i])
			{
				f = _meterValues[i];
			}
			else
			{
				_meterValues[i] = f;
			}

			scale.y = Mathf.Pow(f, 0.5f) * 50.0f;
			pos.y = scale.y / 2.0f;
			_meters[i].transform.localPosition = pos;
			_meters[i].transform.localScale = scale;

			_meterValues[i] -= (delta * f);
		}
	}
}
