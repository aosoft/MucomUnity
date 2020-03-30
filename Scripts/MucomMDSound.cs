using System;
using System.IO;

namespace MucomUnity
{
    public class MucomMDSound
    {
		public static readonly int OpnaSampleRate = 55467;
		public static readonly int OpnaMasterClock = 7987200;

        public MucomMDSound(int sampleBufferSize, Func<string, Stream> appendFileReaderCallback, int sampleRate)
        {
			YM2608 = new MDSound.ym2608();
			var chip = new MDSound.MDSound.Chip
			{
				type = global::MDSound.MDSound.enmInstrumentType.YM2608,
				ID = 0,
				Instrument = YM2608,
				Update = YM2608.Update,
				Start = YM2608.Start,
				Stop = YM2608.Stop,
				Reset = YM2608.Reset,
				SamplingRate = (uint)sampleRate,
				Clock = (uint)OpnaMasterClock,
				Volume = 0,
				Option = new object[] { appendFileReaderCallback }
			};
			MDSound = new MDSound.MDSound((uint)sampleRate, (uint)sampleBufferSize, new MDSound.MDSound.Chip[] { chip });
			SampleRate = sampleRate;
			SampleBufferSize = sampleBufferSize;
		}

		public MucomMDSound(int sampleBufferSize, Func<string, Stream> appendFileReaderCallback) : this(sampleBufferSize, appendFileReaderCallback, OpnaSampleRate)
		{
		}


		public MDSound.ym2608 YM2608 { get; }
		public MDSound.MDSound MDSound { get; }

		public int SampleRate { get; }

		public int SampleBufferSize { get; }
	}
}
