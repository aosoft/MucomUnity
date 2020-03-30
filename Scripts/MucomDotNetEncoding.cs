using mucomDotNET.Common;
using System;
using System.Text;

namespace MucomUnity
{
    public class MucomDotNetEncoding : iEncoding
    {
        private static Lazy<MucomDotNetEncoding> defaultEncoding;

        static MucomDotNetEncoding()
        {
            defaultEncoding = new Lazy<MucomDotNetEncoding>(() => new MucomDotNetEncoding(), true);
        }

        public static iEncoding Default => defaultEncoding.Value;

#if UNITY_EDITOR
        byte[] iEncoding.GetSjisArrayFromString(string utfString) => myEncoding.Default.GetSjisArrayFromString(utfString);

        string iEncoding.GetStringFromSjisArray(byte[] sjisArray) => myEncoding.Default.GetStringFromSjisArray(sjisArray);

        string iEncoding.GetStringFromSjisArray(byte[] sjisArray, int index, int count) => myEncoding.Default.GetStringFromSjisArray(sjisArray, index, count);
#else
        byte[] iEncoding.GetSjisArrayFromString(string utfString) => Encoding.UTF8.GetBytes(utfString);

        string iEncoding.GetStringFromSjisArray(byte[] sjisArray) => Encoding.UTF8.GetString(sjisArray);

        string iEncoding.GetStringFromSjisArray(byte[] sjisArray, int index, int count) => Encoding.UTF8.GetString(sjisArray, index, count);
#endif

        string iEncoding.GetStringFromUtfArray(byte[] utfArray) => Encoding.UTF8.GetString(utfArray);

        byte[] iEncoding.GetUtfArrayFromString(string utfString) => Encoding.UTF8.GetBytes(utfString);
    }
}
