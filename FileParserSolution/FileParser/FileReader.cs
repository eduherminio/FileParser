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
        /// <summary>
        /// Parses a file into an ICollection<string>, optionally separating lines with a given string
        /// </summary>
        /// <param name="path"></param>
        /// <param name="existingSeparator"></param>
        /// <param name="lineSeparatorToAdd"></param>
        /// <returns></returns>
        static public ICollection<string> ParseFile(string path, char[] existingSeparator = null, string lineSeparatorToAdd = null)
        {
            List<string> ParsedFile = new List<string>();

            try
            {
                StreamReader reader = new StreamReader(path);

                using (reader)
                {
                    string original_line;
                    while (!string.IsNullOrEmpty(original_line = reader.ReadLine()))
                    {
                        ParsedFile.AddRange(ProcessLine(original_line, existingSeparator));
                        if (lineSeparatorToAdd != null)
                            ParsedFile.Add(lineSeparatorToAdd);
                    }
                }

                return ParsedFile;
            }
            catch (FileNotFoundException e)
            {
                Print.WriteLine("File cannot be found in path {0}", path);
                throw e;
            }
            catch (DirectoryNotFoundException e)
            {
                Print.WriteLine("Invalid directory in file path, {0}", path);
                throw e;
            }
            catch (IOException e)
            {
                Print.WriteLine("File at the end of {0} cannot be opened", path);
                throw e;
            }
            catch (ArgumentException e)
            {
                Print.WriteLine(e.Message);
                throw e;
            }
            catch (Exception e)
            {
                Print.WriteLine("Internal error: " + e.Message);
                throw e;
            }
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
            catch (FileNotFoundException e)
            {
                Print.WriteLine("File cannot be found in path {0}", path);
                throw e;
            }
            catch (DirectoryNotFoundException e)
            {
                Print.WriteLine("Invalid directory in file path, {0}", path);
                throw e;
            }
            catch (IOException e)
            {
                Print.WriteLine("File at the end of {0} cannot be opened", path);
                throw e;
            }
            catch (ArgumentException e)
            {
                Print.WriteLine(e.Message);
                throw e;
            }
            catch (Exception e)
            {
                Print.WriteLine("Internal error: " + e.Message);
                throw e;
            }
        }

        static private ICollection<string> ProcessLine(string original_line, char[] separator)
        {
            List<string> wordsInLine = original_line
                .Split(separator)
                .Select(str => str.Trim()).ToList();    // Probably not needed, but just in case

            wordsInLine.RemoveAll(string.IsNullOrWhiteSpace);

            return wordsInLine;
        }


        /// <summary>
        /// Parses a line of a file into an ICollection<T>
        /// Default separator: WhiteSpace
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        static public ICollection<T> ParseArray<T>(string path, char[] separator = null)
        {
            if (!StringConverter.SupportedTypes.Contains(typeof(T)))
                throw new NotSupportedException("Parsing to " + typeof(T).ToString() + " is not supported yet");

            ICollection<string> wordsInLine = new List<string>(ParseLine(path, separator));

            return wordsInLine.Select(str => StringConverter.Convert<T>(str)).ToList();
        }

        /// <summary>
        /// Extracts the next element of a Queue of strings, converting it to T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="wordsInLine"></param>
        /// <returns></returns>
        static public T Extract<T>(Queue<string> wordsInLine)
        {
            if (!StringConverter.SupportedTypes.Contains(typeof(T)))
                throw new NotSupportedException("Parsing to " + typeof(T).ToString() + "is not suppoerted yet");

            string stringToConvert = wordsInLine.Dequeue();

            return StringConverter.Convert<T>(stringToConvert);
        }

        /// <summary>
        /// Parses a file into a LineByLineParsedFile instance
        /// </summary>
        /// <param name="path"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        static public LineByLineParsedFile ParseFileLineByLine(string path, char[] separator = null)
        {
            string lineSeparator = Guid.NewGuid().ToString();

            ICollection<string> parsedFileAsAWhhole = ParseFile(path, separator, lineSeparator);
            LineByLineParsedFile parsedFile = new LineByLineParsedFile(parsedFileAsAWhhole, lineSeparator);

            return parsedFile;
        }
    }
}
