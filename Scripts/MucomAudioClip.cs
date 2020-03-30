using System.IO;
using System.Linq;
using mucomDotNET.Driver;
using musicDriverInterface;
using UnityEngine;

namespace MucomUnity
{
	public class MucomAudioClip : System.IDisposable
	{
		private Driver _driver = null;
		private MucomMDSound _mdsound = null;

		public MucomAudioClip(MucomMDSound mdsound, MmlDatum[] srcBuf, System.Func<string, Stream> appendFileReaderCallback)
		{
			try
			{
				_driver = new Driver();
				_driver.Init(
					dat => mdsound.MDSound.WriteYM2608(0, (byte)dat.port, (byte)dat.address, (byte)dat.data),
					(_, __) => { },
					srcBuf,
					new object[] { false, true, false },
					appendFileReaderCallback);

				_driver.StartRendering(mdsound.SampleRate, MucomMDSound.OpnaMasterClock);
				_driver.MusicSTART(0);

				UnityAudioClip = AudioClip.Create("mucom", int.MaxValue, 2, mdsound.SampleRate, true, OnPCMReaderCallback, OnPCMSetPositionCallback);
				_mdsound = mdsound;
			}
			catch
			{
				Dispose();
				throw;
			}
		}

		public MucomAudioClip(MucomMDSound mdsound, byte[] srcBuf, System.Func<string, Stream> appendFileReaderCallback) :
			this(mdsound, srcBuf.Select(x => new MmlDatum(x)).ToArray(), appendFileReaderCallback)
		{
		}

		public void Dispose()
		{
			if (UnityAudioClip != null)
			{
				Object.Destroy(UnityAudioClip);
				UnityAudioClip = null;
			}
			if (_driver != null)
			{
				_driver.StopRendering();
			}
		}

		public AudioClip UnityAudioClip { get; private set; }

		private void OnPCMReaderCallback(float[] data)
		{
			var tmp = new short[data.Length];
			_mdsound.MDSound.Update(tmp, 0, data.Length, _driver.Rendering);

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