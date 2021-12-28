using System;

namespace Quick
{
    public static class Utf8Helper
    {

        private static bool HasBom(byte[] bytes)
        {
            if (bytes.Length < 3)
            {
                return false;
            }

            return bytes[0] == 0xEF && bytes[1] == 0xBB && bytes[2] == 0xBF;
        }
        public static byte[] RemoveBom(byte[] array)
        {
            if (!HasBom(array))
                return array;

            byte[] newArray = new byte[array.Length - 3];
            Buffer.BlockCopy(array, 3, newArray, 0, newArray.Length);
            return newArray;
        }
    }
}
