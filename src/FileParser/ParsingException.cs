using System.Runtime.Serialization;

namespace FileParser
{
    [Serializable]
    public class ParsingException : Exception
    {
        private const string GenericMessage = "Exception triggered during parsing process";

        public ParsingException() : base(GenericMessage)
        {
        }

        public ParsingException(string message) : base(message ?? GenericMessage)
        {
        }

        public ParsingException(string message, Exception inner) : base(message ?? GenericMessage, inner)
        {
        }

        protected ParsingException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
