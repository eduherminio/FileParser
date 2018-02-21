namespace FileParser
{
    public class ParsingException : System.Exception
    {
        private const string _parsingExceptionMessage = "Exception triggered during parsing process";

        public ParsingException()
        {
        }

        public ParsingException(string message) : base(message)
        {
        }

        public ParsingException(string message, System.Exception inner) : base(message, inner)
        {
        }
    }
}
