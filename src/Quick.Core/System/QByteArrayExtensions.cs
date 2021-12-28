using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace System
{
    public static class QByteArrayExtensions
    {
        public static string ToHexString(this byte[] data)
        {
            var sb = new StringBuilder();
            foreach (var hashByte in data)
            {
                sb.Append(hashByte.ToString("X2"));
            }

            return sb.ToString();
        }

        public static string ToMd5UpperString(this byte[] inputBytes)
        {
            using (var md5 = MD5.Create())
            {
                var hashBytes = md5.ComputeHash(inputBytes);

                var sb = new StringBuilder();
                foreach (var hashByte in hashBytes)
                {
                    sb.Append(hashByte.ToString("X2"));
                }

                return sb.ToString();
            }
        }
    }
}
