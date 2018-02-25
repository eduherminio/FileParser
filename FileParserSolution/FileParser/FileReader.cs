using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

//using Print = System.Console;
using Print = System.Diagnostics.Debug;

namespace FileParser
{
    static public class FileReader
    {
        private const long _minimumElementsToUseConverter = 100000;

        /// <summary>
        /// Parses a file into a Queue<Queue<string>>, optionally separating lines with a given string
        /// Queue<Queue<string>> ~~ Queues of 'words' inside of a queue of lines
        /// </summary>
        /// <param name="path"></param>
        /// <param name="existingSeparator"></param>
        /// <param name="lineSeparatorToAdd"></param>
        /// <returns></returns>
        static public Queue<Queue<string>> ParseFile(string path, char[] existingSeparator = null, string lineSeparatorToAdd = null)
        {
            Queue<Queue<string>> parsedFile = new Queue<Queue<string>>();

            try
            {
                StreamReader reader = new StreamReader(path);

                using (reader)
                {
                    string original_line;
                    while (!string.IsNullOrEmpty(original_line = reader.ReadLine()))
                    {
                        Queue<string> parsedLine = new Queue<string>(ProcessLine(original_line, existingSeparator));
                        if (lineSeparatorToAdd != null)
                            parsedLine.Enqueue(lineSeparatorToAdd);

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
                throw e;
            }
        }

        /// <summary>
        /// Parses a file into an ICollection<string>, optionally separating lines with a given string
        /// </summary>
        /// <param name="path"></param>
        /// <param name="existingSeparator"></param>
        /// <param name="lineSeparatorToAdd"></param>
        /// <returns></returns>
        static public ICollection<string> ParseFileAsICollection(string path, char[] existingSeparator = null, string lineSeparatorToAdd = null)
        {
            List<string> parsedFileAsICollection = new List<string>();

            var parsedFile = ParseFile(path, existingSeparator, lineSeparatorToAdd);
            while (parsedFile.Count > 0)
                parsedFileAsICollection.AddRange(parsedFile.Dequeue().ToList());

            return parsedFileAsICollection;
        }

        /// <summary>
        /// Parses the first line of a file into an ICollection<string>
        /// Default separator: WhiteSpace
        /// </summary>
        /// <param name="path"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        static public ICollection<string> ParseLine(string path, char[] separator = null)
        {
            try
            {
                StreamReader reader = new StreamReader(path);

                using (reader)
                {
                    string original_line = reader.ReadLine();

                    return ProcessLine(original_line, separator);
                }

            }
            catch (Exception e)
            {
                // Possible exceptions:
                // FileNotFoundException, DirectoryNotFoundException, IOException, ArgumentException

                Print.WriteLine(e.Message);
                Print.WriteLine("(path: {0}", path);
                throw e;
            }
        }

        /// <summary>
        /// Parses a line of a file into an ICollection<T>
        /// Default separator: WhiteSpace
        /// Default minimum elements needed to avoid instantiating a TypeConverter for each conversion: 100000
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <param name="separator"></param>
        /// <param name="minimumElementsToUseConverter"></param>
        /// <returns></returns>
        static public ICollection<T> ParseArray<T>(string path, char[] separator = null, long minimumElementsToUseConverter = _minimumElementsToUseConverter)
        {
            if (!SupportedTypes.Contains(typeof(T)))
                throw new NotSupportedException("Parsing to " + typeof(T).ToString() + " is not supported yet");

            List<string> wordsInLine = new List<string>(ParseLine(path, separator));
            if (typeof(T) == typeof(string))
                return (ICollection<T>)wordsInLine.ToList();

            if (wordsInLine.Count < minimumElementsToUseConverter)
            {
                return wordsInLine.Select(str => StringConverter.Convert<T>(str)).ToList();
            }
            else
            {
                var converter = StringConverter.GetConverter<T>();
                return wordsInLine.Select(str => (T)converter.ConvertFrom(str)).ToList();
            }
        }

        /// <summary>
        /// Syntatic sugar of ParseArray<T>(string path, char[] separator = null, long minimumElementsToUseConverter = 100000)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <param name="minimumElementsToUseConverter"></param>
        /// <returns></returns>
        static public ICollection<T> ParseArray<T>(string path, long minimumElementsToUseConverter)
        {
            return ParseArray<T>(path, null, minimumElementsToUseConverter).ToList();
        }

        /// <summary>
        /// Returns next element of a Queue<string>, converting it to T and removing it from the Queue
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="wordsInLine"></param>
        /// <returns></returns>
        static public T Extract<T>(ref Queue<string> wordsInLine)
        {
            if (!SupportedTypes.Contains(typeof(T)))
                throw new NotSupportedException("Parsing to " + typeof(T).ToString() + " is not suppoerted yet");

            string stringToConvert = wordsInLine.Dequeue();

            if (typeof(T) == typeof(string))
            {
                return (T)(object)stringToConvert;
            }
            else if (typeof(T) == typeof(char))
            {
                if (wordsInLine.Count != 0)
                    throw new NotSupportedException("Extract<char> can only be used with one-length Queues" +
                        " Try using ExtractChar<string> instead, after parsing each string with Extract<string>(Queue<string>)");

                char nextChar = ExtractChar(ref stringToConvert);
                if (stringToConvert.Length > 0)
                    wordsInLine.Enqueue(stringToConvert);

                return (T)(object)nextChar;
            }

            return StringConverter.Convert<T>(stringToConvert);
        }

        /// <summary>
        /// Returns next char of a string
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        static public char ExtractChar(ref string str)
        {
            if (string.IsNullOrEmpty(str))
                throw new ParsingException("String is empty");

            char nextChar = str.First();
            str = str.Substring(1);

            return nextChar;
        }

        /// <summary>
        /// Returns next element of a Queue<string>, converting it to T but WITHOUT removing it from the Queue
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="wordsInLine"></param>
        /// <returns></returns>
        static public T Peek<T>(Queue<string> wordsInLine)
        {
            if (!SupportedTypes.Contains(typeof(T)))
                throw new NotSupportedException("Parsing to " + typeof(T).ToString() + "is not suppoerted yet");

            string stringToConvert = wordsInLine.Peek();

            return StringConverter.Convert<T>(stringToConvert);
        }

        static private ICollection<string> ProcessLine(string original_line, char[] separator)
        {
            List<string> wordsInLine = original_line
                .Split(separator)
                .Select(str => str.Trim()).ToList();

            wordsInLine.RemoveAll(string.IsNullOrWhiteSpace);   // Probably not needed, but just in case

            return wordsInLine;
        }

        /// <summary>
        /// Supported parsing conversions
        /// </summary>
        static private HashSet<Type> SupportedTypes
        {
            get => new HashSet<Type>()
            {
                typeof(bool),
                typeof(char),
                typeof(string),
                typeof(short),
                typeof(int),
                typeof(long),
                typeof(double),
                typeof(object)
            };
        }
    }
}

