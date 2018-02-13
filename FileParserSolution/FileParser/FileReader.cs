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
        /// Default separator: WhiteSpace
        /// </summary>
        /// <param name="path"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        static public ICollection<string> Parse(string path, char[] separator = null)
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
                        string trimmedLine = original_line.Trim();

                        if (string.IsNullOrWhiteSpace(trimmedLine))
                            continue;

                        List<string> wordsInLine = trimmedLine.Split(separator).ToList();
                        wordsInLine.RemoveAll(string.IsNullOrWhiteSpace);

                        ParsedFile.AddRange(wordsInLine);
                    }
                }
            }
            catch(FileNotFoundException e)
            {
                Print.WriteLine("File cannot be found in path {0}", path);
                throw e;
            }
            catch(DirectoryNotFoundException e)
            {
                Print.WriteLine("Invalid directory in file path, {0}", path);
                throw e;
            }
            catch(IOException e)
            {
                Print.WriteLine("File at the end of {0} cannot be opened", path);
                throw e;
            }
            catch(ArgumentException e)
            {
                Print.WriteLine(e.Message);
                throw e;
            }
            catch(Exception e)
            {
                Print.WriteLine("Internal error: " + e.Message);
                throw e;
            }

            return ParsedFile;
        }

        static public ICollection<T> Parse<T>(string path, char[] separator = null)
        {
            List<T> result = new List<T>();

            ICollection<string> strings = Parse(path, separator);

            if (typeof(T) == typeof(string))
            {
                result = strings.Select( x=> (T)(object) x).ToList();
            }
            else if (typeof(T) == typeof(bool))
            {
                result.AddRange(strings.Select(str => (T)(object)bool.Parse(str)));
            }
            else if (typeof(T) == typeof(short))
            {
                result.AddRange(strings.Select(str => (T)(object)short.Parse(str)));
            }
            else if (typeof(T) == typeof(int))
            {
                result.AddRange(strings.Select(str => (T)(object)int.Parse(str)));
            }
            else if (typeof(T) == typeof(long))
            {
                result.AddRange(strings.Select(str => (T)(object)long.Parse(str)));
            }
            else if (typeof(T) == typeof(double))
            {
                result.AddRange(strings.Select(str => (T)(object)double.Parse(str)));
            }
            else
                throw new NotImplementedException("Type " + typeof(T).ToString() + "not implemented");

            return result;
        }
    }
}
