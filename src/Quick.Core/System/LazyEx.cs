namespace System
{
    public static class LazyEx
    {
        public static T Get<T>(ref T reference, Func<T> constructor)
        {
            if (reference == null)
            {
                reference = constructor();
            }
            return reference;
        }
    }
}
