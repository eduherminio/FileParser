using System.Collections.Generic;

namespace FileParser
{
    public class ParsedLine : IParsedLine
    {
        private Queue<string> Value { get; set; }

        public int Count { get => Value.Count; }

        public bool Empty { get => Count == 0; }

        public ParsedLine(Queue<string> parsedLine)
        {
            Value = parsedLine;
        }

        public T NextElement<T>()
        {
            return !Empty
                ? FileReader.Extract<T>(Value)
                : throw new ParsingException("End of ParsedLine reached");
        }

        public T PeekNextElement<T>()
        {
            return !Empty
                ? FileReader.Peek<T>(Value)
                : throw new ParsingException("End of ParsedLine reached");
        }
    }
}
