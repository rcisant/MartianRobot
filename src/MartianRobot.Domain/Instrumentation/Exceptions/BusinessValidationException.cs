using System;
using System.Runtime.Serialization;

namespace MartianRobot.Domain.Instrumentation.Exceptions
{
    [Serializable]
    public class BusinessValidationException : Exception
    {
        public const string DefaultMessage = "The object contain one or more validation exceptions";

        public BusinessValidationException() : base(DefaultMessage) { }

        public BusinessValidationException(string message) : base(message) { }

        public BusinessValidationException(string message, Exception innerException) : base(message, innerException) { }

        protected BusinessValidationException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
