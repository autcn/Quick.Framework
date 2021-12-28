using System;

namespace Quick
{
    public static class CSharpTypeCategory
    {
        #region Readonly
        public static readonly Type[] IntegerTypes = { typeof(int), typeof(short),typeof(ushort),typeof(uint),typeof(long)
                                                ,typeof(ulong),typeof(byte)};

        public static readonly Type[] IntegerTypes8 = { typeof(byte) };

        public static readonly Type[] IntegerTypes16 = { typeof(short), typeof(ushort) };

        public static readonly Type[] IntegerTypes32 = { typeof(int), typeof(uint), };

        public static readonly Type[] IntegerTypes64 = { typeof(long), typeof(ulong), };

        public static readonly Type[] FloatTypes = { typeof(double), typeof(float), typeof(decimal) };

        public static readonly Type[] TextBoxTypes = { typeof(int), typeof(double), typeof(float), typeof(string),
                                                 typeof(short),typeof(ushort),typeof(uint),typeof(long)
                                                ,typeof(ulong),typeof(byte),typeof(decimal)};
        #endregion
    }
}
