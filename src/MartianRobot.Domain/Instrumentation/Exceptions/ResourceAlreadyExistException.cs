using System;
using System.Runtime.Serialization;

namespace MartianRobot.Domain.Instrumentation.Exceptions
{
    [Serializable]
    public class ResourceAlreadyExistException : Exception
    {
        public const string DefaultMessage = "The resource already exist in the database";

        public ResourceAlreadyExistException() : base(DefaultMessage) { }

        public ResourceAlreadyExistException(string message) : base(message) { }

        public ResourceAlreadyExistException(string message, Exception exception) : base(message, exception) { }

        protected ResourceAlreadyExistException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
