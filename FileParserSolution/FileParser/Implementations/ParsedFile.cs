using System.Collections.Generic;

namespace FileParser
{
    public class ParsedFile : IParsedFile
    {
        private readonly Queue<Queue<string>> _value;

        public int Count { get => _value.Count; }

        public bool Empty { get => Count == 0; }

        public ParsedFile(Queue<Queue<string>> parsedFile)
        {
            _value = parsedFile;
        }

        public ParsedFile(string path, char[] existingSeparator = null, string lineSeparatorToAdd = null)
        {
            _value = FileReader.ParseFile(path, existingSeparator);
        }

        public IParsedLine NextLine()
        {
            return _value.Count != 0
                ? new ParsedLine(_value.Dequeue())
                : throw new ParsingException("End of ParsedFile reached");
        }

        public IParsedLine PeekNextLine()
        {
            return _value.Count != 0
                ? new ParsedLine(_value.Peek())
                : throw new ParsingException("End of ParsedFile reached");
        }

        public List<T> ToList<T>(string lineSeparatorToAdd = null)
        {
            List<T> list = new List<T>();

            if (!string.IsNullOrEmpty(lineSeparatorToAdd))
            {
                foreach (Queue<string> queue in _value)
                    queue.Enqueue(lineSeparatorToAdd);
            }

            while (!Empty)
                list.AddRange(NextLine().ToList<T>());

            return list;
        }
    }
}
