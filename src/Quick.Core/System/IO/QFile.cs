using System.Text;

namespace System.IO
{
    public static class QFile
    {
        public static byte[] ReadAllBytes(string filePath)
        {
            FileStream fileStream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            byte[] data = new byte[fileStream.Length];
            fileStream.Read(data, 0, data.Length);
            fileStream.Close();
            return data;
        }

        public static string ReadAllText(string filePath, Encoding encoding)
        {
            byte[] data = ReadAllBytes(filePath);
            return encoding.GetString(data);
        }

        public static string ReadAllText(string filePath)
        {
            return ReadAllText(filePath, Encoding.Default);
        }
    }
}
