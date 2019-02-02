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

        public ParsedFile(string path, char[] existingSeparator)
            : this(path, new string(existingSeparator))
        {
        }

        /// <summary>
        /// Parses a file
        /// </summary>
        /// <param name="path">FilePath</param>
        /// <param name="existingSeparator">Word separator (space by default)</param>
        public ParsedFile(string path, string existingSeparator = null)
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
                {
                    queue.Enqueue(lineSeparatorToAdd);
                }
            }

            while (!Empty)
            {
                list.AddRange(NextLine().ToList<T>());
            }

            return list;
        }

        public string ToSingleString(string wordSeparator = " ", string lineSeparator = null)
        {
            string lastingString = string.Empty;

            while (!Empty)
            {
                lastingString += NextLine().ToSingleString(wordSeparator);

                if (!Empty)
                {
                    lastingString += (!string.IsNullOrEmpty(lineSeparator) ? lineSeparator : wordSeparator);
                }
            }

            return lastingString;
        }
    }
}
