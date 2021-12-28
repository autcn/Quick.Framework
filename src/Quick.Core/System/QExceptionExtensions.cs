using System.Runtime.ExceptionServices;

namespace System
{
    /// <summary>
    /// Extension methods for <see cref="Exception"/> class.
    /// </summary>
    public static class SiSExceptionExtensions
    {
        /// <summary>
        /// Uses <see cref="ExceptionDispatchInfo.Capture"/> method to re-throws exception
        /// while preserving stack trace.
        /// </summary>
        /// <param name="exception">Exception to be re-thrown</param>
        public static void ReThrow(this Exception exception)
        {
            ExceptionDispatchInfo.Capture(exception).Throw();
        }
        public static string GetAllMessage(this Exception exception)
        {
            string message = "";
            Exception curException = exception;
            while (true)
            {
                if (curException == null)
                {
                    break;
                }
                if (message != "")
                {
                    message += "\r\n";
                }
                message += curException.Message;
                curException = curException.InnerException;
            }
            return message;
        }
    }
}