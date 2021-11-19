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
        public ParsedFile(string path, char[] existingSeparator)
            : this(path, new string(existingSeparator))
        {
        }

        /// <summary>
        /// Parses a file
        /// </summary>
        /// <param name="path">FilePath</param>
        /// <param name="existingSeparator">Word separator (space by default)</param>
        public ParsedFile(string path, string? existingSeparator = null)
            : base(ParseFile(path, existingSeparator))
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
            List<T> list = new();

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
            return File.ReadAllText(path)
                .Replace("\r\n", "\n")
                .Split("\n\n")
                .Where(str => !string.IsNullOrEmpty(str))
                .Select(str => str.Split('\n', StringSplitOptions.RemoveEmptyEntries).ToList())
                .ToList();
        }

        #region Private methods

        /// <summary>
        /// Parses a file into a Queue&lt;IParsedLine&gt;
        /// Queue&lt;IParsedLine&gt; ~~ Queues of 'words' inside of a queue of lines
        /// </summary>
        /// <param name="path"></param>
        /// <param name="existingSeparator">Word separator</param>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="DirectoryNotFoundException"></exception>
        /// <exception cref="IOException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <returns></returns>
        public static Queue<IParsedLine> ParseFile(string path, string? existingSeparator = null)
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

                        IParsedLine parsedLine = new ParsedLine(ProcessLine(original_line, existingSeparator));
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

        private static ICollection<string> ProcessLine(string original_line, string? separator)
        {
            List<string> wordsInLine = original_line
                .Split(separator?.ToCharArray())
                .Select(str => str.Trim()).ToList();

            wordsInLine.RemoveAll(string.IsNullOrWhiteSpace);   // Probably not needed, but just in case

            return wordsInLine;
        }

        #endregion
    }
}
