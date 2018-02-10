using System.IO;
using System.Collections.Generic;
using System.Linq;

//using Print = System.Console;
using Print = System.Diagnostics.Debug;

namespace FileParser
{
    public class FileReader : FileHandler
    {
        static private List<string> ParsedFile { get; set; } = new List<string>();

        static private void Init(string path)
        {
            _path = path;
            ParsedFile = new List<string>();
        }

        /// <summary>
        /// Default separator: WhiteSpace
        /// </summary>
        /// <param name="path"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        static public ICollection<string> Parse(string path, char[] separator = null)
        {
            Init(path);
            
            using (StreamReader reader = new StreamReader(_path))
            {
                string original_line;
                while(!string.IsNullOrEmpty(original_line = reader.ReadLine()))
                {
                    Print.WriteLine(original_line);
                    string trimmedLine = original_line.Trim();
                    //Print.WriteLine(trimmedLine);

                    if (string.IsNullOrWhiteSpace(trimmedLine))
                        continue;

                    List<string> wordsInLine = trimmedLine.Split(separator).ToList();
                    wordsInLine.RemoveAll(string.IsNullOrWhiteSpace);
                    //Print.Write("|");
                    wordsInLine.ForEach(trimmedWord => Print.Write("|" + trimmedWord));
                    Print.Write("|\n");

                    ParsedFile.AddRange(wordsInLine);
                }
            };

            return ParsedFile;
        }
    }
}
