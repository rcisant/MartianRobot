using System;
using System.Runtime.Serialization;

namespace MartianRobot.Domain.Instrumentation.Exceptions
{
    [Serializable]
    public class ResourceAlreadyInUseException : Exception
    {
        public const string DefaultMessage = "The resource is already in use by other resources";

        public ResourceAlreadyInUseException() : base(DefaultMessage) { }

        public ResourceAlreadyInUseException(string message) : base(message) { }

        public ResourceAlreadyInUseException(string message, Exception innerException) : base(message, innerException) { }

        protected ResourceAlreadyInUseException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
