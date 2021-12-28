namespace System.Threading.Tasks
{
    /// <summary>
    /// async方法的同步调用
    /// </summary>
    public static class TaskHelper
    {
        public static void WaitAsync(Func<Task> func)
        {
            Task.Run(() => { func().Wait(); }).Wait();
        }

        public static TResult WaitAsync<TResult>(Func<Task<TResult>> func)
        {
            return Task.Run(() => { return func().Result; }).Result;
        }
    }
}
