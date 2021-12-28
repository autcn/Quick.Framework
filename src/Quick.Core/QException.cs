using System;
using System.Runtime.Serialization;

namespace Quick
{
    /// <summary>
    /// Base exception type for those are thrown by SiS system for SiS specific exceptions.
    /// </summary>
    public class QException : Exception
    {
        /// <summary>
        /// Creates a new <see cref="QException"/> object.
        /// </summary>
        public QException()
        {

        }

        /// <summary>
        /// Creates a new <see cref="QException"/> object.
        /// </summary>
        /// <param name="message">Exception message</param>
        public QException(string message)
            : base(message)
        {

        }

        /// <summary>
        /// Creates a new <see cref="QException"/> object.
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="innerException">Inner exception</param>
        public QException(string message, Exception innerException)
            : base(message, innerException)
        {

        }

        /// <summary>
        /// Constructor for serializing.
        /// </summary>
        public QException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {

        }
    }
}
