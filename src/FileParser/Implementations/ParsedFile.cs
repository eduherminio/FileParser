using FileParser.Utils;
using System.Text;
using Print = System.Diagnostics.Debug;

namespace FileParser
{
    public class ParsedFile : Queue<IParsedLine>, IParsedFile
    {
        public bool Empty { get => Count == 0; }

        /// <summary>
        /// Parses a file
        /// </summary>
        /// <param name="parsedFile"></param>
        public ParsedFile(IEnumerable<IParsedLine> parsedFile)
            : base(new Queue<IParsedLine>(parsedFile))
        {
        }

        /// <summary>
        /// Parses a file
        /// </summary>
        /// <param name="parsedFile"></param>
        public ParsedFile(Queue<IParsedLine> parsedFile)
            : base(parsedFile)
        {
        }

        /// <summary>
        /// Parses a file
        /// </summary>
        /// <param name="path">FilePath</param>
        /// <param name="existingSeparator">Word separator (space by default)</param>
        /// <param name="ignoreEmptyItems"></param>
        public ParsedFile(string path, char[] existingSeparator, bool ignoreEmptyItems = true)
            : this(path, new string(existingSeparator), ignoreEmptyItems)
        {
        }

        /// <summary>
        /// Parses a file
        /// </summary>
        /// <param name="path">FilePath</param>
        /// <param name="existingSeparator">Word separator (space by default)</param>
        /// <param name="ignoreEmptyItems"></param>
        public ParsedFile(string path, string? existingSeparator = null, bool ignoreEmptyItems = true)
            : base(ParseFile(path, ignoreEmptyItems, existingSeparator))
        {
        }

        public IParsedLine NextLine()
        {
            return !Empty
                ? Dequeue()
                : throw new ParsingException("End of ParsedFile reached");
        }

        public IParsedLine PeekNextLine()
        {
            return !Empty
                ? Peek()
                : throw new ParsingException("End of ParsedFile reached");
        }

        public IParsedLine LineAt(int index)
        {
            return this.ElementAt(index);
        }

        public IParsedLine LastLine()
        {
            return this.Last();
        }

        public List<T> ToList<T>(string? lineSeparatorToAdd = null)
        {
            List<T> list = [];

            if (!string.IsNullOrEmpty(lineSeparatorToAdd))
            {
                foreach (IParsedLine parsedLine in this)
                {
                    parsedLine.Append(lineSeparatorToAdd);
                }
            }

            while (!Empty)
            {
                list.AddRange(NextLine().ToList<T>());
            }

            return list;
        }

        public string ToSingleString(string wordSeparator = " ", string? lineSeparator = null)
        {
            StringBuilder stringBuilder = new();

            while (!Empty)
            {
                stringBuilder.Append(NextLine().ToSingleString(wordSeparator));

                if (!Empty)
                {
                    stringBuilder.Append(!string.IsNullOrEmpty(lineSeparator)
                        ? lineSeparator
                        : wordSeparator);
                }
            }

            return stringBuilder.ToString();
        }

        public void Append(IParsedLine parsedLine) => Enqueue(parsedLine);

        public static List<List<string>> ReadAllGroupsOfLines(string path)
        {
            var currentGroup = new List<string>();
            var result = new List<List<string>>()
            {
                currentGroup
            };

            using (var streamReader = new StreamReader(path))
            {
                string? line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    if (line.Length == 0)
                    {
                        if (currentGroup.Count != 0)
                        {
                            currentGroup = [];
                            result.Add(currentGroup);
                        }
                    }
                    else
                    {
                        currentGroup.Add(line);
                    }
                }
            }

            if (currentGroup.Count == 0)
            {
                result.Remove(currentGroup);
            }

            return result;
        }

        public static List<List<T>> ReadAllGroupsOfLines<T>(string path)
        {
            TypeValidator.ValidateSupportedType<T>();

            if (typeof(T) == typeof(char))
            {
                throw new NotSupportedException($"{nameof(ReadAllGroupsOfLines)}<T> does not support T = char, " +
                    "use ReadAllGroupsOfLines<string> instead and split the strings yourself");
            }

            if (typeof(T) == typeof(string))
            {
                throw new NotSupportedException($"{nameof(ReadAllGroupsOfLines)}<T> does not support T = string, " +
                    "use non-generic ReadAllGroupsOfLines instead");
            }

            var currentGroup = new List<T>();
            var result = new List<List<T>>()
            {
                currentGroup
            };

            using (var streamReader = new StreamReader(path))
            {
                string? line;
                while ((line = streamReader.ReadLine()) != null)
                {
                    if (line.Length == 0)
                    {
                        if (currentGroup.Count != 0)
                        {
                            currentGroup = [];
                            result.Add(currentGroup);
                        }
                    }
                    else
                    {
                        currentGroup.Add(StringConverter.Convert<T>(line));
                    }
                }
            }

            if (currentGroup.Count == 0)
            {
                result.Remove(currentGroup);
            }

            return result;
        }

        #region Private methods

        /// <summary>
        /// Parses a file into a Queue&lt;IParsedLine&gt;
        /// Queue&lt;IParsedLine&gt; ~~ Queues of 'words' inside of a queue of lines
        /// </summary>
        /// <param name="path"></param>
        /// <param name="ignoreEmptyItems"></param>
        /// <param name="existingSeparator">Word separator</param>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="DirectoryNotFoundException"></exception>
        /// <exception cref="IOException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <returns></returns>
        private static Queue<IParsedLine> ParseFile(string path, bool ignoreEmptyItems, string? existingSeparator = null)
        {
            Queue<IParsedLine> parsedFile = new();

            try
            {
                using (StreamReader reader = new(path))
                {
                    while (!reader.EndOfStream)
                    {
                        string? original_line = reader.ReadLine();

                        // TODO: Evaluate if is it worth giving the user the option of detecting these kind of lines?
                        if (string.IsNullOrWhiteSpace(original_line))
                        {
                            continue;
                        }
                        // end TODO

                        IParsedLine parsedLine = new ParsedLine(ProcessLine(original_line, existingSeparator, ignoreEmptyItems));
                        parsedFile.Enqueue(parsedLine);
                    }
                }

                return parsedFile;
            }
            catch (Exception e)
            {
                Print.WriteLine(e.Message);
                Print.WriteLine("(path: {0}", path);
                throw;
            }
        }

        private static ICollection<string> ProcessLine(string original_line, string? separator, bool ignoreEmptyItems)
        {
            List<string> wordsInLine = original_line
                .Split(separator?.ToCharArray())
                .Select(str => str.Trim()).ToList();

            if (ignoreEmptyItems)
            {
                wordsInLine.RemoveAll(string.IsNullOrWhiteSpace);
            }

            return wordsInLine;
        }

        #endregion
    }
}
