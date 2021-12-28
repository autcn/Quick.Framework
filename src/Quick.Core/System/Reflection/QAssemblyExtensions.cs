using System.Diagnostics;
using System.IO;
using System.Text;

namespace System.Reflection
{
    public static class QAssemblyExtensions
    {
        public static string GetFileVersion(this Assembly assembly)
        {
            return FileVersionInfo.GetVersionInfo(assembly.Location).FileVersion;
        }

        public static string GetManifestResourceString(this Assembly assembly, string path, Encoding encoding)
        {
            Stream stream = assembly.GetManifestResourceStream(path);
            StreamReader reader = new StreamReader(stream, encoding);
            string strText = reader.ReadToEnd();
            reader.Close();
            return strText;
        }

        public static byte[] GetManifestResourceData(this Assembly assembly, string path)
        {
            Stream stream = assembly.GetManifestResourceStream(path);
            byte[] data = new byte[stream.Length];
            stream.Read(data, 0, data.Length);
            stream.Close();
            return data;
        }
    }
}
