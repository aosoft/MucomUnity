using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpectrumAnalyzer : MonoBehaviour
{
	public GameObject _barPrefab;
	public AudioSource _audioSource;

	private GameObject[] _meters = new GameObject[64];
	private float[] _spectrumData = new float[1024];

	private void Awake()
	{
		for (int i = 0; i < _meters.Length; i++)
		{
			_meters[i] = Instantiate(_barPrefab);
			_meters[i].transform.parent = this.transform;

			_meters[i].transform.localPosition = new Vector3(((float)i / _meters.Length) * 24.0f - 12.0f, 0.0f, 0.0f);
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
		_audioSource.GetSpectrumData(_spectrumData, 0, FFTWindow.Hamming);
		for (int i = 0; i < _meters.Length; i++)
		{
			var pos = _meters[i].transform.localPosition;
			var scale = _meters[i].transform.localScale;
			scale.y = _spectrumData[i] * 100.0f;
			pos.y = scale.y / 2.0f;
			_meters[i].transform.localPosition = pos;
			_meters[i].transform.localScale = scale;
			
		}
	}
}
