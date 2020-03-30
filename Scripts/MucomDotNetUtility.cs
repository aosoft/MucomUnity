using System.IO;
using mucomDotNET.Compiler;
using UnityEngine;

using MucomLog = musicDriverInterface.Log;
using MucomLogLevel = musicDriverInterface.LogLevel;

namespace MucomUnity
{
    public static class MucomDotNetUtility
    {
		public static void InitializeMucomLogger(bool setUnityLogger)
		{
			MucomLog.writeLine = setUnityLogger ? MucomLog_WriteLine : (System.Action<MucomLogLevel, string>)null;
		}

		public static Compiler CreateCompiler()
		{
			var ret = new Compiler(MucomDotNetEncoding.Default);
			ret.Init();
			return ret;
		}

		private static void MucomLog_WriteLine(MucomLogLevel level, string msg)
		{
			if ((level & (MucomLogLevel.FATAL | MucomLogLevel.ERROR)) != 0)
			{
				Debug.LogError(msg);
			}
			else if ((level & MucomLogLevel.WARNING) != 0)
			{
				Debug.LogWarning(msg);
			}
			else
			{
				Debug.Log(msg);
			}
		}

		public static Stream OpenFromStreamingAssets(string fileName)
		{
#if UNITY_ANDROID
			using (var www = new UnityEngine.Networking.UnityWebRequest(Path.Combine(Application.streamingAssetsPath, fileName)))
			{
				www.SendWebRequest();
				while (!www.isDone) ;
				var ret = new MemoryStream(www.downloadHandler.data.Length);
				ret.Write(www.downloadHandler.data, 0, www.downloadHandler.data.Length);
				ret.Seek(0, SeekOrigin.Begin);
				return ret;
			}
#else
			return new FileStream(Path.Combine(Application.streamingAssetsPath, fileName), FileMode.Open, FileAccess.Read);
#endif
		}
	}
}
