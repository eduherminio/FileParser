using System.Collections.Generic;

namespace FileParser
{
    public class ParsedFile : IParsedFile
    {
        private string _path;
        private char[] _existingSeparator;
        private string _lineSeparatorToAdd;

        private Queue<Queue<string>> _value;

        public int Count { get => _value.Count; }

        public bool Empty { get => Count == 0; }

        public ParsedFile(Queue<Queue<string>> parsedFile)
        {
            _value = parsedFile;
        }

        public ParsedFile(string path, char[] existingSeparator = null, string lineSeparatorToAdd = null)
        {
            _path = path;
            _existingSeparator = existingSeparator;
            _lineSeparatorToAdd = lineSeparatorToAdd;
            _value = FileReader.ParseFile(_path, _existingSeparator, _lineSeparatorToAdd);
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

        public List<T> ToList<T>()
        {
            List<T> list = new List<T>();

            while (!Empty)
                list.AddRange(NextLine().ToList<T>());

            return list;
        }
    }
}
