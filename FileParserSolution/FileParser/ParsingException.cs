namespace FileParser
{
    public class ParsingException : System.Exception
    {
        private const string _genericMessage = "Exception triggered during parsing process";

        public ParsingException() : base(_genericMessage)
        {
        }

        public ParsingException(string message) : base(message ?? _genericMessage)
        {
        }

        public ParsingException(string message, System.Exception inner) : base(message ?? _genericMessage, inner)
        {
        }
    }
}
