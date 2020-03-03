using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mucomDotNET.Compiler;
using mucomDotNET.Driver;

namespace Mucom
{
	public class MucomAudioClip : System.IDisposable
	{
		private AudioClip _audioClip = null;
		private string _outputFileName = null;
		private Driver _driver = null;
		private MDSound.MDSound _mds = null;


		private static readonly int MucomSampleRate = 55467;
		private static readonly int OpnaMasterClock = 7987200;

		public MucomAudioClip(string workingDirectory, string songFilename)
		{
			try
			{
				var compiler = new Compiler();
				compiler.Init();
				var bin = compiler.Start(System.IO.Path.GetFullPath(songFilename), null);
				if (bin == null)
				{
					AvailableSongData = false;
					return;
				}
				System.IO.File.WriteAllBytes(compiler.OutFileName, bin);
				_outputFileName = compiler.OutFileName;
				Debug.Log(_outputFileName);
				var ym2608 = new MDSound.ym2608();
				var chip = new MDSound.MDSound.Chip
				{
					type = MDSound.MDSound.enmInstrumentType.YM2608,
					ID = 0,
					Instrument = ym2608,
					Update = ym2608.Update,
					Start = ym2608.Start,
					Stop = ym2608.Stop,
					Reset = ym2608.Reset,
					SamplingRate = (uint)MucomSampleRate,
					Clock = (uint)OpnaMasterClock,
					Volume = 0,
					Option = new object[] { System.Environment.CurrentDirectory }
				};
				_mds = new MDSound.MDSound((uint)MucomSampleRate, 1024, new MDSound.MDSound.Chip[] { chip });

				_driver = new Driver();
				_driver.Init(
					_outputFileName,
					dat => _mds.WriteYM2608(0, (byte)dat.port, (byte)dat.address, (byte)dat.data),
					(_, __) => { },
					false, true, false);

				_driver.StartRendering(MucomSampleRate, OpnaMasterClock);
				_driver.MusicSTART(0);

				_audioClip = AudioClip.Create(
					string.Format("mucom({0})", songFilename),
					int.MaxValue, 2, MucomSampleRate, true, OnPCMReaderCallback, OnPCMSetPositionCallback);

				AvailableSongData = true;
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
			if (_driver != null)
			{
				_driver.StopRendering();
			}
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
				return string.Empty;
			}
		}

		private void OnPCMReaderCallback(float[] data)
		{
			var tmp = new short[data.Length];
			_mds.Update(tmp, 0, data.Length, _driver.Rendering);

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