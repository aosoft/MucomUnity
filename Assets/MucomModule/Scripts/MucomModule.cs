using System;
using System.Runtime.InteropServices;

namespace Mucom
{
	public class MucomModule : IDisposable
	{
		[DllImport("MucomModule")]
		private static extern int CreateMucomModulePluginInstance(System.IntPtr[] buffer, int bufferSize);

		private delegate void FnAction(IntPtr self);

		[UnmanagedFunctionPointer(CallingConvention.Winapi, CharSet = CharSet.Ansi)]
		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool FnOpen(IntPtr self, string workingDirectory, string songFilename);

		public delegate void FnSetRate(IntPtr self, int rate);

		[UnmanagedFunctionPointer(CallingConvention.Winapi, CharSet = CharSet.Ansi)]
		public delegate void FnSetFile(IntPtr self, string file);

		public delegate void FnMix(IntPtr self, short[] buffer, int samples);

		[return: MarshalAs(UnmanagedType.U1)]
		public delegate bool FnPlay(IntPtr self);

		[UnmanagedFunctionPointer(CallingConvention.Winapi, CharSet = CharSet.Ansi)]
		public delegate string FnGetResult(IntPtr self);


		private IntPtr _self;
		private FnAction _fnDestroy;
		private FnOpen _fnOpen;
		private FnSetRate _fnSetRate;
		private FnSetFile _fnSetPCM;
		private FnSetFile _fnSetVoice;
		private FnMix _fnMix;
		private FnPlay _fnPlay;
		private FnAction _fnClose;
		private FnGetResult _fnGetResult;

		public MucomModule()
		{
			int bufferSize = CreateMucomModulePluginInstance(null, 0);
			var buffer = new IntPtr[bufferSize];
			if (CreateMucomModulePluginInstance(buffer, bufferSize) != bufferSize)
			{
				throw new Exception();
			}

			_self = buffer[0];
			_fnDestroy = Marshal.GetDelegateForFunctionPointer<FnAction>(buffer[1]);
			_fnOpen = Marshal.GetDelegateForFunctionPointer<FnOpen>(buffer[2]);
			_fnSetRate = Marshal.GetDelegateForFunctionPointer<FnSetRate>(buffer[3]);
			_fnSetPCM = Marshal.GetDelegateForFunctionPointer<FnSetFile>(buffer[4]);
			_fnSetVoice = Marshal.GetDelegateForFunctionPointer<FnSetFile>(buffer[5]);
			_fnMix = Marshal.GetDelegateForFunctionPointer<FnMix>(buffer[6]);
			_fnPlay = Marshal.GetDelegateForFunctionPointer<FnPlay>(buffer[7]);
			_fnClose = Marshal.GetDelegateForFunctionPointer<FnAction>(buffer[8]);
			_fnGetResult = Marshal.GetDelegateForFunctionPointer<FnGetResult>(buffer[9]);
		}

		public void Dispose()
		{
			_fnDestroy?.Invoke(_self);
			_fnDestroy = null;
			_self = IntPtr.Zero;
		}

		public bool Open(string workingDirectory, string songFilename)
		{
			return _fnOpen(_self, workingDirectory, songFilename);
		}

		public void SetRate(int rate)
		{
			_fnSetRate(_self, rate);
		}

		public void SetPCM(string file)
		{
			_fnSetPCM(_self, file);
		}

		public void SetVoice(string file)
		{
			_fnSetVoice(_self, file);
		}

		public void Mix(short[] buffer, int samples)
		{
			if (buffer.Length < samples * 2)
			{
				throw new ArgumentException();
			}
			_fnMix(_self, buffer, samples);
		}

		public bool Play()
		{
			return _fnPlay(_self);
		}

		public void Close()
		{
			_fnClose(_self);
		}

		public string GetResult()
		{
			return _fnGetResult(_self);
		}
	}
}
