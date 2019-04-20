using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Print = System.Diagnostics.Debug;

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
            _value = ParseFile(path, existingSeparator);
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

        #region Private methods

        /// <summary>
        /// Parses a file into a Queue<Queue<string>>
        /// Queue<Queue<string>> ~~ Queues of 'words' inside of a queue of lines
        /// </summary>
        /// <param name="path"></param>
        /// <param name="existingSeparator">Word separator</param>
        /// <returns></returns>
        public static Queue<Queue<string>> ParseFile(string path, string existingSeparator = null)
        {
            Queue<Queue<string>> parsedFile = new Queue<Queue<string>>();

            try
            {
                StreamReader reader = new StreamReader(path);

                using (reader)
                {
                    while (!reader.EndOfStream)
                    {
                        string original_line = reader.ReadLine();
                        // TODO: Evaluate if is it worth giving the user the option of detecting these kind of lines?
                        if (string.IsNullOrWhiteSpace(original_line))
                        {
                            continue;
                        }
                        // end TODO
                        Queue<string> parsedLine = new Queue<string>(ProcessLine(original_line, existingSeparator));
                        parsedFile.Enqueue(parsedLine);
                    }
                }

                return parsedFile;
            }
            catch (Exception e)
            {
                // Possible exceptions:
                // FileNotFoundException, DirectoryNotFoundException, IOException, ArgumentException

                Print.WriteLine(e.Message);
                Print.WriteLine("(path: {0}", path);
                throw;
            }
        }

        private static ICollection<string> ProcessLine(string original_line, string separator)
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
