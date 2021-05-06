using System;
using System.Runtime.Serialization;

namespace MartianRobot.Domain.Instrumentation.Exceptions
{
    [Serializable]
    public class ResourceNotFoundException : Exception
    {
        public const string DefaultMessage = "The resource you were looking for does not longer exist";

        public ResourceNotFoundException() : base(DefaultMessage) { }

        public ResourceNotFoundException(string message) : base(message) { }

        public ResourceNotFoundException(string message, Exception exception) : base(message, exception) { }

        protected ResourceNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
