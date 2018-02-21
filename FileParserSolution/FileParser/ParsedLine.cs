using System.Collections.Generic;

namespace FileParser
{
    public class ParsedLine : IParsedLine
    {
        private Queue<string> _value;

        public int Count { get => _value.Count; }

        public bool Empty { get => Count == 0; }

        public ParsedLine(Queue<string> parsedLine)
        {
            _value = parsedLine;
        }

        public T NextElement<T>()
        {
            return !Empty
                ? FileReader.Extract<T>(ref _value)
                : throw new ParsingException("End of ParsedLine reached");
        }

        public T PeekNextElement<T>()
        {
            return !Empty
                ? FileReader.Peek<T>(_value)
                : throw new ParsingException("End of ParsedLine reached");
        }
    }
}
