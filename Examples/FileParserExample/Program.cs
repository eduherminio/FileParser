// Process `SimpleInput.txt`, which contains an integer n(>= 0)
// followed by n doubles and a final string.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

using FileParser;

namespace FileParserSample
{
    class Program
    {
        static void Main(string[] args)
        {
            var cultureInfo = new CultureInfo("en-US");
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;

            List<double> listDouble = new List<double>();
            string str;

            // Input start
            IParsedFile file = new ParsedFile("SimpleInput.txt");
            IParsedLine firstLine = file.NextLine();

            int _integer = firstLine.NextElement<int>();

            for(int i=0; i<_integer; ++i)
                listDouble.Add(firstLine.NextElement<double>());

            str = firstLine.NextElement<string>();
            // Input end

            // Data Processing

            // Output start

            StreamWriter writer = new StreamWriter("..\\C#SimpleOutput.txt");
            using (writer)
            {
                writer.WriteLine(_integer + " " + String.Join(null, listDouble));
            }
        }
    }
}
